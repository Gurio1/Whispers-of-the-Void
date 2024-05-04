using Microsoft.AspNetCore.SignalR;
using WoV.UserActivity;
using ILogger = Serilog.ILogger;

namespace WoV.Cultivation;

public class CultivateHostedService : IHostedService,IDisposable
{
    private Timer _timer;
    private readonly IHubContext<CultivationHub> _hubContext;
    private readonly UserActivityService _userActivityService;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public CultivateHostedService(IHubContext<CultivationHub> hubContext,
        UserActivityService userActivityService,
        IServiceProvider services,
        ILogger logger)
    {
        _hubContext = hubContext;
        _userActivityService = userActivityService;
        _services = services;
        _logger = logger;
        ;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)); // Change the TimeSpan here to adjust the interval
        return Task.CompletedTask;
    }
    
    private async void DoWork(object? state)
    {
        var activeUserIds = _userActivityService.GetAllActiveUserIds();
        using var scope = _services.CreateScope();
        
        var characterRepository = 
            scope.ServiceProvider
                .GetRequiredService<ICharacterRepository>();

        var userChars = await characterRepository.GetByUserIdsAsync(activeUserIds);

        foreach (var character in userChars)
        {
            var cultivatedExp =character.Cultivate();
            await _hubContext.Clients.User(character.UserId).SendAsync("ReceiveNotification", "Total exp = "+ cultivatedExp);
        }

        _logger.Information("{ServiceName} was executed",nameof(CultivateHostedService));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}