using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

using Newtonsoft.Json;

using TimelineDataExporter.Data;
using TimelineDataExporter.Enums;

namespace TimelineDataExporter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        App() : base()
        {
            Startup += OnAppStartup;
            Exit += OnAppExit;
        }

        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            // Verify that the directory exists, if not, create it
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);

                // Create a file for each historic category
                foreach (var categoryName in Enum.GetNames(typeof(TimelineHistoricPeriod)))
                {
                    var pathBuilder = new StringBuilder(directoryPath + "/" + categoryName + ".txt");
                    var path = pathBuilder.ToString();
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                    }
                }
            }

            // Get all files in that directory
            var txtFilePaths = Directory.EnumerateFiles(directoryPath, ".txt");
            var dataEntryContainers = new List<DataEntryContainer>();
            foreach (var txtFilePath in txtFilePaths)
            {
                // Read them and populate our DataContainers
                using (var dataEntryContainerFile = new StreamReader(txtFilePath))
                {
                    dataEntryContainers.Add(JsonConvert.DeserializeObject<DataEntryContainer>(dataEntryContainerFile.ReadToEnd()));
                }
            }

            // 
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            
        }

        private string directoryPath = "../../Resources";
    }
}
