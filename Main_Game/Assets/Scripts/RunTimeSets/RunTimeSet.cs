using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class RunTimeSet<T> : ScriptableObject
{
    [HideInInspector] [SerializeField] private List<T> items = new List<T>();
    [SerializeField] private List<T> _items = new List<T>();
    private void OnEnable()
    {
        Initialize();
        for (int i = 0; i < _items.Count; i++)
            items.Add(_items[i]);
    }
    public void Initialize() => items.Clear();
    public int ListCount()
    {
        return items.Count;
    }
    public T GetItemIndex(int index)
    {
        return items[index];
    }
    public void AddToList(T thingToAdd)
    {
        if (!items.Contains(thingToAdd))
            items.Add(thingToAdd);
    }
    public void RemoveFromList(T thingToRemove)
    {
        if (items.Contains(thingToRemove))
            items.Remove(thingToRemove);
    }
    public T GetRandomItem()
    {
        return items[Random.Range(0,items.Count)];
    }
    public int GetIndexOfItem(T thing)
    {
        if(items.Contains(thing))
        {
            return items.IndexOf(thing);
        }
        return -1;
    }
    //[CreateAssetMenu(fileName = "New ... RunTimeSet", menuName = "Scriptable Assets/RunTimeSet/ ... ")]
}
