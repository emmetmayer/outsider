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
    public CinemachineFreeLook mainCamera;
    public Camera moveCamera;

    Collider target;

    [SerializeField] float speed;
    [SerializeField] float turnSmoothVelocity;
    [SerializeField] float turnSpeed;

    public bool hasKey;

    [SerializeField] Animator anim;

    private void Awake()
    {
        player = this;
        canMove = true;
        controller = GetComponent<CharacterController>();
        mainCamera = GetComponentInChildren<CinemachineFreeLook>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        moveCamera = Camera.main;
    }

    //Update is called once per frame
    void Update()
    {
        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * .2f);
        }

        target = FindInteractables(); 

        UISystem.uiSystem.SetPlayerTarget(target);

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(UISystem.uiSystem.dialogue)
            {
                UISystem.uiSystem.ContinueDialogue();
            }
            else if (target && target.GetComponent<Interactable>().interactable)
            {
                target.GetComponent<Interactable>().Interact();
            } 
        }

        if(canMove)
        {

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            if (input.magnitude >= 0.001f)
            {
                anim.Play("walk");
                float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + moveCamera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirection.normalized * Time.deltaTime * speed);
            }
            
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
                if (Vector3.Distance(controller.transform.position, hit.transform.position) < minDist && check.collider == hit && hit.gameObject.GetComponent<Interactable>().interactable)
                {
                    Debug.Log("found int");
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "LocateTrigger")
        {
            UISystem.uiSystem.ProgressMission("locate");
        }
        if (other.name == "BaseTrigger")
        {
            UISystem.uiSystem.ProgressMission("base");
        }
    }
}
