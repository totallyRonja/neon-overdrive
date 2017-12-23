using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTransition : MonoBehaviour {


    public Button[] startButtons;
    public Button[] levelSelectButtons;

    public float startHeight;
    public float levelSelectHeight;

    public float transitionDuration;

    float currentHeight = 0;

    void Start(){
        currentHeight = startHeight;
        transform.localPosition = new Vector3(0, startHeight, 0);
    }
	
	public IEnumerator Transition() {
        if(currentHeight == startHeight)
            currentHeight = levelSelectHeight;
        else if(currentHeight == levelSelectHeight)
            currentHeight = startHeight;
        else {
            Debug.LogWarning("element has invalid height, resetting to start height.");
            currentHeight = startHeight;
        }

        //activate the buttons of the menu that's transitioned to, deactivate the others
        foreach(Button b in startButtons) {
            b.interactable = currentHeight == startHeight;
        }
        foreach (Button b in levelSelectButtons) {
            b.interactable = currentHeight == levelSelectHeight;
        }

        //interpolate menu position
        float startTime = Time.time;
        float transitionStartHeight = transform.localPosition.y;
        while (Time.time < startTime + transitionDuration) {
            yield return null;
            transform.localPosition = new Vector3(0, Mathf.Lerp(transitionStartHeight, currentHeight, (Time.time - startTime)/transitionDuration), 0);
        }
        transform.localPosition = new Vector3(0, currentHeight, 0);
    }
}
