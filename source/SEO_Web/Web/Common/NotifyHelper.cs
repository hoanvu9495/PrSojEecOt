using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Web.Common
{
    
    public class NotifyHelper
    {
        private string SOCKET_SERVER = WebConfigurationManager.AppSettings["SOCKET_SERVER"];
        public void sendNotification(string message, long user_id)
        {
            Client socket = new Client(SOCKET_SERVER);
            socket.Opened += SocketOpened;
            socket.Message += SocketMessage;
            socket.SocketConnectionClosed += SocketConnectionClosed;
            socket.Error += SocketError;
            // register for 'connect' event with io server
            socket.On("connect", (fn) =>
            {
                socket.Emit("notification", new { message = message, user_id = user_id });
            });

            socket.Connect();
        }
        void SocketMessage(object sender, MessageEventArgs e)
        {            
        }

        void SocketOpened(object sender, EventArgs e)
        {

        }
        void SocketError(object sender, SocketIOClient.ErrorEventArgs e)
        {
            Console.WriteLine("socket client error:");
            Console.WriteLine(e.Message);
        }

        void SocketConnectionClosed(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocketConnection was terminated!");
        }
    }
}