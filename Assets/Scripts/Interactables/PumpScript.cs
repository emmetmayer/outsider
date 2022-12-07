using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpScript : Interactable
{
    [SerializeField] Material[] normalMats;
    [SerializeField] MeshRenderer[] meshes;
    bool beenTriggered;
    [SerializeField] DialogueObject completionDialogue;
    [SerializeField] AudioSource hum;
    public override void Interact()
    {
        for (int i = 0; i < 4;  i++)
        {
            meshes[i].material = normalMats[i];
        }
        interactable = false;
        if(UISystem.uiSystem.missionList[^1].mission == "pump" && UISystem.uiSystem.missionList[^1].progress == UISystem.uiSystem.missionList[^1].completionProgress-1)
        {
            UISystem.uiSystem.StartDialogue(completionDialogue);
            hum.Play();
        }
        UISystem.uiSystem.ProgressMission("pump");
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
        if(UISystem.uiSystem.missionList[^1].mission == "pump" && !interactable && !beenTriggered)
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
