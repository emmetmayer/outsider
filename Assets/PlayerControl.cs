using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl player;
    const float INTERACT_RANGE = 5f;

    public CharacterController controller;
    public bool canMove;
    public CinemachineVirtualCamera mainCamera;

    Collider target;

    [SerializeField] float speed;

    public bool hasKey;

    // Start is called before the first frame update
    void Start()
    {
        player = this;
        canMove = true;
        controller = GetComponent<CharacterController>();
        mainCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    //Update is called once per frame
    void Update()
    {
        target = FindInteractables(); 

        UISystem.uiSystem.SetPlayerTarget(target);

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(UISystem.uiSystem.dialogue)
            {
                UISystem.uiSystem.ContinueDialogue();
            }
            else if (target)
            {
                target.GetComponent<Interactable>().Interact();
            } 
        }

        if(canMove)
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            controller.Move(input * Time.deltaTime * speed);
        }
    }

    private Collider FindInteractables()
    {
        Collider[] hits;
        Collider close = null;
        hits = Physics.OverlapSphere(controller.transform.position, INTERACT_RANGE, LayerMask.GetMask("Interactable"));
        if (hits.Length > 0)
        {
            float minDist = INTERACT_RANGE;
            foreach (Collider hit in hits)
            {
                //check to see if there is anything inbetween the player and the interactable
                RaycastHit check;
                Physics.Raycast(controller.transform.position, hit.transform.position - controller.transform.position, out check);
                if (Vector3.Distance(controller.transform.position, hit.transform.position) < minDist && check.collider == hit)
                {
                    minDist = Vector3.Distance(controller.transform.position, hit.transform.position);
                    close = hit;
                }
            }
        }
        return close;
    }

    private void OnDrawGizmos()
    {
        if (controller) { Gizmos.DrawWireSphere(controller.transform.position, INTERACT_RANGE); }
    }
}
