using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(Exit);
	}
	
	// Update is called once per frame
	void Exit () {
        Application.Quit();
	}
}
