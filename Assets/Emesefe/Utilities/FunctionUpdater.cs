using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emesefe.Utilities
{
    public class FunctionUpdater
    {
        // Class to hook Actions into MonoBehaviour
        private class MonoBehaviourHook : MonoBehaviour {

            public Action OnUpdate;

            private void Update() {
                if (OnUpdate != null) OnUpdate();
            }
        }
        
        private static List<FunctionUpdater> updaterList; // Holds a reference to all active updaters
        private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change
        
        private readonly GameObject _gameObject;
        private readonly Func<bool> _updateFunc; // Destroy Updater if return true
        private readonly string _functionName;
        private readonly bool _active;
        
        private FunctionUpdater(GameObject gameObject, Func<bool> updateFunc, string functionName, bool active) {
            _gameObject = gameObject;
            _updateFunc = updateFunc;
            _functionName = functionName;
            _active = active;
        }
        
        private void Update() {
            if (!_active) return;
            if (_updateFunc()) {
                DestroySelf();
            }
        }
        
        private static void InitializeIfNeeded()
        {
            if (initGameObject != null) return;
            
            initGameObject = new GameObject("FunctionUpdater Global");
            updaterList = new List<FunctionUpdater>(); // TODO: If there is a Game Object, updaterList is initialized?
        }

        #region Create
        
        public static FunctionUpdater Create(Action updateFunc) {
            return Create(() => { updateFunc(); return false; }, "", true, false);
        }

        public static FunctionUpdater Create(Action updateFunc, string functionName) {
            return Create(() => { updateFunc(); return false; }, functionName, true, false);
        }

        public static FunctionUpdater Create(Func<bool> updateFunc) {
            return Create(updateFunc, "", true, false);
        }

        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName) {
            return Create(updateFunc, functionName, true, false);
        }

        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active) {
            return Create(updateFunc, functionName, active, false);
        }

        private static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active, bool stopAllWithSameName) {
            InitializeIfNeeded();

            if (stopAllWithSameName) {
                StopAllUpdatersWithName(functionName);
            }

            GameObject gameObject = new GameObject("FunctionUpdater Object " + functionName, typeof(MonoBehaviourHook));
            FunctionUpdater functionUpdater = new FunctionUpdater(gameObject, updateFunc, functionName, active);
            gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionUpdater.Update;

            updaterList.Add(functionUpdater);
            return functionUpdater;
        }
        
        #endregion
        
        public static void StopUpdaterWithName(string functionName) {
            InitializeIfNeeded();
            
            for (int i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i]._functionName != functionName) continue;
                
                updaterList[i].DestroySelf();
                return;
            }
        }

        private static void StopAllUpdatersWithName(string functionName) {
            InitializeIfNeeded();
            
            for (int i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i]._functionName != functionName) continue;
                
                updaterList[i].DestroySelf();
                i--;
            }
        }
        
        private static void RemoveUpdater(FunctionUpdater funcUpdater) {
            InitializeIfNeeded();
            updaterList.Remove(funcUpdater);
        }
        
        private void DestroySelf() {
            RemoveUpdater(this);
            if (_gameObject != null) {
                UnityEngine.Object.Destroy(_gameObject);
            }
        }
    }
}

