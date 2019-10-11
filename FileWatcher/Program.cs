// FileWatcher application in C# to monitor Azure App Service PaaS storage
// and log file system changes as custom metrics to Azure Application Insights.
//
// Copyright © 2019 Isaac H. Roitman, Microsoft Corporation.

using Microsoft.ApplicationInsights;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace FileSystemWatcherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instantiate the FileSystemWatcher object.
            var fileSystemWatcher = new FileSystemWatcher
            {
                // Path for the FielSystemWatcher to look and recurse subdirectories.
                Path = @"D:\home\site\wwwroot\",
                IncludeSubdirectories = true
            };

            // Associates event handlers with the events.
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            // Raises events.
            fileSystemWatcher.EnableRaisingEvents = true;

            // Console output and ReadLine pause for continuous run.
            WriteLine($"Started watching folder {fileSystemWatcher.Path}");
            ReadLine();
        }

        private static void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            TelemetryClient telemetry = new TelemetryClient();
            var customDimensions = new Dictionary<string, string>();

            // Custom dimentions created and added to customDimensions dictionary.
            customDimensions.Add("Name", Path.GetFileName(e.Name));
            customDimensions.Add("OldName", $"{e.OldName}");
            customDimensions.Add("FullPath", $"{e.FullPath}");
            customDimensions.Add("OldFullPath", $"{e.OldFullPath}");

            // Send event name and customDimensions to Application Insights.
            telemetry.TrackEvent("File Renamed", customDimensions);

            // Console output.
            WriteLine($"File Renamed: {e.OldFullPath} changed to {e.FullPath}");
            WriteLine($"File Renamed: {e.OldName} changed to {Path.GetFileName(e.Name)}");
        }

        private static void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            TelemetryClient telemetry = new TelemetryClient();
            var customDimensions = new Dictionary<string, string>();

            // Custom dimentions created and added to customDimensions dictionary.
            customDimensions.Add("Name", Path.GetFileName(e.Name));
            customDimensions.Add("FullPath", $"{e.FullPath}");
            customDimensions.Add("ChangeType", $"{e.ChangeType}");

            // Send custom event associated customDimensions to Application Insights.
            telemetry.TrackEvent("File Deleted", customDimensions);

            // Console output.
            WriteLine($"File Deleted: {e.FullPath}");
            WriteLine($"File Deleted: {Path.GetFileName(e.Name)}");
        }

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            TelemetryClient telemetry = new TelemetryClient();
            var customDimensions = new Dictionary<string, string>();

            // Custom dimentions created and added to customDimensions dictionary.
            customDimensions.Add("Name", Path.GetFileName(e.Name));
            customDimensions.Add("FullPath", $"{e.FullPath}");
            customDimensions.Add("ChangeType", $"{e.ChangeType}");

            // Send event name and customDimensions to Application Insights.
            telemetry.TrackEvent("File Changed", customDimensions);

            // Console output.
            WriteLine($"File Changed: {e.FullPath}");
            WriteLine($"File Changed: {Path.GetFileName(e.Name)}");
        }

        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            TelemetryClient telemetry = new TelemetryClient();
            var customDimensions = new Dictionary<string, string>();

            // Custom dimentions created and added to customDimensions dictionary.
            customDimensions.Add("Name", Path.GetFileName(e.Name));
            customDimensions.Add("FullPath", $"{e.FullPath}");
            customDimensions.Add("ChangeType", $"{e.ChangeType}");

            // Send event name and customDimensions to Application Insights.
            telemetry.TrackEvent("File Created", customDimensions);

            // Console output.
            WriteLine($"File Created: {e.FullPath}");
            WriteLine($"File Created: {Path.GetFileName(e.Name)}");
        }
    }
}