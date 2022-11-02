using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueObject", order = 1)]
public class DialogueObject : ScriptableObject
{
    public string text;
    public DialogueObject[] dialogueOptions;
}
