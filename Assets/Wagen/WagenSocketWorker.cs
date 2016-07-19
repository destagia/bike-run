using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using UnityEngine;

namespace Wagen
{
	public class WagenSocketWorker : IDisposable
	{
		TcpClient tcpClient;
		NetworkStream stream;

		public WagenSocketWorker()
		{
			tcpClient = new TcpClient("localhost", 43376);
			stream = tcpClient.GetStream();
		}

		~WagenSocketWorker()
		{
			Dispose();
		}

		public void Dispose()
		{
			stream.Dispose();
		}

		void SpawnThread(Action action)
		{
			var thread = new Thread(new ThreadStart(action));
			thread.Start();
		}

		public void SendMessage(string message, Action<string> callback)
		{
			var enc = Encoding.UTF8;
			byte[] messageBytes = enc.GetBytes(message + "\n");
			Debug.Log(messageBytes.Length);
			stream.Write(messageBytes, 0, messageBytes.Length);
			SpawnThread(() => {
				var bytes = new Byte[1];
				stream.Read(bytes, 0, bytes.Length);
				callback(enc.GetString(bytes).Trim());
			});
		}
	}
}

