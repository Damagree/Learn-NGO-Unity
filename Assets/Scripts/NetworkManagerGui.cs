using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace CodeZash.NGO {
    public class NetworkManagerGui : MonoBehaviour {

        [SerializeField] bool clearButtonBeforeAdding = true;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        private void Start() {
            if (clearButtonBeforeAdding) {
                hostButton.onClick.RemoveAllListeners();
                clientButton.onClick.RemoveAllListeners();
            }

            hostButton.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });
            clientButton.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });
        }

    }
}