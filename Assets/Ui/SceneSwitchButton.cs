using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitchButton : MonoBehaviour {

    public string sceneToLoad;
    //public bool actionButton;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(LoadScene);
	}

    /*void Update()
    {
        if (actionButton)
        {
            if(Input.GetButton("Fire_Cyan") || Input.GetButton("Fire_Magenta"))
            {
                LoadScene();
            }
        }
    }*/

    // Update is called once per frame
    void LoadScene () {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToLoad);
	}
}
