using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NelowGames {
	
    /// <summary>
    /// Singleton behaviour class, used for components that should only have one instance.
    /// <remarks>Singleton classes live on through scene transitions and will mark their 
    /// parent root GameObject with <see cref="Object.DontDestroyOnLoad"/></remarks>
    /// </summary>
    /// <typeparam name="T">The Singleton Type</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T instance;

        /// <summary>
        /// Returns the Singleton instance of the classes type.
        /// If no instance is found, then we search for an instance
        /// in the scene.
        /// If more than one instance is found, we throw an error and
        /// no instance is returned.
        /// </summary>
        public static T Instance {
            get {
                #if UNITY_EDITOR
                    if(!UnityEditor.EditorApplication.isPlaying) {
                        if(instance == null) Debug.LogError("should initialize Editor Instance");
                        return instance;
                    }
                #endif
                if (!IsInitialized && searchForInstance) {
                    searchForInstance = false;
                    T[] objects = FindObjectsOfType<T>();
                    if(objects.Length == 0) {
                        GameObject go = new GameObject(typeof(T).Name);
                        Debug.Log( "Creating " + go.name + "\ncalled by " + UnityEngine.StackTraceUtility.ExtractStackTrace (), go );
                        instance = go.AddComponent<T>();
                        instance.dontDestroyOnLoad = true;
                    }
                    else if (objects.Length == 1) {
                        instance = objects[0];
                        // DontDestroyOnLoad(instance.gameObject);
                    }
                    // else if (objects.Length > 1) {
                    //     Debug.LogErrorFormat("Expected exactly 1 {0} but found {1}.", typeof(T).Name, objects.Length);
                    // }
                }
                return instance;
            }
        }
        public static T ForceGetInstance() {
            if(instance) return instance;
            T[] objects = FindObjectsOfType<T>();
            if(objects.Length == 0) {
                GameObject go = new GameObject(typeof(T).Name);
                // Debug.Log( "Creating " + go.name + "\ncalled by " + UnityEngine.StackTraceUtility.ExtractStackTrace (), go );
                instance = go.AddComponent<T>();
                instance.dontDestroyOnLoad = true;
            }
            else if (objects.Length == 1) {
                instance = objects[0];
                // DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
            
        }

        private static bool searchForInstance = true;

        public static void AssertIsInitialized() {
            Debug.Assert(IsInitialized, string.Format("The {0} singleton has not been initialized.", typeof(T).Name));
        }

        /// <summary>
        /// Returns whether the instance has been initialized or not.
        /// </summary>
        public static bool IsInitialized {
            get {
                return instance != null;
            }
        }

        /// <summary>
        /// Base Awake method that sets the Singleton's unique instance.
        /// Called by Unity when initializing a MonoBehaviour.
        /// Scripts that extend Singleton should be sure to call base.Awake() to ensure the
        /// static Instance reference is properly created.
        /// </summary>
        protected virtual void Awake() {
            if (IsInitialized && instance != this) {
                Debug.LogErrorFormat("Trying to instantiate a second instance of singleton class {0}. Additional Instance was destroyed onto " + name, GetType().Name, instance.gameObject);
                #if UNITY_EDITOR
                    if(!UnityEditor.EditorApplication.isPlaying)
                        return;
                #endif
                if (Application.isEditor) {
                    DestroyImmediate(this);
                }
                else {
                    Destroy(this);
                }

            }
            else if (!IsInitialized) {
                instance = (T)this;
                searchForInstance = false;
                #if UNITY_EDITOR
                    if(!UnityEditor.EditorApplication.isPlaying)
                        return;
                #endif
                if(dontDestroyOnLoad && transform.parent == null)
                    DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Base OnDestroy method that destroys the Singleton's unique instance.
        /// Called by Unity when destroying a MonoBehaviour. Scripts that extend
        /// Singleton should be sure to call base.OnDestroy() to ensure the
        /// underlying static Instance reference is properly cleaned up.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                searchForInstance = true;
            }
        }

        public bool dontDestroyOnLoad = false;
    }
}
