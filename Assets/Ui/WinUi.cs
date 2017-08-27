using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUi : MonoBehaviour {

    public Text winText;

    public Button[] buttons;

    public void Win(PlayerTeam team)
    {
        gameObject.SetActive(true);
        Screenshake.current.StopAllCoroutines();
        Time.timeScale = 0;
        switch (team)
        {
            case PlayerTeam.CYAN:
                winText.text = "CYAN\nWINS";
                winText.color = Color.cyan;
                break;
            case PlayerTeam.MAGENTA:
                winText.text = "MAGENTA\nWINS";
                winText.color = Color.magenta;
                break;
            default:
                winText.text = "<color=#F0F>D</color><color=#0FF>R</color><color=#F0F>A</color><color=#0FF>W</color>";
                winText.color = Color.white;
                break;

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
