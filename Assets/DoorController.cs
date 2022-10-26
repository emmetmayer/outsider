using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    private bool open;
    [SerializeField] GameObject door;
    [SerializeField] DialogueObject keyDialogue;
    [SerializeField] DialogueObject noKeyDialogue;

    // Start is called before the first frame update
    private void Start()
    {
        open = false;
        prompt = "Open";
        door.SetActive(!open);
    }
    
    // Update is called once per frame
    override public string Prompt()
    {
        return prompt;
    }

    // Update is called once per frame
    override public void Interact()
    {
        if(!open)
        {
            if (GameObject.Find("Player").GetComponent<PlayerControl>().hasKey == true)
            {
                UISystem.uiSystem.StartDialogue(keyDialogue);
                open = !open;
                prompt = "Close";
                door.SetActive(false);
            }
            else
            {
                UISystem.uiSystem.StartDialogue(noKeyDialogue);
            }
        }
        else
        {
            open = !open;
            prompt = "Open";
            door.SetActive(true);
        }
    }
}
