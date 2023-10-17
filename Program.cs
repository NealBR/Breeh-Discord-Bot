// See https://aka.ms/new-console-template for more information

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using CSharp_Discord_Bot.resources;

public class Program
{
    private ClientSingleton? clientSingleton;
    private CommandsSingleton? commandsSingleton;

    private IServiceProvider? _services;

    public static void Main(string[] args)
        => new Program().RunBotAsync().GetAwaiter().GetResult();

    private async Task RunBotAsync()
    {
        DataSingleton dataSingleton = DataSingleton.GetInstance();
        clientSingleton = ClientSingleton.GetInstance();
        commandsSingleton = CommandsSingleton.GetInstance();

        _services = new ServiceCollection().AddSingleton(clientSingleton.client).AddSingleton(commandsSingleton.commands).BuildServiceProvider();

        clientSingleton.client.Log += ClientLog;
        clientSingleton.client.MessageReceived += HandleCommandAsync;

        await commandsSingleton.commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        await clientSingleton.client.LoginAsync(TokenType.Bot, Secrets.DISCORD_BOT_TOKEN);

        await clientSingleton.client.StartAsync();
        /*
        var channel = await _client.GetChannelAsync(Secrets.AL_CHANNEL_ID) as IMessageChannel;

        await channel!.SendMessageAsync("I awake!");
        */
        await Task.Delay(-1);
    }

    private Task ClientLog(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        if(arg == null) return;

        var message = arg as SocketUserMessage;

        var context = new SocketCommandContext(clientSingleton!.client, message);

        if (message!.Author.IsBot) return;
        /*
        Console.WriteLine("Not a bot");
        Console.WriteLine("Message: " + message);
        Console.WriteLine("Author: " + message.Author);
        Console.WriteLine("Content: " + message.Content);
        */
        int argPos = 0;
        if (message.HasStringPrefix("!", ref argPos))
        {
            var result = await commandsSingleton!.commands.ExecuteAsync(context, argPos, _services);
            if(!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
