using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bow : XRGrabInteractable
{
    public Collider m_PhysicsCollider = null;
    private Notch m_Notch = null;
    private Rigidbody m_ThisRigidbody = null;

    protected override void Awake()
    {
        base.Awake();
        m_Notch = GetComponentInChildren< Notch >();
        m_ThisRigidbody = GetComponent< Rigidbody >();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener( m_Notch.SetReady );
        selectExited.AddListener( m_Notch.SetReady );
        selectEntered.AddListener( GameStateManager.Instance.OnBowPickup );
        selectExited.AddListener( GameStateManager.Instance.OnBowDrop );
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener( m_Notch.SetReady );
        selectExited.RemoveListener( m_Notch.SetReady );
        selectEntered.RemoveListener( GameStateManager.Instance.OnBowPickup );
        selectExited.RemoveListener( GameStateManager.Instance.OnBowDrop );
    }

    public void SetPhysics( bool a_Value )
    {
        m_ThisRigidbody.useGravity = a_Value;
        m_ThisRigidbody.isKinematic = !a_Value;
        
        if ( m_PhysicsCollider != null )
        {
            m_PhysicsCollider.enabled = a_Value;
        }
    }
}
