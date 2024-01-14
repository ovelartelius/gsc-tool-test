using Newtonsoft.Json;
using System;

namespace GscToolTest
{
    public static class FileExtensions
    {
        public static void CreateJsonFile(string name, string content)
        {
            //var path = $"{DataFolder}\\{NextSaturday.ToString("yyyy")}v{Week}_{name}.json";
            var path = $"{name}.json";

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            CreateFile(path, content);
        }

        public static void CreateFile(string path, string content)
        {
            try
            {
                System.IO.File.WriteAllText(path, content);
            }
            catch (DirectoryNotFoundException dirEx)
            {
                //PrintVerboseConsole($"Could not create file {path}");
            }
        }

        public static T LoadFromFile<T>(string path)
        {
            try
            {
                string jsonData = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
