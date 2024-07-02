using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace CodeZash.NGO {
    public class NetworkManagerGui : MonoBehaviour {

        [SerializeField] bool disableButtonOnConnected = true;
        [SerializeField] bool clearButtonBeforeAdding = true;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

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
                if (NetworkManager.Singleton.StartClient()) {
                    UpdateInteractable();
                }
            });
        }

        void UpdateInteractable() {
            if (!disableButtonOnConnected) {
                return;
            }

            hostButton.interactable = false;
            clientButton.interactable = false;
        }

    }
}