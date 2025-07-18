using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emesefe.Utilities
{
    public class FunctionPeriodic
    {
        private class MonoBehaviourHook : MonoBehaviour {

            public Action OnUpdate;

            private void Update() {
                if (OnUpdate != null) OnUpdate();
            }
        }
        
        private static List<FunctionPeriodic> funcList; // Holds a reference to all active timers
        private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change
        
        private GameObject _gameObject;
        public Action _action;
        private float _timer;
        public Func<bool> _testDestroy;
        private string _functionName;
        private bool _useUnscaledDeltaTime;
        private float _baseTimer;
        
        private FunctionPeriodic(GameObject gameObject, Action action, float timer, Func<bool> testDestroy, string functionName, bool useUnscaledDeltaTime) {
            _gameObject = gameObject;
            _action = action;
            _timer = timer;
            _testDestroy = testDestroy;
            _functionName = functionName;
            _useUnscaledDeltaTime = useUnscaledDeltaTime;
            _baseTimer = timer;
        }
        
        private void Update() {
            if (_useUnscaledDeltaTime) {
                _timer -= Time.unscaledDeltaTime;
            } else {
                _timer -= Time.deltaTime;
            }
            if (_timer <= 0) {
                _action();
                if (_testDestroy != null && _testDestroy()) {
                    //Destroy
                    DestroySelf();
                } else {
                    //Repeat
                    _timer += _baseTimer;
                }
            }
        }
        
        private static void InitializeIfNeeded() {
            if (initGameObject == null) {
                initGameObject = new GameObject("FunctionPeriodic_Global");
                funcList = new List<FunctionPeriodic>();
            }
        }
        
        #region Create
        
        // Trigger [action] every [timer], execute [testDestroy] after triggering action, destroy if returns true
        public static FunctionPeriodic Create(Action action, Func<bool> testDestroy, float timer) {
            return Create(action, testDestroy, timer, "", false);
        }

        public static FunctionPeriodic Create(Action action, float timer) {
            return Create(action, null, timer, "", false, false, false);
        }

        public static FunctionPeriodic Create(Action action, float timer, string functionName) {
            return Create(action, null, timer, functionName, false, false, false);
        }

        public static FunctionPeriodic Create(Action callback, Func<bool> testDestroy, float timer, string functionName, bool stopAllWithSameName) {
            return Create(callback, testDestroy, timer, functionName, false, false, stopAllWithSameName);
        }

        public static FunctionPeriodic Create(Action action, Func<bool> testDestroy, float timer, string functionName, bool useUnscaledDeltaTime, bool triggerImmediately, bool stopAllWithSameName) {
            InitializeIfNeeded();

            if (stopAllWithSameName) {
                StopAllFunc(functionName);
            }

            GameObject gameObject = new GameObject("FunctionPeriodic Object " + functionName, typeof(MonoBehaviourHook));
            FunctionPeriodic functionPeriodic = new FunctionPeriodic(gameObject, action, timer, testDestroy, functionName, useUnscaledDeltaTime);
            gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionPeriodic.Update;

            funcList.Add(functionPeriodic);

            if (triggerImmediately) action();

            return functionPeriodic;
        }
        
        #endregion
        
        public static void StopAllFunc(string functionName) {
            InitializeIfNeeded();
            for (int i = 0; i < funcList.Count; i++) {
                if (funcList[i]._functionName == functionName) {
                    funcList[i].DestroySelf();
                    i--;
                }
            }
        }
        
        public static void RemoveTimer(FunctionPeriodic funcTimer) {
            InitializeIfNeeded();
            funcList.Remove(funcTimer);
        }
        
        public void DestroySelf() {
            RemoveTimer(this);
            if (_gameObject != null) {
                UnityEngine.Object.Destroy(_gameObject);
            }
        }
    }
}