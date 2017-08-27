using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDistortion : MonoBehaviour
{

    public static GridDistortion current;

    public float[] radius = new float[2];
    public Vector3[] position = new Vector3[2];
    Material gridMat;

    // Use this for initialization
    void Awake()
    {
        current = this;
        gridMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            //print(position[i]);
            gridMat.SetFloat("radius" + i, radius[i]);
            gridMat.SetVector("origin" + i, position[i]);
            radius[i] = Mathf.MoveTowards(radius[i], 0, Time.deltaTime *100);
        }
    }

    public void Distort(int index, Vector3 position, float radius)
    {
        this.radius[index] = radius;
        this.position[index] = position;
    }

    public void Expand(int index, float maxRadius, float speed)
    {
        StartCoroutine(ExpandAction(index, maxRadius, speed));
    }

    public IEnumerator ExpandAction(int i, float maxRadius, float speed)
    {
        while (radius[i] != maxRadius)
        {
            yield return null;
            radius[i] = Mathf.MoveTowards(radius[i], maxRadius, speed * Time.deltaTime);
        }
    }
}