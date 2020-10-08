using System;
using System.Net;
using System.Net.NetworkInformation;

namespace RMAPPV2
{
    static class ServerConnection
    {
        public static bool IsServerOnline()
        {
            Datos.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PRDB"].ConnectionString.ToString();
            Datos.HostName = Datos.ConnectionString.Between("data source=", ";initial");

            try
            {
                IPAddress[] ip = Dns.GetHostAddresses(Datos.HostName);
                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(ip[0]);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}




