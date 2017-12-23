using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour {

    public Text text;
	public PauseUi pauseUI;

	void Start () {
        StartCoroutine(Countdown(3));
	}

    IEnumerator Countdown(int secondsLeft)
    {
		float startTime = Time.realtimeSinceStartup;
		
        Time.timeScale = 0;
		pauseUI.gameOver = true;
        text.text = ("<color=#" + (Random.value > 0.5f ? "F0F" : "0FF") + ">") + (secondsLeft > 0 ? "" + secondsLeft : "GO") + "</color>";
		while (Time.realtimeSinceStartup < startTime + 1)
        {
            yield return null;
            text.rectTransform.localScale = Vector3.one * (1 + (Time.realtimeSinceStartup - startTime)*0.5f);
        }
        if (secondsLeft <= 0)
        {
            Time.timeScale = 1;
			pauseUI.gameOver = false;
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(Countdown(secondsLeft - 1));
        }
    }
}
