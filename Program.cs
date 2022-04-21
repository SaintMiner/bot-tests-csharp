using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using SaintSeedCs;

public class Program
{
    private DiscordSocketClient _client;
    private dynamic _config;
    private DateTime _connectedAt;
    private CommandService _commandService;
    private CommandHandler _commandHandler;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();
        _config = JsonConvert.DeserializeObject(File.ReadAllText("config.json"));
        _commandService = new CommandService();

        if (_config is null)
        {
            Console.WriteLine("Config not provided :(");
            return;
        }

        _commandHandler = new CommandHandler(_client, _commandService);
        _commandHandler.InstallCommandsAsync();


        _client.Log += Log;
        _client.Connected += Connected;

        await _client.LoginAsync(TokenType.Bot, (string)_config.token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Connected()
    {
        _connectedAt = DateTime.Now;
        Console.WriteLine($"Connected as {_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator} at {_connectedAt}");
        return Task.CompletedTask;
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}