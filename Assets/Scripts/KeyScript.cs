using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : Interactable
{
    public override void Interact()
    {
        GameObject.Find("Player").GetComponent<PlayerControl>().hasKey = true;
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
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0f, 1f, 0f), Space.World);
    }
}
