using Discord.Commands;

public class CommandsSingleton
{
    private static CommandsSingleton? instance;

    private static CommandService? _commands;

    // Private constructor to prevent instantiation from other classes.
    private CommandsSingleton() { }

    // Public method to provide access to the Singleton instance.
    public static CommandsSingleton GetInstance()
    {
        // Check if the instance is null; if it is, create a new instance.
        if (instance == null)
        {
            instance = new CommandsSingleton();

            _commands = new CommandService();
        }
        return instance;
    }

    public CommandService commands { get { return _commands!; } }
}
