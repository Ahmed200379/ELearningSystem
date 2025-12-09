using Microsoft.AspNetCore.SignalR;

namespace ELearningSystem
{
    public class ChatHub:Hub
    {
       public async Task JoinGroup(string groupid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupid);
        }
        public async Task LeaveGroup(string groupid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupid);
        }
    }
}
