using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool< T > where T : IResettable
{
    public int PoolSize
    {
        get
        {
            return m_Pool == null ? 0 : m_Pool.Length;
        }
    }
    public int LastActiveIndex
    {
        get
        {
            return 0;
        }
    }
    public ObjectPool(int a_PoolSize, Func<T> a_OnPopulate, Action<T> a_OnActive, Action<T> a_OnInactive  )
    {
        m_Pool = new T[a_PoolSize];
        m_LastActiveIndex = -1;
        m_LastInactiveIndex = -1;
        m_OnPopulate = a_OnPopulate;
        m_OnActive = a_OnActive;
        m_OnInactive = a_OnInactive;
    }

    public void Populate()
    {
        for (int i = 0; i < m_Pool.Length; ++i)
        {
            m_Pool[i] = m_OnPopulate();
        }
        m_IsPopulated = true;
    }

    public void ResetAll()
    {
        if (m_Pool == null)
        {
            return;
        }

        foreach (T pooledObject in m_Pool)
        {
            pooledObject.OnReset();
        }
    }

    public void Reset(int a_Index)
    {
        if (m_Pool == null || a_Index < 0 || a_Index >= m_Pool.Length)
        {
            return;
        }
        m_Pool[a_Index].OnReset();
    }

    public bool TrySpawn(out T o_SpawnedTarget)
    {
        if (!m_IsPopulated)
        {
            o_SpawnedTarget = default;
            return false;
        }

        o_SpawnedTarget = m_Pool[(m_LastActiveIndex + 1) % m_Pool.Length];
        m_OnActive(o_SpawnedTarget);
        return true;
    }

    public T GetLastActivated()
    {
        return default;
    }

    public T GetLastDeactivated()
    {
        return default;
    }

    public T GetAt( int a_Index )
    {
        return default;
    }

    private T[] m_Pool;
    private bool m_IsPopulated;
    private int m_LastActiveIndex;
    private int m_LastInactiveIndex;
    private Func<T> m_OnPopulate;
    private Action<T> m_OnActive;
    private Action<T> m_OnInactive;
}
