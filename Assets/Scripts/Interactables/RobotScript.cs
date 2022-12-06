using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : Interactable
{
    [SerializeField] Material[] normalMats;
    bool beenTriggered;
    [SerializeField] DialogueObject completionDialogue;
    public override void Interact()
    {
        gameObject.GetComponent<MeshRenderer>().materials = normalMats;
        interactable = false;
        if(UISystem.uiSystem.missionList[^1].mission == "robot" && UISystem.uiSystem.missionList[^1].progress == UISystem.uiSystem.missionList[^1].completionProgress-1)
        {
            UISystem.uiSystem.StartDialogue(completionDialogue);
        }
        UISystem.uiSystem.ProgressMission("robot");
        //start dialogue if this is the last camera
    }

    public override string Prompt()
    {
        return prompt;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        prompt = "Place";
        interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(UISystem.uiSystem.missionList[^1].mission == "robot" && !interactable && !beenTriggered)
        {
            interactable = true;
            beenTriggered = true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
