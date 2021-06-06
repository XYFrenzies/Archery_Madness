using UnityEngine.XR.Interaction.Toolkit;

public class CustomInteractionManager : XRInteractionManager
{
    public void ForceDeselect( XRBaseInteractor a_Interactor )
    {
        if ( a_Interactor.selectTarget )
        {
            SelectExit( a_Interactor, a_Interactor.selectTarget );
        }
    }
}
