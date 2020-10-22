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
            var txtFilePaths = Directory.EnumerateFiles(directoryPath, ".txt");
            foreach (var txtFilePath in txtFilePaths)
            {
                // Trim the files extension
                var noExtensionFileName = txtFilePath.Replace(".txt", "");
                // Read them and populate our DataContainers
                using (var dataEntryContainerFile = new StreamReader(txtFilePath))
                {
                    DataModel.Instance.HistoricPeriods.Add((TimelineHistoricPeriod)Enum.Parse(typeof(TimelineHistoricPeriod), noExtensionFileName),
                                                          JsonConvert.DeserializeObject<DataEntryContainer>(dataEntryContainerFile.ReadToEnd()));
                }
            }
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            VerifyDataFolderIntegrity();

            var txtFilePaths = Directory.EnumerateFiles(directoryPath, ".txt");
            foreach (var txtFilePath in txtFilePaths)
            {
                var noExtensionFileName = txtFilePath.Replace(".txt", "");
                foreach (var categoryName in Enum.GetNames(typeof(TimelineHistoricPeriod)))
                {
                    if (String.Compare(noExtensionFileName, categoryName) == 0)
                    {
                        //JsonConvert.SerializeObject(DataModel.Instance.HistoricPeriods)
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
        }
        private string directoryPath = "../../Resources";
    }
}
