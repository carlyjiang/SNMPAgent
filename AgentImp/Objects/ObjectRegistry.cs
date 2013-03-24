using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib;

namespace Carl.Agent
{
    class ObjectRegistry
    {
        private static XDocument _doc;
        private static List<ObjectBase> _list = new List<ObjectBase>();

        internal List<ObjectBase> Objects
        {
            get
            {
                if(_list.Count == 0)
                {
                    GetAllObject();
                }
                return _list;
            }
        }


        private readonly string _filename = "ObjectConfig.xml";

        public string DefaultFileName
        {
            get
            {
                return _filename;
            }
        }

        public void Load(string filename)
        {
            _doc = XDocument.Load(filename);
        }

        public void Save(string filename)
        {
            if (_doc == null)
            {
                throw new ArgumentNullException("Xml document has not been load");
            }
            _doc.Save(filename);
        }

        public void AddNewObject(ObjectBase obj)
        {
            if (_doc.Descendants("object").Attributes("id").Where(o => o.Value == obj.GetId.ToString()).FirstOrDefault() != null)
            {
                throw new ArgumentException("id of the new object gets duplicate");
            }

            if (_doc.Descendants("object").Attributes("name").Where(o => o.Value == obj.Name).FirstOrDefault() != null)
            {
                throw new ArgumentException("name of the new object gets duplicate");
            }

            XElement element = new XElement("object",
                new XAttribute("id", obj.GetId),
                new XAttribute("name", obj.Name),
                new XAttribute("type", obj.GetType().ToString()),
                new XElement("description", obj.Description)
            );
            
            FieldInfo fieldinfo = obj.GetType().GetField("GetDataHandler", 
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            Delegate d = fieldinfo.GetValue(obj) as Delegate;
            if (d != null)
            {
                Delegate[] delegatelist = d.GetInvocationList();
                foreach (var v in delegatelist)
                {
                    element.Add(new XElement("delegate", v.Method.Name));
                    element.Add(new XElement("delegateObject", v.Method.ReflectedType));
                }
            }
            
            _doc.Element("objects").Add(element);
        }

        public void DeleteObject(String name)
        {
            if (_doc == null)
            {
                throw new ArgumentNullException("XML file not load");
            }
            var obj = _doc.Descendants("object").Where(o => o.Attribute("name").Value == name);
            obj.Remove<XElement>();
        }

        public List<ObjectBase> GetAllObject()
        {
            if (_doc == null)
            {
                throw new ArgumentNullException("XML file Not Load");
            }
            
            var docObjects = _doc.Descendants("object");
            foreach (var v in docObjects)
            {
                object[] objects = new object[2];
                objects[0] = v.Attribute("id").Value;
                objects[1] = v.Attribute("name").Value;
                ObjectBase ob = (ObjectBase)Activator.CreateInstance(Type.GetType(v.Attribute("type").Value), objects);

                ob.Description = v.Element("description").Value;

                if (v.Element("delegate") != null && v.Element("delegate").Value != String.Empty)
                {
                    EventInfo eventinfo = ob.GetType().GetEvent("GetDataHandler");
                    Delegate registeredMethod;

                    if (v.Element("delegateObject") != null 
                        && v.Element("delegateObject").Value != String.Empty
                        && v.Element("delegateObject").Value != "Carl.Agent.DataGetMethodFactory")
                    {
                        object objectCaller = (object)Activator.CreateInstance(Type.GetType(v.Element("delegateObject").Value));
                        
                        if (objectCaller.GetType().GetMethod(v.Element("delegate").Value).IsStatic)
                        {
                            registeredMethod = Delegate.CreateDelegate(
                                eventinfo.EventHandlerType,
                                objectCaller.GetType().GetMethod(v.Element("delegate").Value)
                            );
                        }
                        else
                        {
                            registeredMethod = Delegate.CreateDelegate(
                                eventinfo.EventHandlerType,
                                objectCaller,
                                objectCaller.GetType().GetMethod(v.Element("delegate").Value)
                            );
                        }
                    }
                    else
                    {
                        registeredMethod = Delegate.CreateDelegate(eventinfo.EventHandlerType,
                            DataGetMethodFactory.Instance,
                            DataGetMethodFactory.Instance.GetType().GetMethod(v.Element("delegate").Value)
                            );
                    }

                    eventinfo.AddEventHandler(ob, registeredMethod);
                }
                _list.Add(ob);
            }
            return _list;
        }

        public ObjectIdentifier GetIdByName(string name)
        {
            if (_doc == null)
            {
                throw new ArgumentNullException("XML file not load");
            }
            var id = _doc.Descendants("object").Where(o => o.Attribute("name").Value == name).FirstOrDefault().Attribute("id").Value;

            return new ObjectIdentifier(id);
        }

        public ObjectBase GetObjectByName(string name)
        {
            foreach (var v in _list)
            {
                if(v.Name == name)
                {
                    return v;
                }
            }
            return null;
        }

        public void test()
        {
            ObjectRegistry or = new ObjectRegistry();
            or.Load(or.DefaultFileName);

            //ObjectBase o1 = new IntegerObject("1.2.3.4.5.6", "integer");
            //o1.Description = "o3";
            //o1.GetDataHandler += DataGetMethodFactory.GetDataGetMethodFactory().GetMethodInteger32;

            //or.AddNewObject(o1);
            //or.Save(DefaultFileName);
            or.DeleteObject("integer");
            List<ObjectBase> ll = or.GetAllObject();
            foreach (var ob in ll)
            {
                Console.WriteLine("Object id {0}, name {1}, type {2}, description {3}", ob.GetId, ob.Name, ob.GetType(), ob.Description);
                try
                {
                    Console.WriteLine(ob.GetType());
                    Console.WriteLine(ob.Data);
                }
                catch
                { }
            }
        }
    }
}
