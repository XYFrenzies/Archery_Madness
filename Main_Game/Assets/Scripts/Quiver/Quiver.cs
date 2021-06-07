using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Quiver : XRBaseInteractable
{
    public GameObject ArrowPrefab = null;

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

    private void CreateAndSelectArrow( SelectEnterEventArgs a_Args )
    {
        Arrow arrow = CreateArrow( a_Args.interactor.transform );
        interactionManager.ForceSelect( a_Args.interactor, arrow );
    }

    private Arrow CreateArrow( Transform a_Orientation )
    {
        GameObject arrowObject = Instantiate(
            ArrowPrefab,
            a_Orientation.position,
            a_Orientation.rotation );

        return arrowObject.GetComponent< Arrow >();
    }
}
