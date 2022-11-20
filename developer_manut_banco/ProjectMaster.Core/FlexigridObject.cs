using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ProjectMaster.Core
{
    public class FlexigridRow
    {
        public string id;
        public List<string> cell = new List<string>();
    }

    public class FlexigridObject
    {
        public int page;
        public int total;
        public List<FlexigridRow> rows = new List<FlexigridRow>();

        public static List<string> GetPropertyList(object obj)
        {
            List<string> propertyList = new List<string>();

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance |
                                                            BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                object o = property.GetValue(obj, null);
                propertyList.Add(o == null ? "" : o.ToString());
            }
            return propertyList;
        }
    }
}
