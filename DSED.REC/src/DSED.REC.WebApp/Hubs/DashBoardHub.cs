using Microsoft.AspNetCore.SignalR;

namespace DSED.REC.WebApp.Hubs;

public class DashBoardHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}