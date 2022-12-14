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
    [SerializeField] TextMeshProUGUI speakerText;
    [SerializeField] TextMeshProUGUI dialogueMain;
    [SerializeField] TextMeshProUGUI dialogueChoiceMain;
    [SerializeField] GameObject[] choices;
    private DialogueObject currentDialogue;

    [SerializeField] TextMeshProUGUI missionText;
    [SerializeField] MissionObject firstMission;
    public List<MissionObject> missionList;

    //variables for the use of the dialogue camera
    [SerializeField] CinemachineTargetGroup dialogueTarget;
    [SerializeField] CinemachineVirtualCamera dialogueCam;

    [SerializeField] AudioSource jonathan;
    [SerializeField] AudioSource music1;
    [SerializeField] AudioSource music2;


    public float RANGE = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.value > .5f)
        {
            music1.Play();
        }
        else 
        { 
            music2.Play();
        }
        if(firstMission)
        {
            missionList.Add(ScriptableObject.Instantiate(firstMission));
        }
        uiSystem = this;
        playerTarget = null;
        cam = Camera.main;
        player = PlayerControl.player;
        dialoguePanel = GameObject.Find("DialoguePanel");
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
        if(currentDialogue.speaker == "Jonathan")
        {
            jonathan.Play();
        }
        speakerText.text = currentDialogue.speaker;
        dialogue = true;
        player.canMove = false;
        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);
        }
        if (currentDialogue.dialogueOptions.Length <= 1)
        {
            dialogueChoiceMain.gameObject.SetActive(false);
            dialogueMain.gameObject.SetActive(true);
            dialogueMain.text = currentDialogue.text;
        }
        else
        {
            dialogueMain.gameObject.SetActive(false);
            for (int i = 0; i < currentDialogue.dialogueOptions.Length; i++)
            {
                choices[i].SetActive(true);
                choices[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.dialogueOptions[i].text; 
            }
            dialogueChoiceMain.gameObject.SetActive(true);
            dialogueChoiceMain.text = currentDialogue.text;
        }
        dialogueMain.text = currentDialogue.text;
    }

    public void ContinueDialogue(int x = 0)
    {
        if(currentDialogue.dialogueOptions.Length != 0)
        {
            currentDialogue = currentDialogue.dialogueOptions[x];
            speakerText.text = currentDialogue.speaker;
            foreach (GameObject choice in choices)
            {
                choice.SetActive(false);
            }
            if (currentDialogue.dialogueOptions.Length < 2)
            {
                dialogueChoiceMain.gameObject.SetActive(false);
                dialogueMain.gameObject.SetActive(true);
                dialogueMain.text = currentDialogue.text;
            }
            else
            {
                dialogueMain.gameObject.SetActive(false);
                for (int i = 0; i < currentDialogue.dialogueOptions.Length; i++)
                {
                    choices[i].SetActive(true);
                    choices[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.dialogueOptions[i].text;
                }
                dialogueChoiceMain.gameObject.SetActive(true);
                dialogueChoiceMain.text = currentDialogue.text;
            }
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        if((PlayerPrefs.GetInt("mission") == 4) && (PlayerPrefs.GetInt("missionDone") == 1))
        {
            SceneManager.LoadScene(0);
        }
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
                    if(PlayerPrefs.GetInt("mission") == 1 || PlayerPrefs.GetInt("mission") == 4)
                    {
                        PlayerPrefs.SetInt("missionDone", 1);
                        SceneManager.LoadScene(5);
                    }
                    else
                    {
                        //main menu
                        SceneManager.LoadScene(0);
                    }

                }
                else
                {
                    missionList.Add(ScriptableObject.Instantiate(mission.nextMissions[0]));
                }
            }
        }   
    }
}
