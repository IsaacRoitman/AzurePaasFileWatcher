using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ApplicationInsights;
using System.IO;
using static System.Console;
using static System.ConsoleColor;
using System.Text.RegularExpressions;


//to do
//clean \ in file path
//make property naming consistenet across alerts
//copy / program all alerts
//

namespace FileSystemWatcherSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // instantiate the object
            var fileSystemWatcher = new FileSystemWatcher
            {
                // tell the watcher where to look and recurse subdirectories
                Path = @"D:\home\site\wwwroot\",
                IncludeSubdirectories = true
            };

            // Associate event handlers with the events
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            // You must add this line - this allows events to fire.
            fileSystemWatcher.EnableRaisingEvents = true;
            
            WriteLine($"Started watching folder {fileSystemWatcher.Path}");
            //WriteLine("(Press any key to exit.)");
            ReadLine();
        }

        private static void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            TelemetryClient telemetry = new TelemetryClient();
            var customDimensions = new Dictionary<string, string>();
            customDimensions.Add("NewFileName", $"{e.Name}");
            customDimensions.Add("OriginalFileName", $"{e.OldName}");
            customDimensions.Add("FullPath", $"{e.FullPath}");
            customDimensions.Add("OldFullPath", $"{e.OldFullPath}");
            telemetry.TrackEvent("File Renamed", customDimensions);

            WriteLine($"File renamed: {e.OldFullPath} changed to {e.FullPath}");
        }

        private static void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            TelemetryClient telemetry = new TelemetryClient();
            var customDimensions = new Dictionary<string, string>();
            
            //remove
            //var fwfile = Path.GetFileName(@"\filename\filefolder\file.csv");
            //WriteLine($"File is {fwfile}");
            //filename good newfilename bad

            customDimensions.Add("NewFileName", $"{e.Name}");
            customDimensions.Add("FullPath", $"{e.FullPath}");
            //
            customDimensions.Add("FileName",Path.GetFileName(e.Name));
            telemetry.TrackEvent("File Deleted", customDimensions);
 
            WriteLine($"File Deleted: {e.FullPath}");
            WriteLine($"File Deleted: {e.Name}");
        }

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //ForegroundColor = Green;
            //WriteLine($"A new file has been changed - {e.Name}");
            TelemetryClient telemetry = new TelemetryClient();
            telemetry.TrackEvent("File Changed 1 - {e.Name}");
        }

        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //ForegroundColor = Blue;
            //WriteLine($"A new file has been created - {e.Name}");
            TelemetryClient telemetry = new TelemetryClient();
            telemetry.TrackEvent("File Created 1 - {e.Name}");
        }
    }
}