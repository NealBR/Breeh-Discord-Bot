﻿// See https://aka.ms/new-console-template for more information
using CSharp_Discord_Bot.models;
using CSharp_Discord_Bot.modules;
using CSharp_Discord_Bot.resources;

using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using System.Text.Json;

public class Program
{

    public static void Main(string[] args)
        => new Program().RunBotAsync().GetAwaiter().GetResult();
    /*
    static void OnProcessExit(object sender, EventArgs e)
        => KillBotAsync().GetAwaiter().GetResult();
    */
    private DiscordSocketClient _client;
    private CommandService _commands;
    private IServiceProvider _services;

    private List<Hat> LoadHats()
    {
        const string resourceName = "CSharp_Discord_Bot.resources.HatData.json";

        Console.WriteLine($"Loading Hats...");

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
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

    private async Task RunBotAsync()
    {
        DataSingleton dataSingleton = DataSingleton.GetInstance();
        dataSingleton.hats = LoadHats();

        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);
        _commands = new CommandService();

        _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();

        _client.Log += _client_Log;

        await RegisterCommandsAsync();

        await _client.LoginAsync(TokenType.Bot, Secrets.DISCORD_BOT_TOKEN);

        await _client.StartAsync();
        /*
        var channel = await _client.GetChannelAsync(Secrets.AL_CHANNEL_ID) as IMessageChannel;

        await channel!.SendMessageAsync("I awake!");
        */
        await Task.Delay(-1);
    }

    private async Task KillBotAsync()
    {
        var channel = await _client.GetChannelAsync(Secrets.AL_CHANNEL_ID) as IMessageChannel;

        await channel!.SendMessageAsync("I sleep!");
    }

    private Task _client_Log(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    public async Task RegisterCommandsAsync()
    {
        _client.MessageReceived += HandleCommandAsync;
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        var context = new SocketCommandContext(_client, message);

        if (message.Author.IsBot) return;
        /*
        Console.WriteLine("Not a bot");
        Console.WriteLine("Message: " + message);
        Console.WriteLine("Author: " + message.Author);
        Console.WriteLine("Content: " + message.Content);
        */
        int argPos = 0;
        if (message.HasStringPrefix("!", ref argPos))
        {
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if(!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
            }
        }
    }

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;
        await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable(Secrets.DISCORD_BOT_TOKEN));
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
