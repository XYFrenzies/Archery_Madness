using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ObjectPool< T > where T : IResettable
{
    struct PooledObject
    {
        public T m_Object;
        public bool m_IsActive;
    }

    public int PoolSize
    {
        get
        {
            return m_Pool == null ? 0 : m_Pool.Length;
        }
    }
    public int CountActive
    {
        get
        {
            return m_CountActive;
        }
    }
    public int CountInactive
    {
        get
        {
            return m_Pool.Length - m_CountActive;
        }
    }
    public int LastActiveIndex
    {
        get
        {
            return 0;
        }
    }

    public ObjectPool(Func<T> a_OnPopulate, Action<T> a_OnActive, Action<T> a_OnInactive)
    {
        m_LastActiveIndex = -1;
        m_LastInactiveIndex = -1;
        m_OnPopulate = a_OnPopulate;
        m_OnActive = a_OnActive;
        m_OnInactive = a_OnInactive;
    }

    public void Populate(int a_Count)
    {
        a_Count = Math.Max(1, a_Count);
        m_CountActive = 0;
        m_Pool = new PooledObject[a_Count];
        for (int i = 0; i < m_Pool.Length; ++i)
        {
            PooledObject pooledObject = new PooledObject();
            pooledObject.m_Object = m_OnPopulate();
            pooledObject.m_IsActive = false;
            m_Pool[i] = pooledObject;
        }

        m_IsPopulated = true;
    }

    public void ResetAll()
    {
        if (!m_IsPopulated)
        {
            return;
        }

        for (int i = 0; i < m_Pool.Length; ++i)
        {
            m_Pool[i].m_Object.OnReset();
            m_Pool[i].m_IsActive = false;
        }

        m_CountActive = 0;
    }

    public void Reset(int a_Index)
    {
        if (!m_IsPopulated || a_Index < 0 || a_Index >= m_Pool.Length)
        {
            return;
        }

        m_Pool[a_Index].m_Object.OnReset();
        m_Pool[a_Index].m_IsActive = false;
        --m_CountActive;
    }

    public bool TrySpawn(out T o_SpawnedTarget)
    {
        if (!m_IsPopulated)
        {
            o_SpawnedTarget = default;
            return false;
        }

        int spawnedIndex = -1;
        PooledObject spawnedObject = m_Pool.FirstOrDefault((PooledObject pooledObject) =>
        {
            ++spawnedIndex;

            if (!pooledObject.m_IsActive)
            {
                pooledObject.m_Object.OnReset();
                return true;
            }

            return false;
        });

        o_SpawnedTarget = spawnedObject.m_Object;

        if ( spawnedObject.m_Object != null)
        {
            m_LastActiveIndex = spawnedIndex;
            m_Pool[m_LastInactiveIndex].m_IsActive = true;
            m_OnActive(spawnedObject.m_Object);
            ++m_CountActive;
            return true;
        }

        return false;
    }

    public bool Despawn(int a_Index)
    {
        if (!m_IsPopulated || a_Index < 0 || a_Index >= m_Pool.Length || !m_Pool[a_Index].m_IsActive)
        {
            return false;
        }

        m_Pool[a_Index].m_IsActive = false;
        m_OnInactive(m_Pool[a_Index].m_Object);
        m_LastInactiveIndex = a_Index;
        --m_CountActive;
        return true;
    }

    public T GetLastActivated()
    {
        return m_Pool[m_LastActiveIndex].m_Object;
    }

    public T GetLastDeactivated()
    {
        return m_Pool[m_LastInactiveIndex].m_Object;
    }

    public T GetAt( int a_Index )
    {
        if (a_Index < 0 || a_Index >= m_Pool.Length)
        {
            return default;
        }

        return m_Pool[a_Index].m_Object;
    }

    private PooledObject[] m_Pool;
    private bool m_IsPopulated;
    public int m_CountActive;
    private int m_LastActiveIndex;
    private int m_LastInactiveIndex;
    private Func<T> m_OnPopulate;
    private Action<T> m_OnActive;
    private Action<T> m_OnInactive;
}
