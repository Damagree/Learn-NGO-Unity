using Newtonsoft.Json;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeZash.NGO {
    public class NetworkManagerGui : MonoBehaviour {

        [SerializeField] bool disableButtonOnConnected = true;
        [SerializeField] bool clearButtonBeforeAdding = true;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        [Header("Connection Manager")]
        [SerializeField] ConnectionManager connectionManager;
        [SerializeField] private TMP_InputField localIp;
        [SerializeField] private TMP_InputField port;
        [SerializeField] private TMP_InputField remoteIp;
        [SerializeField] private TMP_Dropdown serverDropdown;

        [SerializeField] UnityEvent OnConnectedEvents;

        private void Start() {
            if (clearButtonBeforeAdding) {
                hostButton.onClick.RemoveAllListeners();
                clientButton.onClick.RemoveAllListeners();
            }

            hostButton.onClick.AddListener(() => {
                if (NetworkManager.Singleton.StartHost()) {
                    UpdateInteractable();
                }
            });
            clientButton.onClick.AddListener(() => {
                connectionManager.UpdateRemoteIP(serverDropdown.options[serverDropdown.value].text.Split(':')[0]);
                connectionManager.UpdatePort(ushort.Parse(serverDropdown.options[serverDropdown.value].text.Split(':')[1]));
                if (NetworkManager.Singleton.StartClient()) {
                    UpdateInteractable();
                }
            });

            // Initialize inputfield
            localIp.onEndEdit.AddListener((text) => {
                connectionManager.UpdateLocalIP(text);
            });

            port.contentType = TMP_InputField.ContentType.IntegerNumber;
            port.SetTextWithoutNotify(connectionManager.Port.ToString());
            port.onEndEdit.AddListener((text) => {
                connectionManager.UpdatePort(ushort.Parse(text));
            });

            remoteIp.onEndEdit.AddListener((text) => {
                connectionManager.UpdateRemoteIP(text);
            });
        }

        void UpdateInteractable() {
            if (!disableButtonOnConnected) {
                return;
            }

            hostButton.interactable = false;
            clientButton.interactable = false;

            Debug.Log($"ConnectionData: " + JsonConvert.SerializeObject(GetComponent<UnityTransport>().ConnectionData, Formatting.Indented));
        }
    }
}
