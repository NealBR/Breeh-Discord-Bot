using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharp_Discord_Bot.models.singletons
{
    public class DataSingleton
    {
        private static DataSingleton? instance;

        private List<Hat> _sourceHats = new();
        private List<Hat> _filteredHats = new();

        // Private constructor to prevent instantiation from other classes.
        private DataSingleton() { }

        // Public method to provide access to the Singleton instance.
        public static DataSingleton GetInstance()
        {
            // Check if the instance is null; if it is, create a new instance.
            if (instance == null)
            {
                instance = new DataSingleton();
                instance.LoadHats();
                instance.CheckHats();
            }
            return instance;
        }

        private void CheckHats()
        {
            Console.WriteLine($"Checking Hats...");
            string workingDirectory = Environment.CurrentDirectory;
            string resourceDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName + "\\resources\\";
            if (_sourceHats != null)
            {
                foreach (var hat in _sourceHats)
                {
                    if (string.IsNullOrEmpty(hat.filename))
                        continue;
                    if (!File.Exists(resourceDirectory + hat.filename))
                        Console.Error.WriteLine($"File '{hat.filename}' with caption '{hat.caption}' does not exist.");
                }
            }
            Console.WriteLine("Checking done.");
        }

        private void LoadHats()
        {
            const string resourceName = "CSharp_Discord_Bot.resources.HatData.json";

            Console.WriteLine($"Loading Hats...");

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                Console.WriteLine("Failed to load: " + resourceName);
                return;
            }

            using var reader = new StreamReader(stream);

            string json = reader.ReadToEnd();
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            var hatList = JsonSerializer.Deserialize<HatList>(json, options);

            Console.WriteLine($"Loaded {hatList?.hats?.Count ?? 0} hat(s).");

            if (hatList?.hats == null) return;

            _sourceHats = hatList.hats;

            // Note, with the current logic, if we allow negative weights in this query,
            // you could technically force one hat to ALWAYS be rolled on a certain day.
            var validHats = from hat in hatList.hats
                            where hat.enabled && hat.weight > 0
                            select hat;
            Console.WriteLine($"Enabled Hats: {validHats.Count()}.");

            foreach (var hat in validHats)
                Console.WriteLine($"'{hat.filename}': {hat.weight}.");

            double totalWeight = validHats.Sum((hat) => hat.weight);
            Console.WriteLine($"Total Weight: {totalWeight}.");
            _filteredHats = validHats.ToList();
        }

        public List<Hat> hats => _filteredHats;
    }
}