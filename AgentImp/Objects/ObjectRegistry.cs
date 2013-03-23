using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using Lextm.SharpSnmpLib.Pipeline;

namespace Carl.Agent
{
    class ObjectRegistry
    {
        private static XDocument _doc;
        private readonly string _filename = "ObjectConfig.xml";

        public string DefaultFileName
        {
            get
            {
                return _filename;
            }
        }

        private void Load(string filename)
        {
            _doc = XDocument.Load(filename);
        }

        private void Save(string filename)
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
            List<ObjectBase> list = new List<ObjectBase>();
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
                    Delegate registeredMethod = Delegate.CreateDelegate(eventinfo.EventHandlerType,
                        DataGetMethodFactory.GetDataGetMethodFactory(),
                        DataGetMethodFactory.GetDataGetMethodFactory().GetType().GetMethod(v.Element("delegate").Value)
                        );
                    eventinfo.AddEventHandler(ob, registeredMethod);
                }
                list.Add(ob);
            }
            return list;
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
