using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CodeZash.NGO {
    public class NetworkPlayerSpawnedEvents : NetworkBehaviour {

        [SerializeField] private Transform targetFollowCamera;

        public override void OnNetworkSpawn() {
            if (!IsOwner) {
                return;
            }

            var theCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
            if (theCamera == null) return;

            theCamera.Follow = targetFollowCamera;
        }

    }
}