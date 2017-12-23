using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUi : MonoBehaviour {

    public Text textComponent;

	public string drawText;
	public string winText;
	public string pauseText;
	public Color pauseTextColor;

    public Button[] buttons;

	[HideInInspector] public bool gameOver = false;

	Graphic[] graphicElements;

	bool paused = false;

	void Awake(){
		graphicElements = GetComponentsInChildren<Graphic> ();
		for (int i = 0; i < graphicElements.Length; i++) {
			graphicElements [i].enabled = false;
		}
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].enabled = false;
		}
	}

	void Update(){
		if (!gameOver && Input.GetButtonDown ("Cancel")) {
			SetPaused (!paused);
		}
	}

	public void SetPaused(bool newPausedState){
		paused = newPausedState;

		for (int i = 0; i < graphicElements.Length; i++) {
			graphicElements [i].enabled = newPausedState;
		}
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].enabled = newPausedState;
		}

		if (paused) {
			textComponent.text = pauseText;
			textComponent.color = pauseTextColor;
			Screenshake.current.StopAllCoroutines ();
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	public void Win(TeamProperty team)
    {
		SetPaused (true);
		gameOver = true;

		if (team == null) {
			textComponent.text = drawText;
			textComponent.color = Color.white;
		} else {
			textComponent.text = string.Format(winText, team.name);
			textComponent.color = team.teamColor;
		}
        StartCoroutine(Interactable());
    }

    IEnumerator Interactable()
    {
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }
        yield return new WaitForSecondsRealtime(1f);
        foreach (Button b in buttons)
        {
            b.interactable = true;
        }
    }
}
