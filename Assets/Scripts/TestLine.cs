using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(10.0f, 30.0f, 10.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
