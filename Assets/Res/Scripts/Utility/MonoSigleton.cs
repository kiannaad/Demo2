using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSigleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool _isDontDestroyOnLoad = false;
    private static object _lock = new object();
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject();
                            _instance = obj.AddComponent<T>();
                        }
                    }
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        // else
        // {
        //     Destroy(gameObject);
        // }

        if (_isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
