using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace client
{
    public class ReportsPublisher : Hub
    {
        //private static ConcurrentDictionary<string, string> connectingUser = new ConcurrentDictionary<string, string>();
        private static List<string> connectingUser = new List<string>();
        private static List<HubCallerContext> hubct = new List<HubCallerContext>();

        public override Task OnConnectedAsync()
        {
            //HttpContext httpContext = Context.Connection.GetHttpContext();
            //Context.Connection.Abort();
            hubct.Add(Context);

            connectingUser.Add(Context.ConnectionId);
            Console.WriteLine(Context.ConnectionId + " is connected");
            //AddGroup("Chen");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            string ConnectionId = Context.ConnectionId; 
            connectingUser.Remove(ConnectionId);
            var contextIndex = hubct.RemoveAll(x => x.ConnectionId == Context.ConnectionId);            

            Console.WriteLine(Context.ConnectionId + " is disconnected >> " + ConnectionId + 
                " Remain: " + connectingUser.Count + " HubRemain: " + hubct.Count);
            return base.OnDisconnectedAsync(ex);
        }
        public Task PublishReport(string reportName)
        {
            Console.WriteLine(Context.ConnectionId + " Send Message: " + reportName);
            //return Clients.Group("Chen").InvokeAsync("OnReportPublished", reportName);
            return Clients.All.InvokeAsync("OnReportPublished", reportName);
        }

        public Task ForceDisconnectUser(string connectionId)
        {
            //ควรใช้ concurrentdictionary แทน list เพือ่ความปลอดภัย
            var contextInfo = hubct.FirstOrDefault(x => x.ConnectionId == connectionId);
            contextInfo.Connection.Abort();
            return Clients.Client(Context.ConnectionId).InvokeAsync("OnReportPublished", connectionId + " ลบแล้ว");
        }

        public Task AddGroup(string groupName)
        {
            return Groups.AddAsync(Context.ConnectionId, groupName);
        }

    }
}