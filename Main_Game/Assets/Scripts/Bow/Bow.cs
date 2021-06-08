using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bow : XRGrabInteractable
{
    private Notch m_Notch = null;

    protected override void Awake()
    {
        base.Awake();
        m_Notch = GetComponentInChildren< Notch >();
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
}
