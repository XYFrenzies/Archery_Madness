using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTargetSpawnPoint : MonoBehaviour
{
    [System.Serializable]
    public class TargetInScene 
    {
        public GameObject obj = null;
        public float timeBetweenEachSpawn = 1;
        
    }
    private List<TargetInScene> target;
    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
        {

            foreach (var item in target)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
