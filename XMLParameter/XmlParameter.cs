using System;
using System.Xml.Linq;

namespace RevitXMLExtension
{
    public class XmlParameter : IXmlElement
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public XmlParameterValueType ValueType { get; set; }
        public string ParameterDescription { get; set; }

        private XmlParameter() { }
        public XmlParameter(string name, Guid guid, XmlParameterValueType valueType, string description = "")
        {
            Name = name;
            Guid = guid;
            ValueType = valueType;
            ParameterDescription = description;
        }
        public XmlParameter(string name, string guid, XmlParameterValueType valueType, string description = "")
            : this(name, new Guid(guid), valueType, description)
        { }

        public XElement ToXmlElement()
        {
            return new XElement("XmlParameter",
                new XElement("Name", Name),
                new XElement("Guid", Guid),
                new XElement("ValueType", ValueType),
                new XElement("ParameterDescription", ParameterDescription));
        }
        public static XmlParameter Parse(XElement xElement)
        {
            return new XmlParameter
            {
                Name = xElement.Element("Name").Value,
                Guid = new Guid(xElement.Element("Guid").Value),
                ValueType = (XmlParameterValueType)Enum.Parse(typeof(XmlParameterValueType), xElement.Element("ValueType").Value),
                ParameterDescription = xElement.Element("ParameterDescription").Value
            };
        }
    }
}
