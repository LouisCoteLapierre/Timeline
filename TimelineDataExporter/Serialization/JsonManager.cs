using System.IO;

using Newtonsoft.Json;

namespace TimelineDataExporter.Serialization
{
    public class JsonManager
    {
        public void ConvertToJson<T>(T objectToConvert) where T : IJsonSerializable
        {
            using (StreamWriter dataFile = new StreamWriter(dataFilePath))
            {
                dataFile.WriteLine(JsonConvert.SerializeObject(objectToConvert));
            }
        }

        public T ConvertFromJson<T>() where T : IJsonSerializable
        {
            T result;
            using (StreamReader dataFile = new StreamReader(dataFilePath))
            {
                result = JsonConvert.DeserializeObject<T>(dataFile.ReadToEnd());
            }

            return default(T);
        }

        private string dataFilePath = "../Data.txt";
    }
}
