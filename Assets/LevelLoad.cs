using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(int level)
    {
        PlayerPrefs.SetInt("mission", level);
        PlayerPrefs.SetInt("missionDone", 0);
        SceneManager.LoadScene(5);
    }
}
