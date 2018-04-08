using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RevitXMLExtension
{
    public partial class XmlEntityInstance : IXmlElement
    {
        private List<Parameter> parameters = new List<Parameter>();

        public XmlEntityType Description { get; private set; }

        public XmlEntityInstance(XmlEntityType description)
        {
            Description = description;
            foreach (var item in description)
                parameters.Add(new Parameter(item));
            description.ParameterAdded += OnParameterAdded;
        }

        public object GetParameterValue(string name)
        {
            string value = (string)GetParameter(name).Value;
            if (value == string.Empty)
            {
                switch (GetParameter(name).GetValueType)
                {
                    case XmlParameterValueType.Double:
                        return 0;
                    case XmlParameterValueType.Integer:
                        return 0;
                    case XmlParameterValueType.String:
                        return string.Empty;
                }
            }
            return value;
        }
        public void SetParameterValue(string name, double value) => GetParameter(name).Value = value;
        public void SetParameterValue(string name, int value) => GetParameter(name).Value = value;
        public void SetParameterValue(string name, string value)
        {
            CheckString(name, value);
            GetParameter(name).Value = value;
        }

        private void OnParameterAdded(object sender, ParameterAddedEventArgs args)
        {
            parameters.Add(new Parameter(args.AddedParameter));
            switch (args.AddedParameter.ValueType)
            {
                case XmlParameterValueType.Double:
                    SetParameterValue(args.AddedParameter.Name, 0);
                    break;
                case XmlParameterValueType.Integer:
                    SetParameterValue(args.AddedParameter.Name, 0);
                    break;
                case XmlParameterValueType.String:
                    SetParameterValue(args.AddedParameter.Name, "");
                    break;
            }
        }
        private void CheckString(string name, string inputValue)
        {
            if (GetParameter(name).GetValueType != XmlParameterValueType.String)
            {
                double value = 0;
                bool check = double.TryParse(inputValue, out value);
                if (!check) throw new ArgumentException("Не правильно введено значение параметра!\n" +
                    $"Имя параметра: {name}\n" +
                    $"Тип параметра: {GetParameter(name).GetValueType}\n" +
                    $"Значение параметра: {inputValue}");
            }
        }

        private Parameter GetParameter(string name) => parameters
            .Where(x => x.Key == name)
            .First();

        public XElement ToXmlElement() => new XElement("Entity", 
            new XAttribute("EntityName", Description.Name),
            parameters.Select(x => x.ToXmlElement()));

    }
}
