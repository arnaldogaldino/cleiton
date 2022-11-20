using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace ProjectMaster.Core
{
    /// <summary>
    /// http://logging.apache.org/log4net/download_log4net.cgi
    /// </summary>
    public class LogManager
    {
        private const string DEBUG_CATEGORY = "PMLog_Debug";
        private const string INFO_CATEGORY = "PMLog_Info";
        private const string ERROR_CATEGORY = "PMLog_Error";
        private const string TRACE_CATEGORY = "PMLog_Trace";

        #region Methods

        #region Debug Info
        public static void Info(string stringMessage)
        {
            ProjectMasterLogEntry logEntry = new ProjectMasterLogEntry();

            logEntry.Categories.Add(INFO_CATEGORY);
            logEntry.Severity = System.Diagnostics.TraceEventType.Information;
            if (Logger.ShouldLog(logEntry))
            {

                logEntry.Message = stringMessage;

                Logger.Write(logEntry);
            }
        }
        #endregion

        #region Debug Log
        public static void Debug(string stringMessage)
        {
            ProjectMasterLogEntry logEntry = new ProjectMasterLogEntry();

            logEntry.Categories.Add(DEBUG_CATEGORY);
            logEntry.Severity = System.Diagnostics.TraceEventType.Verbose;

            if (Logger.ShouldLog(logEntry))
            {
                logEntry.Message = stringMessage;
                Logger.Write(logEntry);
            }
        }
        #endregion

        #region Debug Parameter
        /// <summary>
        /// Logs the parameter values
        /// </summary>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="parameterValue">Parameter Value</param>
        public static void DebugParameter(string parameterName, bool parameterValue)
        {
            LogManager.Debug(parameterName + " = " + parameterValue.ToString());
        }

        /// <summary>
        /// Logs the parameter values
        /// </summary>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="parameterValue">Parameter Value</param>
        public static void DebugParameter(string parameterName, DateTime parameterValue)
        {
            LogManager.Debug(parameterName + " = " + parameterValue.ToString());
        }

        /// <summary>
        /// Logs the parameter values
        /// </summary>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="parameterValue">Parameter Value</param>
        public static void DebugParameter(string parameterName, int parameterValue)
        {
            LogManager.Debug(parameterName + " = " + parameterValue.ToString());
        }

        /// <summary>
        /// Logs the parameter values
        /// </summary>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="parameterValue">Parameter Value</param>
        public static void DebugParameter(string parameterName, double parameterValue)
        {
            LogManager.Debug(parameterName + " = " + parameterValue.ToString());
        }

        /// <summary>
        /// Logs the parameter values
        /// </summary>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="parameterValue">Parameter Value</param>
        public static void DebugParameter(string parameterName, string parameterValue)
        {
            if (parameterValue == null)
            {
                LogManager.Debug(parameterName + ":null");
            }
            else
            {
                LogManager.Debug(parameterName + " = " + parameterValue);
            }
        }

        /// <summary>
        /// Logs the parameter values
        /// </summary>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="parameterValue">Parameter Value</param>
        public static void DebugParameter(string parameterName, object parameterValue)
        {
            if (parameterValue == null)
            {
                LogManager.Debug(parameterName + ":null");
            }
            else
            {
                LogManager.Debug(parameterName + " = " + parameterValue.ToString());
            }
        }
        #endregion

        #region Error
        public static void Error(Exception ex)
        {
            Error(string.Empty, ex);
        }

        public static void Error(string stringMessage, Exception ex)
        {
            ProjectMasterLogEntry logEntry = new ProjectMasterLogEntry();

            logEntry.Categories.Add(ERROR_CATEGORY);
            logEntry.Severity = System.Diagnostics.TraceEventType.Error;

            if (Logger.ShouldLog(logEntry))
            {

                if (stringMessage != string.Empty)
                {
                    logEntry.Message = stringMessage + " - ";
                }
                if (ex != null)
                {
                    logEntry.Message += ex.ToString();
                }
                Logger.Write(logEntry);
            }
        }
        #endregion

        #region Error
        //public static void Error(Exception ex)
        //{
        //    Error(string.Empty, ex);
        //}

        public static void Trace(string stringMessage)
        {
            ProjectMasterLogEntry logEntry = new ProjectMasterLogEntry();

            logEntry.Categories.Add(TRACE_CATEGORY);
            logEntry.Severity = System.Diagnostics.TraceEventType.Information;

            if (Logger.ShouldLog(logEntry))
            {

                if (stringMessage != string.Empty)
                {
                    logEntry.Message = stringMessage + " - ";
                }

                Logger.Write(logEntry);
            }
        }
        #endregion

        #endregion

        public static void Error(string p)
        {
            Error( p, null );
        }
    }
}

