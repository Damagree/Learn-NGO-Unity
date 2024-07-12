using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;

namespace CodeZash.NGO {
    public class ClientDiscovery : MonoBehaviour {

        [SerializeField] private ConnectionManager connectionManager;
        [SerializeField] private TMP_Dropdown serverDropdown;

        private UdpClient udpClient;
        private Thread listenThread;

        public void RefreshList() {
            serverDropdown.ClearOptions();
            if (udpClient != null) {
                ListenForServerBroadcasts();
                return;
            }
            udpClient = new UdpClient(8888);
            listenThread = new Thread(new ThreadStart(ListenForServerBroadcasts));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void OnDestroy() {
            listenThread?.Abort();
            udpClient?.Close();
        }

        private void ListenForServerBroadcasts() {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8888);
            byte[] data = udpClient.Receive(ref endPoint);
            string message = Encoding.UTF8.GetString(data);
            UpdateServerList(message);
        }

        private void UpdateServerList(string serverInfo) {
            // Update the server dropdown in the main thread
            UnityMainThreadDispatcher.Instance.Enqueue(() => {
                serverDropdown.options.Add(new TMP_Dropdown.OptionData(serverInfo));
                serverDropdown.RefreshShownValue();
            });
        }
    }
}
