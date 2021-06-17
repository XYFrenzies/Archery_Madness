using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Quiver : XRBaseInteractable
{
    public GameObject ArrowPrefab = null;
    public Arrow.ArrowType Type;

    public ObjectPool< Arrow > ArrowPool
    {
        get
        {
            return m_ArrowPool;
        }
    }

    private void Awake()
    {
        m_ArrowPool = new ObjectPool< Arrow >( CreateArrow, OnArrowActive, OnArrowInactive );
        m_ArrowPool.Populate( 3 );
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener( CreateAndSelectArrow );
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener( CreateAndSelectArrow );
    }

    private Arrow CreateArrow()
    {
        switch (Type)
        {
            case Arrow.ArrowType.BROAD:
                {
                    return Instantiate( ArrowPrefab, new Vector3( 0, -10, 0 ), Quaternion.identity ).GetComponent< Arrow_Broadhead >();
                }
            case Arrow.ArrowType.HAMMER:
                {
                    return Instantiate( ArrowPrefab, new Vector3( 0, -10, 0 ), Quaternion.identity ).GetComponent< Arrow_Hammerhead >();
                }
            case Arrow.ArrowType.WATER:
                {
                    return Instantiate( ArrowPrefab, new Vector3( 0, -10, 0 ), Quaternion.identity ).GetComponent< Arrow_WaterBalloon >();
                }
            default:
                {
                    return null;
                }
        }
    }

    private void OnArrowActive( Arrow a_Arrow )
    {
        a_Arrow.gameObject.SetActive( true );
    }

    private void OnArrowInactive( Arrow a_Arrow )
    {
        a_Arrow.gameObject.transform.position = new Vector3( 0, -10, 0 );
        a_Arrow.gameObject.SetActive( false );
    }

    private void CreateAndSelectArrow( SelectEnterEventArgs a_Args )
    {
        Arrow arrow = CreateArrow( a_Args.interactor.transform );
        interactionManager.ForceSelect( a_Args.interactor, arrow );
    }

    //private Arrow CreateArrow( Transform a_Orientation )
    //{
    //    GameObject arrowObject = Instantiate(
    //        ArrowPrefab,
    //        a_Orientation.position,
    //        a_Orientation.rotation );

    //    return arrowObject.GetComponent< Arrow >();
    //}

    private Arrow CreateArrow( Transform a_Orientation )
    {
        if ( m_ArrowPool.TrySpawn( out Arrow arrow ) )
        {
            arrow.gameObject.transform.position = a_Orientation.position;
            arrow.gameObject.transform.rotation = a_Orientation.rotation;

            return arrow;
        }
        else
        {
            return null;
        }
            //a_Orientation.position,
            //a_Orientation.rotation );

        //return arrowObject.GetComponent< Arrow >();
    }

    private ObjectPool< Arrow > m_ArrowPool;
}