using System.Reflection;
using System.Text.Json;

namespace CSharp_Discord_Bot.models.singletons
{
    public class DataSingleton
    {
        private static DataSingleton? instance;

        private static List<Hat>? _hats;

        // Private constructor to prevent instantiation from other classes.
        private DataSingleton() { }

        // Public method to provide access to the Singleton instance.
        public static DataSingleton GetInstance()
        {
            // Check if the instance is null; if it is, create a new instance.
            if (instance == null)
            {
                instance = new DataSingleton();

                _hats = instance.LoadHats();
            }
            return instance;
        }

        private List<Hat> LoadHats()
        {
            const string resourceName = "CSharp_Discord_Bot.resources.HatData.json";

            Console.WriteLine($"Loading Hats...");

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                Console.WriteLine("Failed to load: " + resourceName);

                return new List<Hat>();
            }
            else
            {
                using var reader = new StreamReader(stream);
                string json = reader.ReadToEnd();
                var hatList = JsonSerializer.Deserialize<HatList>(json);

                Console.WriteLine($"Loaded {hatList?.hats?.Count ?? 0} hat(s).");

                if (hatList == null)
                    return new List<Hat>();

                // Note, with the current logic, if we allow negative weights,
                // you could technically force one hat to ALWAYS be rolled on a certain day.
                var validHats = from hat in hatList.hats
                                where hat.enabled && hat.weight > 0
                                select hat;
                Console.WriteLine($"Enabled Hats: {validHats.Count()}.");

                foreach (var hat in validHats)
                    Console.WriteLine($"'{hat.filename}': {hat.weight}.");

                double totalWeight = validHats.Sum((hat) => hat.weight);
                Console.WriteLine($"Total Weight: {totalWeight}.");
                return validHats.ToList();
            }
        }

        public List<Hat> hats { get { return _hats!; } }
    }
}