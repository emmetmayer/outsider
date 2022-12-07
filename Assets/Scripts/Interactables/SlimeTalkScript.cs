using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeTalkScript : Interactable
{
    [SerializeField] DialogueObject dialogue;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource talk;
    [SerializeField] AudioSource idle;
    private bool newTalk;
    public override void Interact()
    {
        if(newTalk)
        {
            UISystem.uiSystem.ProgressMission("talk");
            newTalk = false;
        }
        talk.Play();
        UISystem.uiSystem.StartDialogue(dialogue);
        animator.Play("talking");
    }

    public override string Prompt()
    {
        return prompt;
    }

    // Start is called before the first frame update
    void Start()
    {
        idle.Play();
        prompt = "Talk";
        newTalk = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
