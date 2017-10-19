using System.Collections.Generic;
using System.Linq;
using Model.DBTool;
using Business.Business;
using System.Text;
using System.Net.Sockets;
using SocketIOClient;
using Newtonsoft.Json.Linq;
using SocketIOClient.Messages;
using System;

namespace Web.Common
{
    public class HnHelper
    {
        Entities context = new Entities();
        //
        // GET: /HnHelper/


        private string genPreChar(int solan)
        {
            var str = new StringBuilder();
            str = str.Append("|");
            if (solan > 0)
            {
                for (int i = 0; i < solan; i++)
                {
                    str = str.Append("--");
                }
                return str.ToString();
            }
            return string.Empty;
        }
        

    }
}
