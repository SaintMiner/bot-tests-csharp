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

    public async Task Ticker()
    {
        while (true)
        {
            TickGuild();
            await Task.Delay(DotNetEnv.Env.GetInt("TICK_INTERVAL_SECONDS") * 1000);
        }
    }

    public void TickGuild()
    {
        Console.WriteLine("TICK");
    }
    
    public async Task MainAsync()
    {
        DotNetEnv.Env.Load();
        _client = new DiscordSocketClient();
        _config = JsonConvert.DeserializeObject(File.ReadAllText("config.json")) ?? null;
        _commandService = new CommandService();
        Ticker();
        
        if (_config is null)
        {
            Console.WriteLine("Config not provided :(");
            return;
        }

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
        var guilds = _client.Guilds.ToList();
        // guilds.ForEach(async delegate(SocketGuild guild)
        // {
        //     // Console.WriteLine(guild.Name);
        //     // Console.WriteLine(guild.GetUsersAsync());
        //     // var users = guild.Users.ToList();
        //     // users.ForEach((SocketGuildUser user) =>
        //     // {
        //     //     Console.WriteLine(user.DisplayName);
        //     // });
        //     var users = await guild.GetUsersAsync().FlattenAsync();
        //     foreach (var guildUser in users)
        //     {
        //         Console.WriteLine(guildUser.DisplayName);
        //     }
        // });
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