using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace APIMocker.Seeds
{
    public static class Seeds
    {
        private static string SeedFilePathTemplate { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Seeds/{0}"); }

        private static string GetFormatted(string fileName) => string.Format(SeedFilePathTemplate, fileName);

        public static ICollection<T> From<T>(string fileName)
        {
            using (var sr = new StreamReader(GetFormatted(fileName), Encoding.UTF8))
                return JsonConvert.DeserializeObject<ICollection<T>>(sr.ReadToEnd());
        }
    }
}