using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitchButton : MonoBehaviour {

    public SceneField sceneToLoad;

    public void LoadScene () {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToLoad);
	}
}
