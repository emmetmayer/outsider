using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    private bool open;
    [SerializeField] GameObject door;

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
        if(!UISystem.uiSystem.dialogue)
        {
            UISystem.uiSystem.StartDialogue();
        }
        else
        {
            UISystem.uiSystem.EndDialogue();
        }

        if(open)
        {
            open = !open;
            prompt = "Open";
            door.SetActive(true);
        }
        else if(GameObject.Find("Player").GetComponent<PlayerControl>().hasKey == true)
        {
            open = !open;
            prompt = "Close";
            door.SetActive(false);
        }
    }
}
