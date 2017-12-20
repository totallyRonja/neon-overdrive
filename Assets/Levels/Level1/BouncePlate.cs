using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlate : Bouncy {
    bool hidden = false;

    public override void Bounce()
    {
        if(!hidden)
            StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        float startTime = Time.time;
        while(Time.time < startTime + 1)
        {
            yield return null;
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(0, -4, Time.time - startTime);
            transform.position = pos;
        }
    }

    IEnumerator Show()
    {
        float startTime = Time.time;
        while (Time.time < startTime + 1)
        {
            yield return null;
            Vector3 pos = transform.position;
            pos.z = Mathf.Lerp(-4, 0, Time.time - startTime);
            transform.position = pos;
        }
    }
}
