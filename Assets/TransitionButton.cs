using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionButton : MonoBehaviour {

    public MenuTransition trans;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(DoTransition);
	}

    void DoTransition()
    {
        StartCoroutine(trans.Transition());
    }
}
