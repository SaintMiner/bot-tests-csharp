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

    /* Starting ticker every configured seconds */
    public async Task Ticker()
    {
        while (true)
        {
            Console.WriteLine("TICK");
            await Task.Delay(DotNetEnv.Env.GetInt("TICK_INTERVAL_SECONDS") * 1000);
        }
    }

    /* Getting all users */
    public async Task GetUsersList()
    {
        var guilds = _client.Guilds.ToList();
        Console.WriteLine("==== USER LIST ====");
        // Instead foreach use select to await all loops
        var tasks = guilds.Select(async delegate(SocketGuild guild)
        {
            Console.WriteLine($"= Guild: {guild.Name} =");
            var users = await guild.GetUsersAsync().FlattenAsync();
            foreach (var guildUser in users)
            {
                Console.WriteLine(guildUser.DisplayName);
            }
        });
        await Task.WhenAll(tasks);
        Console.WriteLine("===================");
    }
    
    /* Starting bot */
    public async Task MainAsync()
    {
        DotNetEnv.Env.Load(); // Loading env - configuration
        _client = new DiscordSocketClient();
        _commandService = new CommandService();

        _commandHandler = new CommandHandler(_client, _commandService);
        _commandHandler.InstallCommandsAsync();


        _client.Log += Log;
        _client.Connected += Connected;
        _client.Ready += Ready;

        await _client.LoginAsync(TokenType.Bot, System.Environment.GetEnvironmentVariable("TOKEN"));
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Ready()
    {
        Ticker();
        GetUsersList();
        return Task.CompletedTask;
    }

    private Task Connected()
    {
        _connectedAt = DateTime.Now;
        Console.WriteLine($"Connected as {_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator} at {_connectedAt}");
        Console.WriteLine($"Found: {_client.Guilds.Count} guilds");
        return Task.CompletedTask;
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}