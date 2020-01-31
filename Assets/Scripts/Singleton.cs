using UnityEngine;


// Use this class to make any script a singleton
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    GameObject container = new GameObject();
                    DontDestroyOnLoad(container);
                    container.name = typeof(T) + "Container";
                    instance = (T)container.AddComponent(typeof(T));
                }
            }
            return instance;
        }
    }

    public static bool InstanceExists
    {
        get
        {
            if (applicationIsQuitting)
            {
                return false;
            }
            return instance != null || FindObjectOfType<T>() != null;
        }
    }

    public void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}