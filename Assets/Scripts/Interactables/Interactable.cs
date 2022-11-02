using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected string prompt = "Interact";

    public bool interactable = true;
    public abstract string Prompt();
    public abstract void Interact();
}
