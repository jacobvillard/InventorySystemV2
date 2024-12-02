using UnityEngine;

namespace Generics {
    /// <summary>
    /// Singleton class that makes sure only one instance of a MonoBehaviour exists in the scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) _instance = FindObjectOfType<T>();
                return _instance;
            }
        }
    }
}