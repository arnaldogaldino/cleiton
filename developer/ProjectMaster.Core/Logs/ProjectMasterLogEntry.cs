using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace ProjectMaster.Core
{
    class ProjectMasterLogEntry : LogEntry
    {
        private string classFile;
        private string method;

        public ProjectMasterLogEntry()
            : base()
        {
        }

        public string ClassFile
        {
            get { return classFile; }
            set { classFile = value; }
        }

        public string Method
        {
            get { return method; }
            set { method = value; }
        }
    }
}
