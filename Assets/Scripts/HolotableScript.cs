using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HolotableScript : Interactable
{
    [SerializeField] DialogueObject mission1;
    [SerializeField] DialogueObject mission1end;
    [SerializeField] DialogueObject mission2;
    [SerializeField] DialogueObject mission3;
    [SerializeField] DialogueObject mission4;
    [SerializeField] Interactable door;
    [SerializeField] Interactable deskPlant;
    public override void Interact()
    {
        switch(PlayerPrefs.GetInt("mission"))
        {
            case 1:
                if(PlayerPrefs.GetInt("missionDone") == 1)
                {
                    UISystem.uiSystem.StartDialogue(mission1end);
                    deskPlant.gameObject.SetActive(true);
                    deskPlant.interactable = true;
                }
                else
                {
                    UISystem.uiSystem.StartDialogue(mission1);
                    door.interactable = true;
                }
                break;
            case 2:
                UISystem.uiSystem.StartDialogue(mission2);
                door.interactable = true;
                break;
            case 3:
                UISystem.uiSystem.StartDialogue(mission3);
                door.interactable = true;
                break;
            case 4:
                UISystem.uiSystem.StartDialogue(mission4);
                door.interactable = true;
                break;
            default:
                break;

        }
    }

    public override string Prompt()
    {
        return prompt;
    }

    // Start is called before the first frame update
    void Start()
    {    
        prompt = "Answer";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
