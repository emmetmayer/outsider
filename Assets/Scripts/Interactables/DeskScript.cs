using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeskScript : Interactable
{
    [SerializeField] Material normalMat;
    bool beenTriggered;
    public override void Interact()
    {
        gameObject.GetComponent<MeshRenderer>().material = normalMat;
        interactable = false;
        SceneManager.LoadScene(5);
    }

    public override string Prompt()
    {
        return prompt;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        prompt = "Place";
        interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
