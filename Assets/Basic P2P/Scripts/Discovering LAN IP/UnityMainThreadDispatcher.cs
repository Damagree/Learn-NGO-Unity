using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeZash.NGO {
    public class UnityMainThreadDispatcher : MonoBehaviour {
        private static readonly Queue<Action> executionQueue = new Queue<Action>();

        public void Update() {
            lock (executionQueue) {
                while (executionQueue.Count > 0) {
                    executionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(Action action) {
            lock (executionQueue) {
                executionQueue.Enqueue(action);
            }
        }

        public static UnityMainThreadDispatcher Instance;

        private void Awake() {
            if (Instance && Instance != this) {
                Destroy(this);
            } else {
                Instance = this;
            }
        }
    }
}