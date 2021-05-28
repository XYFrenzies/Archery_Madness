using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Range(1, 100)] public int PoolSize;
    public ConfigurableJoint SlideRailJoint;
    public GameObject TargetPrefab1;
    public GameObject TargetPrefab2;
    public GameObject TargetPrefab3;

    void Start()
    {
        m_TargetPoolWood = new ObjectPool<Target_WoodBird>(PoolSize, GenerateNewTarget<Target_WoodBird>, target => target.OnActivate(), target => target.OnDeactivate());
        m_TargetPoolFire = new ObjectPool<Target_FireBird>(PoolSize, GenerateNewTarget<Target_FireBird>, target => target.OnActivate(), target => target.OnDeactivate());
        m_TargetPoolGlass = new ObjectPool<Target_GlassBird>(PoolSize, GenerateNewTarget<Target_GlassBird>, target => target.OnActivate(), target => target.OnDeactivate());
    }

    public ObjectPool<Target_WoodBird> PoolWoodBird
    {
        get
        {
            return m_TargetPoolWood;
        }
    }

    public ObjectPool<Target_FireBird> PoolFireBird
    {
        get
        {
            return m_TargetPoolFire;
        }
    }

    public ObjectPool<Target_GlassBird> PoolGlassBird
    {
        get
        {
            return m_TargetPoolGlass;
        }
    }

    public bool SpawnNewTarget(Target.TargetType a_TargetType)
    {
        return false;
    }

    private T GenerateNewTarget<T>() where T : Target
    {
        GameObject newGameObject = new GameObject();
        T newTarget = newGameObject.AddComponent<T>();
        newTarget.AttachToSlideRail(SlideRailJoint);
        newGameObject.SetActive(false);
        return newTarget;
    }

    private ObjectPool<Target_WoodBird> m_TargetPoolWood;
    private ObjectPool<Target_FireBird>   m_TargetPoolFire;
    private ObjectPool<Target_GlassBird> m_TargetPoolGlass;
}
