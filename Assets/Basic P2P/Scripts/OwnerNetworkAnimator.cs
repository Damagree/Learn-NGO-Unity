using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

namespace CodeZash.NGO {
    public class OwnerNetworkAnimator : NetworkAnimator {

        protected override bool OnIsServerAuthoritative() {
            return false;
        }

    }
}