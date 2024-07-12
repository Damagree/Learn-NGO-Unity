using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Events;

namespace CodeZash.NGO {
    public class ConnectionManager : MonoBehaviour {

        [Header("Setup")]
        [Tooltip("Filled automatically")]
        [SerializeField] private string localIp;

        [Tooltip("Port to join as host / client")]
        [SerializeField] ushort port = 21548;

        [Tooltip("Filled when input in runtime")]
        [SerializeField] private string remoteIp;

        [Header("Unity Transport")]
        [SerializeField] private UnityTransport unityTransport;

        [Header("Events"), Space()]
        [SerializeField] private UnityEvent<string> onLocalIPReceived;
        [SerializeField] private UnityEvent<int> onPortUpdated;

        public string LocalIp { get => localIp; private set => localIp = value; }
        public string RemoteIp { get => remoteIp; set => remoteIp = value; }
        public ushort Port { get => port; set => port = value; }

        private UdpClient udpClient;
        private Thread broadcastThread;

        private void Start() {
            LocalIp = GetLocalIPAddress();
            onLocalIPReceived.Invoke(LocalIp);

            unityTransport.ConnectionData.Address = LocalIp;
            unityTransport.ConnectionData.Port = Port;

            StartBroadcasting();
        }

        private void OnDestroy() {
            StopBroadcasting();
        }

        string GetLocalIPAddress() {
            string localIP = "";
            foreach (var address in Dns.GetHostAddresses(Dns.GetHostName())) {
                if (address.AddressFamily == AddressFamily.InterNetwork) {
                    localIP = address.ToString();
                    break;
                }
            }
            return localIP;
        }

        public void UpdateRemoteIP(string remoteIP) {
            RemoteIp = remoteIP;
            unityTransport.ConnectionData.ServerListenAddress = remoteIP;
        }

        public void UpdateLocalIP(string localIP) {
            LocalIp = localIP;
            unityTransport.ConnectionData.Address = LocalIp;
        }

        public void UpdatePort(ushort port) {
            this.Port = port;
            unityTransport.ConnectionData.Port = Port;
        }

        private void StartBroadcasting() {
            udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;

            broadcastThread = new Thread(new ThreadStart(BroadcastServerInfo));
            broadcastThread.IsBackground = true;
            broadcastThread.Start();
        }

        private void StopBroadcasting() {
            broadcastThread.Abort();
            udpClient.Close();
        }

        private void BroadcastServerInfo() {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 8888);
            while (true) {
                string message = $"{LocalIp}:{Port}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.Send(data, data.Length, endPoint);
                Thread.Sleep(1000); // Broadcast every second
            }
        }
    }
}
