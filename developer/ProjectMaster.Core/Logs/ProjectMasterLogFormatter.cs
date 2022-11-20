using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace ProjectMaster.Core
{
    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class ProjectMasterLogFormatter : LogFormatter
    {
        public ProjectMasterLogFormatter(NameValueCollection nvc)
        {
        }

        public override string Format(LogEntry log)
        {
            ProjectMasterLogEntry exceptionLogEntry = (ProjectMasterLogEntry)log;
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString());
            sb.Append(" ");
            sb.Append(exceptionLogEntry.ClassFile);
            sb.Append(" ");
            sb.Append(exceptionLogEntry.Method);
            sb.Append(" ");
            sb.Append(exceptionLogEntry.Message);

            return sb.ToString();
        }
    }
}
