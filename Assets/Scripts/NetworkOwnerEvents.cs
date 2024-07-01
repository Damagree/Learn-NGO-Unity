using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace CodeZash.NGO {
    public class NetworkOwnerEvents : MonoBehaviour {

        [SerializeField] private NetworkObject networkObject;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent onOwner;
        [SerializeField] private UnityEvent onNotOwner;

        private void Start() {
            if (networkObject == null) {
                Debug.LogError($"Please assign the NetworkObject!");
                return;
            }

            if (networkObject.IsOwner) {
                onOwner.Invoke();
            } else {
                onNotOwner.Invoke();
            }
        }

    }
}