using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System;
using System.Linq;


namespace Carl.Agent
{
    class XmlParser
    {
        private XDocument _doc;

        public void test()
        {
            Load("Inventory.xml");

            AddMapping("item2", "target2", "desc2");
        }
        //bugs
        public XElement AddMapping(string name, string target, string description)
        {
            if (_doc != null)
            {
                XElement element = new XElement("mapping-item",
                    new XElement("name", name),
                    new XElement("target", target),
                    new XElement("description", description)
                );
                _doc.Element("objects").Add(element);
                return element;
            }
            else
            {
                throw new ArgumentNullException("XML document has not been loaded");
            }
        }

        public void AddElementTo(XElement parentElement, string name, string value)
        {
            if (parentElement != null)
            {
                XElement element = new XElement(name, value);
                parentElement.Add(element);
            }
            else
            {
                throw new ArgumentNullException("parent element is null");
            }
        }

        public IEnumerable<XElement> GetMappingByName(string name)
        {
            if (_doc != null)
            {
                var result =
                    from QueryName in _doc.Descendants("name")
                    where QueryName.Value == name
                    select QueryName.Parent;
                return result;
            }
            else
            {
                throw new ArgumentNullException("XML file has not been loaded");
            }
        }

        void Load(string filename)
        {
            _doc = XDocument.Load(filename);
        }

        void Save(string filename)
        {
            if (_doc != null)
                _doc.Save(filename);
        }
    }
}