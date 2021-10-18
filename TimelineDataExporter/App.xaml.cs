using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

using Newtonsoft.Json;

using TimelineDataExporter.Data;
using TimelineDataExporter.Enums;
using TimelineDataExporter.Models;

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
            VerifyDataFolderIntegrity();

            // Get all files in that directory
            foreach (var txtFilePath in Directory.EnumerateFiles(directoryPath))
            {
                // Trim the files extension
                var noExtensionFileName = txtFilePath.Replace(".txt", "");
                noExtensionFileName = noExtensionFileName.Substring(noExtensionFileName.LastIndexOf("/") + 1);

                // Read them and populate our DataContainers
                using (var reader = new StreamReader(txtFilePath))
                {
                    var historicPeriodEnum = (TimelineHistoricPeriod)Enum.Parse(typeof(TimelineHistoricPeriod), noExtensionFileName);
                    var historicPeriod = JsonConvert.DeserializeObject<HistoricPeriod>(reader.ReadToEnd());

                    HistoricPeriodsModel.Instance.AddHistoricPeriod(historicPeriodEnum, historicPeriod);
                }
            }

            // Initialize the data model if it's not fine
            HistoricPeriodsModel.Instance.VerifyIntegrity();
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            VerifyDataFolderIntegrity();

            foreach (var txtFilePath in Directory.EnumerateFiles(directoryPath))
            {
                var noExtensionFileName = txtFilePath.Replace(".txt", "");
                noExtensionFileName = noExtensionFileName.Substring(noExtensionFileName.LastIndexOf("/") + 1);
                foreach (var categoryName in Enum.GetNames(typeof(TimelineHistoricPeriod)))
                {
                    if (String.Compare(noExtensionFileName, categoryName) == 0)
                    {
                        var serializedTimelineEvents = HistoricPeriodsModel.Instance.HistoricPeriods[(TimelineHistoricPeriod)Enum.Parse(typeof(TimelineHistoricPeriod), noExtensionFileName)];
                        File.WriteAllText(txtFilePath, JsonConvert.SerializeObject(serializedTimelineEvents));  
                    }
                  }
            }
        }

        private void VerifyDataFolderIntegrity()
        {
            // Verify that the directory exists, if not, create it
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

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
        private string directoryPath = "../../Resources/";
    }
}
