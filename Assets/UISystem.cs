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



        if (playerTarget)
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
    public void StartDialogue()
    {
        CameraManager.cameraManager.SwitchCamera(dialogueCam);
        dialogue = true;
    }

    public void EndDialogue()
    {
        CameraManager.cameraManager.SwitchCamera(PlayerControl.player.mainCamera);
        dialogue = false;
    }
}
