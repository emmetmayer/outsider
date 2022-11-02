using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class UISystem : MonoBehaviour
{
    public static UISystem uiSystem;

    Camera cam;
    PlayerControl player;
    public bool dialogue = false;

    //variables for the use of the interaction system
    [SerializeField] Image interactImg;
    [SerializeField] TextMeshProUGUI interactPrompt;
    [SerializeField] Collider playerTarget;

    [SerializeField] GameObject dialoguePanel;
    private TextMeshProUGUI dialogueMain;
    private DialogueObject currentDialogue;

    //variables for the use of the dialogue camera
    [SerializeField] CinemachineTargetGroup dialogueTarget;
    [SerializeField] CinemachineVirtualCamera dialogueCam;

    public float RANGE = 5f;

    // Start is called before the first frame update
    void Start()
    {
        uiSystem = this;
        playerTarget = null;
        cam = Camera.main;
        player = PlayerControl.player;
        dialoguePanel = GameObject.Find("DialoguePanel");
        dialogueMain = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
        dialoguePanel.SetActive(false);
    }

    public void SetPlayerTarget(Collider target)
    {
        playerTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTarget)
        {
            dialogueTarget.m_Targets = null;
            dialogueTarget.AddMember(player.transform, 1f, 0f);
            dialogueTarget.AddMember(playerTarget.transform, 1f, 0f);
            
            Vector2 temp = Vector2.Perpendicular(new Vector2(playerTarget.transform.position.x, playerTarget.transform.position.z) - new Vector2(player.controller.transform.position.x, player.controller.transform.position.z));
            temp = temp.normalized * RANGE;
            if(player.controller.transform.position.x < playerTarget.transform.position.x)
            {
                temp = -temp;
            }
            dialogueCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(temp.x, 1f, temp.y);
        }



        if (playerTarget && !dialogue)
        {
            interactImg.transform.position = cam.WorldToScreenPoint(playerTarget.transform.position);
            interactImg.gameObject.SetActive(true);
            interactPrompt.text = playerTarget.GetComponent<Interactable>().Prompt();
        }
        else
        {
            interactImg.gameObject.SetActive(false);
        }
    }
    public void StartDialogue(DialogueObject dialogueObj)
    {
        CameraManager.cameraManager.SwitchCamera(dialogueCam);
        dialoguePanel.SetActive(true);
        currentDialogue = dialogueObj;
        dialogueMain.text = currentDialogue.text;
        dialogue = true;
        player.canMove = false;
    }

    public void ContinueDialogue()
    {
        if(currentDialogue.dialogueOptions.Length != 0)
        {
            currentDialogue = currentDialogue.dialogueOptions[0];
            dialogueMain.text = currentDialogue.text;
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        CameraManager.cameraManager.SwitchCamera(PlayerControl.player.mainCamera);
        dialoguePanel.SetActive(false);
        dialogue = false;
        player.canMove = true;
    }
}
