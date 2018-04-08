using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace RevitXMLExtension
{
    public class XmlEntityType : IEnumerable<XmlParameter>, IXmlElement
    {
        public ObservableCollection<XmlParameter> XmlParameters { get; private set; } = new ObservableCollection<XmlParameter>();

        public string Name { get; private set; }

        public event ParameterAddedEventHandler ParameterAdded;

        public XmlEntityType(string name) => Name = name;

        public void AddParameter(XmlParameter xmlParameter)
        {
            var parameter = XmlParameters
                .Where(x => x.Guid == xmlParameter.Guid || x.Name == xmlParameter.Name)
                .Count();
            if (parameter == 0)
            {
                XmlParameters.Add(xmlParameter);
                ParameterAdded?.Invoke(this, new ParameterAddedEventArgs(xmlParameter));
            }
        }
        public XElement ToXmlElement() => 
            new XElement("Description", new XAttribute("Name", Name), XmlParameters.Select(x => x.ToXmlElement()));
        public static XmlEntityType Parse(XElement xElement)
        {
            XmlEntityType description = new XmlEntityType(
                xElement.Attribute("Name").Value);
            foreach (var item in xElement.Descendants("XmlParameter"))
                description.XmlParameters.Add(XmlParameter.Parse(item));
            return description;            
        }
        public IEnumerator<XmlParameter> GetEnumerator() => XmlParameters.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var item in XmlParameters)
            {
                hash += item.Guid.ToByteArray()
                    .Select(x => (x + 1) * (x + 2))
                    .Sum();
                hash += item.Name.Select(x => x + 1).Sum();
                hash += ((int)item.ValueType + 1) * ((int)item.ValueType + 1);
            }
            return hash;
        }
    }
}
