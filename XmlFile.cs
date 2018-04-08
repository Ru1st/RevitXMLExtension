using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RevitXMLExtension
{
    public class XmlFile : IXmlElement
    {
        private List<XmlEntityType> entityTypes = new List<XmlEntityType>();
        private List<XmlEntityInstance> entityInstance = new List<XmlEntityInstance>();

        public void AddEntityInstance(XmlEntityInstance entity)
        {
            var description = entityTypes
                .Where(x => x.Name == entity.Description.Name);
            if (description.Count() == 0)
                entityTypes.Add(entity.Description);
            if (entity.Description.GetHashCode() != description.First().GetHashCode())
                throw new ArgumentException();
            entityInstance.Add(entity);
        }
        public bool AddEntityTypes(XmlEntityType xmlEntityTypes)
        {
            var description = entityTypes
                .Where(x => x.Name == xmlEntityTypes.Name);
            if (description.Count() == 0)
            {
                entityTypes.Add(xmlEntityTypes);
                return true;
            }
            return false;
        }
        public IEnumerable<XmlEntityType> GetEntityTypes() => entityTypes;
        public IEnumerable<XmlEntityInstance> GetEntityInstances() => entityInstance;

        public static XmlFile Parse(XElement xElement)
        {
            if (xElement == null) throw new NullReferenceException();

            XmlFile xml = new XmlFile();
            var entities = new List<XmlEntityInstance>();
            foreach (var item in xElement.Descendants("Description"))
                xml.AddEntityTypes((XmlEntityType.Parse(item)));

            foreach (var item in xElement.Descendants("Entity"))
            {
                string name = item.Attribute("Name").Value;
                var description = xml.entityTypes
                    .Where(x => x.Name == name)
                    .First();
                XmlEntityInstance xmlEntity = new XmlEntityInstance(description);

                foreach (var parameter in description)
                {
                    var value = item.Element(parameter.Name).Value;
                    switch (parameter.ValueType)
                    {
                        case XmlParameterValueType.Double:
                            xmlEntity.SetParameterValue(parameter.Name, double.Parse(value));
                            break;
                        case XmlParameterValueType.Integer:
                            xmlEntity.SetParameterValue(parameter.Name, int.Parse(value));
                            break;
                        case XmlParameterValueType.String:
                            xmlEntity.SetParameterValue(parameter.Name, value);
                            break;
                    }
                }
                xml.AddEntityInstance(xmlEntity);
            }
            return xml;
        }

        public XElement ToXmlElement()
        {
            return new XElement("Root",
                new XElement("Descriptions",
                    entityTypes.Select(x => x.ToXmlElement())),
                new XElement("Entities",
                    entityInstance.Select(x => x.ToXmlElement())));
        }
    }
}