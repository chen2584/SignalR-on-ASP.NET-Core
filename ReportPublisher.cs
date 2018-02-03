using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace client
{
    public class ReportsPublisher : Hub
    {
        //private static ConcurrentDictionary<string, string> connectingUser = new ConcurrentDictionary<string, string>();
        private static List<string> connectingUser = new List<string>();
        public override Task OnConnectedAsync()
        {
            connectingUser.Add(Context.ConnectionId);
            Console.WriteLine(Context.ConnectionId + " is connected");
            //AddGroup("Chen");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            string ConnectionId = Context.ConnectionId; 
            connectingUser.Remove(ConnectionId);
            Console.WriteLine(Context.ConnectionId + " is disconnected >> " + ConnectionId + " Remain: " + connectingUser.Count);
            return base.OnDisconnectedAsync(ex);
        }
        public Task PublishReport(string reportName)
        {
            Console.WriteLine(Context.ConnectionId + " Send Message: " + reportName);
            //return Clients.Group("Chen").InvokeAsync("OnReportPublished", reportName);
            return Clients.All.InvokeAsync("OnReportPublished", reportName);
        }

        public Task AddGroup(string groupName)
        {
            return Groups.AddAsync(Context.ConnectionId, groupName);
        }

    }
}