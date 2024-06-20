using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WoV.Cultivation.UseCases;
using WoV.Identity;
using WoV.UserActivity;
using ILogger = Serilog.ILogger;

namespace WoV.Cultivation;

//TO DO: NEED TO THINK ABOUT CACHE
public class CultivateHostedService : IHostedService,IDisposable
{
    private Timer _timer;
    private readonly UserActivityService _userActivityService;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    private readonly  TimeSpan _period = TimeSpan.FromSeconds(10);

    public CultivateHostedService(
        UserActivityService userActivityService,
        IServiceProvider services,
        ILogger logger)
    {
        _userActivityService = userActivityService;
        _services = services;
        _logger = logger;
        ;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(AddExperienceToOnlineUsers, null, TimeSpan.Zero, _period); // Change the TimeSpan here to adjust the interval
        return Task.CompletedTask;
    }
    
    private async void AddExperienceToOnlineUsers(object? state)
    {
        var start = DateTime.UtcNow;
        var activeUserIds = _userActivityService.GetAllActiveUserIds();
        using var scope = _services.CreateScope();
        
        var mediatr = scope.ServiceProvider
            .GetRequiredService<IMediator>();

        await mediatr.Send(new AddBulkCultivationExpCommand(activeUserIds));
        
        var end = DateTime.UtcNow;
        _logger.Information("{ServiceName} was executed - Total time = {TotalTime}",nameof(CultivateHostedService),(end-start).TotalSeconds);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}