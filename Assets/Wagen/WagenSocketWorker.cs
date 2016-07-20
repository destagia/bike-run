using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;

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
			var shortMessage = string.Join("", message.Take(30).Select(_ => _.ToString()).ToArray());
			Debug.Log("[WagenSocketWorker] Send message : " + shortMessage);
			var enc = Encoding.UTF8;
			byte[] messageBytes = enc.GetBytes(message + "\n");
			stream.Write(messageBytes, 0, messageBytes.Length);
			SpawnThread(() => {
				var bytes = new Byte[1];
				var start = DateTime.Now;
				var stringBuilder = new StringBuilder();
				var messageComing = false;
				while ((DateTime.Now - start).Seconds < 1) {
					stream.Read(bytes, 0, bytes.Length);
					var response = enc.GetString(bytes).Trim();

					if (!string.IsNullOrEmpty(response)) {
						messageComing = true;
						Debug.Log("[Wagen] Receive response! : " + response);
					} else if (messageComing) {
						break;
					}

					stringBuilder.Append(response);
				}
				callback(stringBuilder.ToString().Trim());
			});
		}
	}
}

