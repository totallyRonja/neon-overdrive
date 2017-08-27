using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTransition : MonoBehaviour {

    public Button[] main;
    public Button[] level;

    public int startY;
    public int levelY;

    bool start = false;

	// Use this for initialization
	void Start () {
		
	}
	
	public IEnumerator Transition()
    {
        foreach(Button b in start ? main : level)
        {
            b.interactable = true;
        }
        start = !start;
        foreach (Button b in start ? main : level)
        {
            b.interactable = false;
        }

        float startTime = Time.time;
        while (Time.time < startTime + 1)
        {
            yield return null;
            transform.localPosition = Vector3.up * Mathf.Lerp(start?startY:levelY, start?levelY:startY, Time.time - startTime);
        }
    }
}
