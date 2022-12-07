using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScript : Interactable
{
    [SerializeField] Material[] normalMats;
    [SerializeField] MeshRenderer[] meshes;
    bool beenTriggered;
    [SerializeField] DialogueObject completionDialogue;
    public override void Interact()
    {
        for (int i = 0; i < 2; i++)
        {
            meshes[i].material = normalMats[i];
        }
        interactable = false;
        if(UISystem.uiSystem.missionList[^1].mission == "marker" && UISystem.uiSystem.missionList[^1].progress == UISystem.uiSystem.missionList[^1].completionProgress-1)
        {
            UISystem.uiSystem.StartDialogue(completionDialogue);
        }
        UISystem.uiSystem.ProgressMission("marker");
        //start dialogue if this is the last camera
    }

    public override string Prompt()
    {
        return prompt;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = false;
        }
        prompt = "Place";
        interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(UISystem.uiSystem.missionList[^1].mission == "marker" && !interactable && !beenTriggered)
        {
            interactable = true;
            beenTriggered = true;
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.enabled = true;
            }
        }
    }
}
