using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Suframa.Sciex.CrossCutting.Ping
{
	public static class PingCheck
	{

		public static bool CheckPort(string ip, int porta)
		{
			using (TcpClient tcpClient = new TcpClient())
			{
				try
				{
					tcpClient.Connect(ip, porta);
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}


		public static bool PingHost(string nameOrAddress)
		{
			bool pingable = false;
			System.Net.NetworkInformation.Ping pinger = null;

			try
			{
				pinger = new System.Net.NetworkInformation.Ping();
				PingReply reply = pinger.Send(nameOrAddress);
				pingable = reply.Status == IPStatus.Success;
			}
			catch (PingException)
			{
				// Discard PingExceptions and return false;
			}
			finally
			{
				if (pinger != null)
				{
					pinger.Dispose();
				}
			}

			return pingable;
		}
	}
}
