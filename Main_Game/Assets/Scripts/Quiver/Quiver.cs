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

    protected override void Awake()
    {
        base.Awake();
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
                    GameObject newObject = Instantiate( ArrowPrefab, new Vector3( 0, -10, 0 ), Quaternion.identity );
                    Arrow_Broadhead arrow = newObject.GetComponent< Arrow_Broadhead >();
                    arrow.Pool = m_ArrowPool;
                    arrow.gameObject.SetActive( false );

                    return arrow;
                }
            case Arrow.ArrowType.HAMMER:
                {
                    GameObject newObject = Instantiate( ArrowPrefab, new Vector3( 0, -10, 0 ), Quaternion.identity );
                    Arrow_Hammerhead arrow = newObject.GetComponent< Arrow_Hammerhead >();
                    arrow.Pool = m_ArrowPool;
                    arrow.gameObject.SetActive( false );

                    return arrow;
                }
            case Arrow.ArrowType.WATER:
                {
                    GameObject newObject = Instantiate( ArrowPrefab, new Vector3( 0, -10, 0 ), Quaternion.identity );
                    Arrow_WaterBalloon arrow = newObject.GetComponent< Arrow_WaterBalloon >();
                    arrow.Pool = m_ArrowPool;
                    arrow.gameObject.SetActive( false );

                    return arrow;
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
        a_Arrow.gameObject.transform.rotation = Quaternion.identity;

        // Resets
        Collider collider = a_Arrow.GetComponent< Collider >();
        collider.isTrigger = true;

        Rigidbody rigidbody = a_Arrow.GetComponent< Rigidbody >();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;

        a_Arrow.movementType = MovementType.Instantaneous;
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

    //private Arrow CreateArrow( Transform a_Orientation )
    //{
    //    if ( m_ArrowPool.TrySpawn( out Arrow arrow ) )
    //    {
    //        arrow.gameObject.transform.position = a_Orientation.position;
    //        arrow.gameObject.transform.rotation = a_Orientation.rotation;

    //        return arrow;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //        //a_Orientation.position,
    //        //a_Orientation.rotation );

    //    //return arrowObject.GetComponent< Arrow >();
    //}

    private Arrow CreateArrow( Transform a_Orientation )
    {
        Arrow arrow = null;

        switch (Type)
        {
            case Arrow.ArrowType.BROAD:
                {
                    arrow = Instantiate( GalleryController.Instance.PersistantArrowBroad, a_Orientation.position, a_Orientation.rotation ).GetComponent< Arrow >();
                    break;
                }
            case Arrow.ArrowType.HAMMER:
                {
                    arrow = Instantiate( GalleryController.Instance.PersistantArrowHammer, a_Orientation.position, a_Orientation.rotation ).GetComponent< Arrow >();
                    break;
                }
            case Arrow.ArrowType.WATER:
                {
                    arrow = Instantiate( GalleryController.Instance.PersistantArrowWater, a_Orientation.position, a_Orientation.rotation ).GetComponent< Arrow >();
                    break;
                }
            default:
                break;
        }
        arrow.gameObject.SetActive( true );
        return arrow;
    }

    private ObjectPool< Arrow > m_ArrowPool;
}