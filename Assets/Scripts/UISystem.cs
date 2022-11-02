using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private TextMeshProUGUI speakerText;
    private TextMeshProUGUI dialogueMain;
    private DialogueObject currentDialogue;

    [SerializeField] TextMeshProUGUI missionText;
    [SerializeField] MissionObject firstMission;
    public List<MissionObject> missionList;

    //variables for the use of the dialogue camera
    [SerializeField] CinemachineTargetGroup dialogueTarget;
    [SerializeField] CinemachineVirtualCamera dialogueCam;

    public float RANGE = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if(firstMission)
        {
            missionList.Add(ScriptableObject.Instantiate(firstMission));
        }
        uiSystem = this;
        playerTarget = null;
        cam = Camera.main;
        player = PlayerControl.player;
        dialoguePanel = GameObject.Find("DialoguePanel");
        dialogueMain = dialoguePanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        speakerText = dialoguePanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
        dialoguePanel.SetActive(false);
    }

    public void SetPlayerTarget(Collider target)
    {
        playerTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        missionText.text = "";
        foreach (MissionObject mission in missionList)
        {
            if(mission.completed)
            {
                if (mission.completionProgress == 1)
                {
                    missionText.text += string.Format("<s>- {0}</s>\n", mission.text);
                }
                else
                {
                    missionText.text += string.Format("<s>- {0}/{1} {2}</s>\n", mission.progress, mission.completionProgress, mission.text);
                }
            }
            else
            {
                if(mission.completionProgress == 1)
                {
                    missionText.text += string.Format("- {0}\n", mission.text);
                }
                else
                {
                    missionText.text += string.Format("- {0}/{1} {2}\n", mission.progress, mission.completionProgress, mission.text);
                }   
            }
        }

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


        //interaction icon 
        if (playerTarget && !dialogue && playerTarget.GetComponent<Interactable>().interactable)
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
        speakerText.text = currentDialogue.speaker;
        dialogue = true;
        player.canMove = false;
    }

    public void ContinueDialogue()
    {
        if(currentDialogue.dialogueOptions.Length != 0)
        {
            currentDialogue = currentDialogue.dialogueOptions[0];
            speakerText.text = currentDialogue.speaker;
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

    public void ProgressMission(string name)
    {
        MissionObject mission = missionList[^1];
        if(mission.mission == name)
        {
            mission.progress++;
            if (mission.progress == mission.completionProgress)
            {
                mission.completed = true;
                if(mission.nextMissions.Length == 0)
                {
                    PlayerPrefs.SetInt("missions", PlayerPrefs.GetInt("missions") + 1);
                    SceneManager.LoadScene(0);
                }
                else
                {
                    missionList.Add(ScriptableObject.Instantiate(mission.nextMissions[0]));
                }
            }
        }   
    }
}
