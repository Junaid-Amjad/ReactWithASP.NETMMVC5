using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace API.Classes
{
    public class objects{
        public int SourceIndex { get; set; }
        public string VideoSourceString { get; set; }
    }
    public class GlobalMessage
    {
        public string Message { get; set; }
        public int StatusID { get; set; }
        public string StatusDescription { get; set; }
    }
    /*
    StatusID:
    1: Success
    2: Error
    3: Exception
    */

    public class CameraIPResult
    {
        public bool IPfound { get; set; }
        //Only added for StreamController.cs. If StreamController.cs is removed, this can be removed as well.
        public string FileServerAddress { get; set; }
        public string Message { get; set; }
        public int ProcessID { get; set; }
    }
    public class cameraAttribute
    {
        public string GUID { get; set; }
        public string URL { get; set; }
        public int noofColumns{get;set;}
        public string LayoutName{get;set;}
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
    public class FilePath
    {
        public bool isDirectory { get; set; }
        public string path { get; set; }
        public bool isSamePath { get; set; }
    }
    public class StreamingClass
    {
        public bool isDirectory { get; set; }
        public string path { get; set; }
        public bool isSamePath { get; set; }
    }
    public class SearchFilesToXML
    {
        public string CameraName { get; set; }
        public string CameraDirectoryName { get; set; }
        public string FileName { get; set; }
        public int TriggerType { get; set; }
        public long CreatedTickDate { get; set; }
        public string FolderPath { get; set; }
        public string FullFolderandFileName { get; set; }
        public int length { get; set; }
    }
    public class MainClasses
    {

    }
    public class GlobalMembers
    {
        public static string SoftwareName = "StrandUSA";
        public static string ApplicationProductVersion = "1.0.0.1";
        public static string ServerName = "StrandUSA Server";

        public static string LogLocation = "E:\\Strantin-Work\\Strand-Git-Testing-2\\Strand\\StrandUSAIIS\\Log\\";

        public static int LogDays = 3;
    }
    public class FFMPEGClass
    {
        public string GUID { get; set; }
        public Task ffmpegsource { get; set; }
        public CancellationTokenSource cancelToken { get; set; }

    }
    public static class InsertFFMPEG
    {
        public static List<FFMPEGClass> FFMPEGobj = new List<FFMPEGClass>();

        public static void ADDFFMPEG(string GUID, Task ffmpegsource, CancellationTokenSource cancel)
        {
            FFMPEGobj.Add(new FFMPEGClass() { GUID = GUID, ffmpegsource = ffmpegsource, cancelToken = cancel });
        }
    }
    public class GlobalFunction
    {
        public static string ConvertASCIIIntoString(String Value)
        {
            string urlresult = "";
            try
            {
                string[] v = Value.Split('-');
                foreach (string str in v)
                {
                    if (str.Trim() != "")
                        urlresult += char.ConvertFromUtf32(Convert.ToInt16(str.Trim()));
                }
                return urlresult;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }

}