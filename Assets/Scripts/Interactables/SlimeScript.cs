using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : Interactable
{
    [SerializeField] DialogueObject completionDialogue;
    public override void Interact()
    {
        if (UISystem.uiSystem.missionList[^1].mission == "slime" && UISystem.uiSystem.missionList[^1].progress == UISystem.uiSystem.missionList[^1].completionProgress - 1)
        {
            UISystem.uiSystem.StartDialogue(completionDialogue);
        }
        UISystem.uiSystem.ProgressMission("slime");
        Destroy(gameObject);
    }

    public override string Prompt()
    {
        return prompt;
    }

    // Start is called before the first frame update
    void Start()
    {
        prompt = "Pick Up";
        interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (UISystem.uiSystem.missionList[^1].mission == "slime" && !interactable)
        {
            interactable = true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
