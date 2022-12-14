using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : Interactable
{

    // Start is called before the first frame update
    private void Start()
    {
        prompt = "Exit";
        interactable = false;
    }
    
    // Update is called once per frame
    override public string Prompt()
    {
        return prompt;
    }

    // Update is called once per frame
    override public void Interact()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("mission"));
    }
}
