using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool opened;
    public bool toopen;
    private float axisOffset;
    private Vector3 axisPoint;
    private Vector3 axis;
    // Start is called before the first frame update
    void Start()
    {
        opened = false;
        toopen = false;
        axisOffset = 0.5f * gameObject.GetComponent<MeshRenderer>().bounds.size.z;
        axisPoint = transform.position + Vector3.forward  * axisOffset;
        axis = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (toopen)
            StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        if (!opened)
        {
            opened  = true;
            for (int i = 0; i < 15; i++)
            {
                transform.RotateAround(axisPoint, axis, -6);
                yield return new WaitForSeconds(1 / 15);
            }
        }
    }
}
