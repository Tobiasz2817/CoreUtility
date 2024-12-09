using UnityEditor;
using UnityEngine;

namespace CoreUtility
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    public class SingletonPersistent<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    [InitializeOnLoad]
    public class StaticPersistent<T> : MonoBehaviour where T : Component
    {
        protected static T Instance { get; private set; }
        
        static StaticPersistent() =>
            Instance = PersistentFactory.CreateComponent<T>();
    }

    public static class PersistentFactory {
        public static T CreateComponent<T>() where T: Component {
            var newGameObject = new GameObject(typeof(T).Name);
            var instance = newGameObject.AddComponent<T>();
            Object.DontDestroyOnLoad(newGameObject);
            
            return instance;
        }
    }
}
