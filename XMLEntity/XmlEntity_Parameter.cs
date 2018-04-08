using System.Xml.Linq;

namespace RevitXMLExtension
{
    public partial class XmlEntityInstance
    {
        private class Parameter : IXmlElement
        {
            private XmlParameter _xmlParameter;
            public object Value { get; set; }

            public string Key { get { return _xmlParameter.Name; } }
            public XmlParameterValueType GetValueType { get { return _xmlParameter.ValueType; } }

            public Parameter(XmlParameter xmlParameter) => _xmlParameter = xmlParameter;
            public XElement ToXmlElement() => new XElement(_xmlParameter.Name, Value);
        }
    }
}
