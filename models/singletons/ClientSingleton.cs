using Discord;
using Discord.WebSocket;

public class ClientSingleton
{
    private static ClientSingleton? instance;

    private static DiscordSocketClient? _client;

    // Private constructor to prevent instantiation from other classes.
    private ClientSingleton() { }

    // Public method to provide access to the Singleton instance.
    public static ClientSingleton GetInstance()
    {
        // Check if the instance is null; if it is, create a new instance.
        if (instance == null)
        {
            instance = new ClientSingleton();

            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            _client = new DiscordSocketClient(config);
        }
        return instance;
    }

    public DiscordSocketClient client {  get { return _client!; } }
}
