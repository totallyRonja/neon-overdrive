using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDistortion : MonoBehaviour
{

    public static GridDistortion current;

    public float[] radius = new float[2];
    public Vector3[] position = new Vector3[2];
    Material gridMat;

    void Awake()
    {
        current = this;
        gridMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        //iterate over all distortion points
        for (int i = 0; i < 2; i++)
        {
            //set the shader values
            gridMat.SetFloat("radius" + i, radius[i]);
			gridMat.SetVector("distortionPoint" + i, position[i]);

            //deflate the points
            radius[i] = Mathf.MoveTowards(radius[i], 0, Time.deltaTime *100);
        }
    }

    //set a distortion point
    public void Distort(int index, Vector3 position, float radius)
    {
        this.radius[index] = radius;
        this.position[index] = position;
    }

    //start expanding a distortion point
    public void Expand(int index, float maxRadius, float speed)
    {
        StartCoroutine(ExpandAction(index, maxRadius, speed));
    }

    //expand a distortion point
    public IEnumerator ExpandAction(int i, float maxRadius, float speed)
    {
        while (radius[i] != maxRadius)
        {
            yield return null;
            radius[i] = Mathf.MoveTowards(radius[i], maxRadius, speed * Time.deltaTime);
        }
    }
}