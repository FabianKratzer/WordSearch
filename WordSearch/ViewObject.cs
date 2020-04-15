using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace WordSearch
{

    public class ViewObject
    {

        public List<string> _properties { get; set; }
        public Dictionary<string, string> _propertyValues { get; set; }

        public ViewObject()
        {
            _properties = new List<string>();
            _propertyValues = new Dictionary<string, string>();
        }

        public List<string> Properties()
        {
            return _properties;
        }
        public void AddProperties(List<PropertyInfo> _newProperties)
        {
            foreach (var item in _newProperties)
            {
                _properties.Add(item.Name);
                _propertyValues.Add(item.Name, "");

            }
        }

        public void UpdateProperty(string propertyName, string propertyValue)
        {
            if (_propertyValues.ContainsKey(propertyName))
            {
                _propertyValues[propertyName] = propertyValue;
            }
        }
        public string GetPropertyValue(string propertyName)
        {
            if (_propertyValues.ContainsKey(propertyName))
            {
                return _propertyValues[propertyName];
            }
            else return null;
        }

    }

}
