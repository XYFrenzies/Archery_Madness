using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(IInteractable a_Other);
    void UnInteract(IInteractable a_Other);
}
