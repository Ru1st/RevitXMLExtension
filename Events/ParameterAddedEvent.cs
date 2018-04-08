using System;

namespace RevitXMLExtension
{
    public delegate void ParameterAddedEventHandler(object sender, ParameterAddedEventArgs e);

    public class ParameterAddedEventArgs : EventArgs
    {
        public XmlParameter AddedParameter { get; private set; }
        public ParameterAddedEventArgs(XmlParameter addedParameter) => AddedParameter = addedParameter;
    }
}
