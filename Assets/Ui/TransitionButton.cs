using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionButton : MonoBehaviour {
    public MenuTransition transition;

    public void Transition()
    {
        StartCoroutine(transition.Transition());
    }
}
