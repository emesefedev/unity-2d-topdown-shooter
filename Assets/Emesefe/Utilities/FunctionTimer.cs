using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emesefe.Utilities
{
    public class FunctionTimer
    {
        private class MonoBehaviourHook : MonoBehaviour {

            public Action OnUpdate;

            private void Update() {
                if (OnUpdate != null) OnUpdate();
            }

        }
        
        private static List<FunctionTimer> timerList; // Holds a reference to all active timers
        private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change
        
        private bool _active;
        
        private GameObject _gameObject;
        private Action _action;
        private float _timer;
        private string _functionName;
        private bool _useUnscaledDeltaTime;
        
        public FunctionTimer(GameObject gameObject, Action action, float timer, string functionName, bool useUnscaledDeltaTime) {
            _gameObject = gameObject;
            _action = action;
            _timer = timer;
            _functionName = functionName;
            _useUnscaledDeltaTime = useUnscaledDeltaTime;
        }
        
        private void Update() {
            if (_useUnscaledDeltaTime) {
                _timer -= Time.unscaledDeltaTime;
            } else {
                _timer -= Time.deltaTime;
            }
            if (_timer <= 0) {
                // Timer complete, trigger Action
                _action();
                DestroySelf();
            }
        }

        private static void InitializeIfNeeded() {
            if (initGameObject == null) {
                initGameObject = new GameObject("FunctionTimer_Global");
                timerList = new List<FunctionTimer>();
            }
        }

        #region Create
        
        public static FunctionTimer Create(Action action, float timer) {
            return Create(action, timer, "", false, false);
        }

        public static FunctionTimer Create(Action action, float timer, string functionName) {
            return Create(action, timer, functionName, false, false);
        }

        public static FunctionTimer Create(Action action, float timer, string functionName, bool useUnscaledDeltaTime) {
            return Create(action, timer, functionName, useUnscaledDeltaTime, false);
        }

        public static FunctionTimer Create(Action action, float timer, string functionName, bool useUnscaledDeltaTime, bool stopAllWithSameName) {
            InitializeIfNeeded();

            if (stopAllWithSameName) {
                StopAllTimersWithName(functionName);
            }

            GameObject obj = new GameObject("FunctionTimer Object "+functionName, typeof(MonoBehaviourHook));
            FunctionTimer funcTimer = new FunctionTimer(obj, action, timer, functionName, useUnscaledDeltaTime);
            obj.GetComponent<MonoBehaviourHook>().OnUpdate = funcTimer.Update;

            timerList.Add(funcTimer);

            return funcTimer;
        }
        
        #endregion
        
        public static void StopAllTimersWithName(string functionName) {
            InitializeIfNeeded();
            for (int i = 0; i < timerList.Count; i++) {
                if (timerList[i]._functionName == functionName) {
                    timerList[i].DestroySelf();
                    i--;
                }
            }
        }
        
        public static void RemoveTimer(FunctionTimer funcTimer) {
            InitializeIfNeeded();
            timerList.Remove(funcTimer);
        }

        private void DestroySelf() {
            RemoveTimer(this);
            if (_gameObject != null) {
                UnityEngine.Object.Destroy(_gameObject);
            }
        }
    }
}