using System.IO;
using Newtonsoft.Json;

namespace Velir.Environment.Foundation.Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            UnzipConfig c = null;

            if (args != null && args.Length > 0 && args[0] == "runexample")
            {
                string json = File.ReadAllText(@".\ExampleConfig\example.json");
                c = JsonConvert.DeserializeObject<UnzipConfig>(json);
            }

            Runner.Run(c);
        }
    }
}
