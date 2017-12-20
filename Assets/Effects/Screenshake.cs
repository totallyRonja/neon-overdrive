using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour {

    //Singleton instance
    public static Screenshake current;

    public float intensity = 1;

    [HideInInspector] public bool shaking = false;

    Vector3 basePosition;

	void Awake () {
        current = this;
        basePosition = transform.position;
	}
	
	void LateUpdate () {
        if (shaking)
        {
            transform.position = basePosition + Random.insideUnitSphere * intensity * Time.timeScale;
            shaking = false;
        } else
        {
            transform.position = basePosition;
        }
    }

    public void Shake(float duration, float intensity = 1)
    {
        StartCoroutine(ShakeAction(duration, intensity));
    }

    IEnumerator ShakeAction(float duration, float intensity)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            shaking = true;
            yield return null;
        }
        transform.position = basePosition;
    }

    public void FreezeFrames(float duration)
    {
        StartCoroutine(FreezeFramesAction(duration));
    }

    IEnumerator FreezeFramesAction(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }
}
