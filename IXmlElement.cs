using System.Xml.Linq;

namespace RevitXMLExtension
{
    public interface IXmlElement
    {
        XElement ToXmlElement();
    }
}
