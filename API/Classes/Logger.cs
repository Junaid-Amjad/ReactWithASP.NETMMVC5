using System;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace API.Classes
{
    public static class Logger
    {
        public static string NextLog = "";
        private static bool _logging;
        private static StringBuilder _logFile = new StringBuilder();
        private static string _lastlog = "";
        private static string _lastPluginLog = "";
        private static readonly StringBuilder PluginLogFile = new StringBuilder(100000);
        private static DateTime _logStartDateTime = DateTime.Now;
        private static readonly string PluginLogTemplate =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?><PluginLog username=\"" + Environment.UserName +
            "\"><!--CONTENT--></PluginLog>";
        
        internal static void LogException(Exception ex, string info)
        {
            ex.HelpLink = info + ": " + ex.Message;
            Logger.LogException(ex);
        }
        internal static void LogException(Exception ex)
        {
            try
            {
                string em = ex.HelpLink + "<br/>" + ex.Message + "<br/>" + ex.Source + "<br/>" + ex.StackTrace +
                            "<br/>" + ex.InnerException + "<br/>" + ex.Data;
                _logFile.Append("<tr><td style=\"color:red\" valign=\"top\">Exception:</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + em + "</td></tr>");
            }
            catch
            {
                // ignored
            }
        }
        internal static void LogMessage(String message, string e)
        {
            Logger.LogMessage(String.Format(message, e));
        }
        internal static void LogMessage(String message)
        {
         
            try
            {
                _logFile.Append("<tr><td style=\"color:green\" valign=\"top\">Message</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }
        internal static void LogError(String message)
        {
         
            try
            {
                _logFile.Append("<tr><td style=\"color:red\" valign=\"top\">Error</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }
        internal static void LogError(String message, string message2)
        {
            try
            {
                _logFile.Append("<tr><td style=\"color:red\" valign=\"top\">Error</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + ", " + message2 + "</td></tr>");
                Console.WriteLine(message + ", " + message2);
            }
            catch
            {
                //do nothing
            }
        }

        internal static void LogWarningToFile(String message)
        {
            try
            {
                _logFile.Append("<tr><td style=\"color:orange\" valign=\"top\">Warning</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }
        public static void WriteLogs()
        {
            
            if (DateTime.Now.DayOfYear != _logStartDateTime.DayOfYear)
            {
                //start new log
                _logging = true;
                _logStartDateTime = DateTime.Now;
                _logFile = new StringBuilder();
                InitLogging(false);
                return;
            }
            if (_logging)
            {

                try
                {
                    if (_logFile.Length > 100 * 1024)
                    {
                        _logFile.Append("<tr><td style=\"color:red\" valign=\"top\">Logging Exiting</td><td valign=\"top\">" +
                            DateTime.Now.ToLongTimeString() +
                            "</td><td valign=\"top\">Logging is being disabled as it has reached the maximum size (" +
                            100 + "kb).</td></tr>");
                        _logging = false;
                    }
                    if (_lastlog.Length != _logFile.Length)
                    {
                        string logTemplate = "<html><head><title>"+GlobalMembers.SoftwareName+@" v" + GlobalMembers.ApplicationProductVersion + " Log File</title><style type=\"text/css\">body,td,th,div {font-family:Verdana;font-size:10px}</style></head><body><h1>" + GlobalMembers.ServerName + ": Log Start (v" + GlobalMembers.ApplicationProductVersion + " ): " + _logStartDateTime + "</h1><p><table cellpadding=\"2px\"><!--CONTENT--></table></p></body></html>";
                        _lastlog = _logFile.ToString();
                        string fc = logTemplate.Replace("<!--CONTENT-->", _lastlog);
                        File.WriteAllText(GlobalMembers.LogLocation + @"log_" + NextLog + ".htm", fc);
                    }
                }
                catch (Exception)
                {
                    _logging = false;
                }
            }

        }
        public static string ZeroPad(int i)
        {
            if (i < 10)
                return "0" + i;
            return i.ToString(CultureInfo.InvariantCulture);
        }
        public static void InitLogging(bool warnonerror = true)
        {
            DateTime logdate = DateTime.Now;

            foreach (string s in Directory.GetFiles(GlobalMembers.LogLocation, "log_*", SearchOption.TopDirectoryOnly))
            {
                var fi = new FileInfo(s);
                if (fi.CreationTime < DateTime.Now.AddDays(0 - GlobalMembers.LogDays))
                    fi.Delete();
//                    FileOperations.Delete(s);
            }
            NextLog = ZeroPad(logdate.Day) + ZeroPad(logdate.Month) + logdate.Year;
            int i = 1;
            if (File.Exists(GlobalMembers.LogLocation + "log_" + NextLog + ".htm"))
            {
                while (File.Exists(GlobalMembers.LogLocation + "log_" + NextLog + "_" + i + ".htm"))
                    i++;
                NextLog += "_" + i;
            }
            try
            {
                File.WriteAllText(GlobalMembers.LogLocation + "log_" + NextLog + ".htm", DateTime.Now + Environment.NewLine);
                _logging = true;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

    }
}