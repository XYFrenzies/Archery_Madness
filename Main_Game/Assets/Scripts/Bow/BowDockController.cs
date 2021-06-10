using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowDockController : MonoBehaviour
{
    public Bow Bow;
    public Transform BowDock;

    private void Awake()
    {
        m_Quaternion = new Quaternion();

        if ( m_RedockOnAwake )
        {
            ForceRedockBow();
        }
    }

    public void ForceRedockBow()
    {
        GameStateManager.Instance.Bow.transform.position = BowDock.position;
        GameStateManager.Instance.Bow.transform.rotation = BowDock.rotation;

        if ( m_BowHover == null )
        {
            m_BowHover = StartCoroutine( BowHover() );
        }
    }

    public void StartRedockTimer()
    {
        m_BowRedockTimer = StartCoroutine( BowRedockTimer() );
    }

    public void StopRedockTimer()
    {
        if ( m_BowRedockTimer != null )
        {
            StopCoroutine( m_BowRedockTimer );
            m_BowRedockTimer = null;
        }

        if ( m_BowHover != null )
        {
            StopCoroutine( m_BowHover );
            m_BowHover = null;
        }
    }

    private IEnumerator BowRedockTimer()
    {
        yield return new WaitForSeconds( m_RedockTimer );

        Bow.SetPhysics( false );
        ForceRedockBow();
    }

    private IEnumerator BowHover()
    {
        float elapsedTime = 0.0f;

        while ( true )
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;

            float height = BowDock.position.y + 
                m_HoverAmplitude * Mathf.Sin( elapsedTime * m_HoverSpeed * Mathf.Rad2Deg );
            m_Quaternion.eulerAngles = new Vector3( 0, m_SpinSpeed * elapsedTime * Mathf.Rad2Deg, 0 );

            Bow.gameObject.transform.position = new Vector3( BowDock.position.x, height, BowDock.position.z );
            Bow.gameObject.transform.rotation = m_Quaternion;
        }
    }

    #pragma warning disable 0649

    [ SerializeField ] private bool m_RedockOnAwake;
    [ SerializeField ] [ Range( 0.0f, 10.0f ) ] private float m_RedockTimer;
    [ SerializeField ] [ Range( 0.0f, 10.0f ) ] private float m_SpinSpeed;
    [ SerializeField ] [ Range( 0.0f, 10.0f ) ] private float m_HoverSpeed;
    [ SerializeField ] [ Range( 0.0f, 10.0f ) ] private float m_HoverAmplitude;
    private Quaternion m_Quaternion;
    private Coroutine m_BowRedockTimer;
    private Coroutine m_BowHover;

    #pragma warning restore
}