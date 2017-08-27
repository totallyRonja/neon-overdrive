using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    Button b;

	// Use this for initialization
	void Start () {
        b = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		if((EventSystem.current.currentSelectedGameObject == null && b.interactable) || (EventSystem.current.currentSelectedGameObject != null && !EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable))
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
	}
}
