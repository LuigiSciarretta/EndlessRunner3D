using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Cerco se esiste un'altro ogetto con lo stesso componente nella scena.
                var objs = FindObjectOfType(typeof(T)) as T[];
                if (objs != null && objs.Length > 0) //Esiste uno
                    _instance = objs[0];
                if (objs != null && objs.Length > 1) //Esiste più di uno
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }
                if (_instance == null) //Non esiste
                {
                    //Faccio la instance
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
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
            Destroy(gameObject);
    }
}
