using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace ProjectMaster.Core
{
    public class FlexigridObject
    {
        const int REGISTROS_POR_PAGINA = 20;
        public int page;
        public int total;
        public List<FlexigridRow> rows ;
        public int registros { get; set; }
        public FlexigridObject()
        {
            this.page = 1;
            this.total = 0;
            this.rows = new List<FlexigridRow>();
        }
   
        public object GerarDadosGrid()
        {
            var retorno = new
            {
                page = this.page,
                total = this.total,
                records = this.registros,
                rows = this.rows,
            };

            return retorno;
        }
        
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

    public class FlexigridRow
    {
        public long id;
        public object cell = new object();
    }
}
