using System.Reflection;
using System.Text.Json;

namespace CSharp_Discord_Bot.models.singletons
{
    public class ConfigSingleton
    {
        private static ConfigSingleton? instance;
        private readonly ConfigModel _config;
        private readonly SecretsModel _secrets;
        private readonly VersionModel _version;

        // Private constructor to prevent instantiation from other classes.
        private ConfigSingleton()
        {
            _config = LoadJson<ConfigModel>("config.json");
            _secrets = LoadJson<SecretsModel>("secrets.json");
            _version = LoadJson<VersionModel>("version.json");
            Console.WriteLine($"Loaded version '{_version.Version}'.");
        }

        // Public method to provide access to the Singleton instance.
        public static ConfigSingleton GetInstance()
        {
            // Check if the instance is null; if it is, create a new instance.
            instance ??= new ConfigSingleton();
            return instance;
        }

        private static TModel LoadJson<TModel>(string fileName)
        {
            string resourceName = $"CSharp_Discord_Bot.{fileName}";
            Console.WriteLine($"Loading \"{fileName}\"");

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Failed to load: \"{resourceName}\".");
            using var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            var config = JsonSerializer.Deserialize<TModel>(json)
                ?? throw new Exception($"Could not deserialize \"{resourceName}\"");

            Console.WriteLine($"Loaded \"{fileName}\".");
            return config;
        }

        public ConfigModel Config => _config;
        internal SecretsModel Secrets => _secrets;
        public VersionModel Version => _version;
    }
}