using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowDockController : MonoBehaviour
{
    public Bow Bow;
    public Transform BowDock;

    public void ForceRedockBow()
    {
        GameStateManager.Instance.Bow.transform.position = BowDock.position;
        GameStateManager.Instance.Bow.transform.rotation = BowDock.rotation;
    }

    public void StartRedock()
    {
        m_BowRedockTimer = StartCoroutine( BowRedockTimer() );
    }

    public void StopRedock()
    {
        if ( m_BowRedockTimer != null )
        {
            StopCoroutine( m_BowRedockTimer );
            m_BowRedockTimer = null;
        }
    }

    private IEnumerator BowRedockTimer()
    {
        yield return new WaitForSeconds( 2.0f );

        Bow.SetPhysics( false );
        ForceRedockBow();
    }

    private Coroutine m_BowRedockTimer;
}
