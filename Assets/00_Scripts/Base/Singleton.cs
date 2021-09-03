using System.Linq;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    protected bool flag;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Singleton<T>[] objs = FindObjectsOfType<Singleton<T>>();

                GameObject obj = objs.Where(item => item.flag == true).FirstOrDefault()?.gameObject; //GameObject.Find(typeof(T).Name);
                if (obj == null)
                {
                    if (objs.Length > 0)
                        return objs[0].GetComponent<T>();
                    obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                else
                {
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    //protected virtual void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}
}

public class SingletonBasic<T> where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }
}