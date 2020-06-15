using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace FindTrip_Web.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.show(message);
        }

        //宣告靜態類別，儲存user清單
        public static class Users
        {
            public static Dictionary<string, string> ConnectionIds = new Dictionary<string, string>();
        }

        //傳送訊息給所有User

        //傳送訊息給某人
        public void SendOne(string id, string message)
        {
            var from = Users.ConnectionIds.FirstOrDefault(u => u.Key == Context.ConnectionId);
            //var to = Users.ConnectionIds.Where(u => u.Key == id).FirstOrDefault();

            Clients.Client(id).show("<span style='color:red'>" + from.Value + "密你:" + message + "</span>");
        }

        //新使用者連線進入聊天室
        public void UserConnected(string name)
        {
            //將目前使用者新增至user清單
            Users.ConnectionIds.Add(Context.ConnectionId, name);

            //發送給所有人，取得user清單
            Clients.All.getList(Users.ConnectionIds.Select(u => new { id = u.Key, name = u.Value }).ToList());

            //通知其他人，有新使用者
            Clients.Others.show("歡迎" + name + "進入聊天室");
        }

        //當使用者斷線時呼叫
        //stopCalled是SignalR 2.1.0版新增的參數
        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.Others.removeList(Context.ConnectionId);
            Users.ConnectionIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}
