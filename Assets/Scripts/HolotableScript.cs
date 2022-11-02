using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolotableScript : Interactable
{
    [SerializeField] DialogueObject mission0;
    [SerializeField] DialogueObject mission1;
    [SerializeField] Interactable door;
    [SerializeField] Interactable deskPlant;
    public override void Interact()
    {
        if (PlayerPrefs.GetInt("missions") == 0)
        {
            UISystem.uiSystem.StartDialogue(mission0);
            door.interactable = true;
        }
        else if (PlayerPrefs.GetInt("missions") == 1)
        {
            UISystem.uiSystem.StartDialogue(mission1);
            deskPlant.interactable = true;
        }
    }

    public override string Prompt()
    {
        return prompt;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("missions"))
        {
            PlayerPrefs.SetInt("missions", 0);
        }
        
        prompt = "Answer";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
