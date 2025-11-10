using Newtonsoft.Json;

namespace Sandbox.BatchOperations
{
    internal class Program
    {
        string _sourcePath = @"C:\Sources\techsson-betssonoriginals-plinko\src\techsson-betssonoriginals-plinko\Techsson.BetssonOriginals.Plinko\BackendGameConfig.json";
        string _targetPath = @"C:\Temp\BackendGameConfig.json";

        static void Main(string[] args)
        {
            var deserialized = JsonConvert.DeserializeObject<Root>(System.IO.File.ReadAllText(@"C:\Sources\techsson-betssonoriginals-plinko\src\techsson-betssonoriginals-plinko\Techsson.BetssonOriginals.Plinko\BackendGameConfig.json"));

            foreach (var outcome in deserialized.Outcomes)
            {
                var minWeight = outcome.Value.Min(b => b.Weight);
                foreach (var bucket in outcome.Value)
                {
                    bucket.Weight /= minWeight;
                }
            }

            System.IO.File.WriteAllText(@"C:\Temp\BackendGameConfig.json", JsonConvert.SerializeObject(deserialized, Formatting.Indented));

            Console.WriteLine("Hello, World!");
        }
    }

    public class Root
    {
        [JsonProperty("Outcomes")]
        public Dictionary<string, List<Bucket>> Outcomes { get; set; }
    }


    public class Bucket
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("multiplier")]
        public double Multiplier { get; set; }
    }
}


