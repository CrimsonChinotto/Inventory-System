using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private bool autoUnparentOnAwake = false;
    [SerializeField] private bool dontDestroyOnLoad = true;

    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                //if (instance == null)
                //{
                //    var go = new GameObject(typeof(T).Name + " Auto-Generated");
                //    instance = go.AddComponent<T>();
                //}
            }

            return instance;
        }
    }

    /// <summary>
    /// Make sure to call base.Awake() in override if you need awake.
    /// </summary>
    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying) return;

        if (autoUnparentOnAwake)
        {
            transform.SetParent(null);
        }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this as T;

            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}