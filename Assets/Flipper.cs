using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : Bouncy {

    public Transform mesh;
    public AnimationCurve flip;

    public override void Bounce()
    {
        StopAllCoroutines();
        StartCoroutine(BounceAction());
    }

    IEnumerator BounceAction()
    {
        float startTime = Time.time;
        float endTime = startTime + flip.keys[flip.length-1].time;
        while(Time.time < endTime)
        {
            yield return null;
            mesh.localEulerAngles = Vector3.up * flip.Evaluate(Time.time - startTime);
        }
        mesh.rotation = Quaternion.identity;
    }
}
