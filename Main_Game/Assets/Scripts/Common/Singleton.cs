using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton< T > : MonoBehaviour where T : Singleton< T >
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                T[] foundObjects = FindObjectsOfType<T>();
                
                if (foundObjects == null || foundObjects.Length == 0)
                {
                    GameObject newGameObject = new GameObject();
                    m_Instance = newGameObject.AddComponent<T>();
                    DontDestroyOnLoad(newGameObject);
                    return m_Instance;
                }
                else
                {
                    m_Instance = foundObjects[0];
                    return m_Instance;
                }
            }

            return m_Instance;
        }
    }

   // public static
}
