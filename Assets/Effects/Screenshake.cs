using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour {

    public static Screenshake current;

    Vector3 basePosition;
    public bool shaking = false;
    public float intensity = 1;

	// Use this for initialization
	void Awake () {
        current = this;
        basePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (shaking)
        {
            print(Time.timeScale);
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
            transform.position = basePosition + Random.insideUnitSphere * intensity * Time.timeScale;
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
