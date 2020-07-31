using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameObject controller;
    private Transform contrans;
    public  bool tofall;
    private bool falled;
    private  float axisOffset;
    private Vector3 axisPoint;
    private Vector3 axis;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Controller");
        contrans = controller.GetComponent<Transform>();
        falled = false;
        axisOffset = 0.5f * gameObject.GetComponent<MeshRenderer>().bounds.size.y;
        axisPoint = transform.position + Vector3.down * axisOffset;
        axis = Vector3.forward ;
    }
    private void Update()
    {
        if (tofall)
           StartCoroutine(Fall());
    }
    IEnumerator Fall()
    {
        if (!falled)
        {
            falled = true;
            if (transform.InverseTransformPoint(contrans.position).x > 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    transform.RotateAround(axisPoint, axis, -6);
                    yield return new WaitForSeconds(1 / 15);
                }
            }
            else
            {
                for (int i = 0; i < 15; i++)
                {
                    transform.RotateAround(axisPoint, axis, 6);
                    yield return new WaitForSeconds(1 / 15);
                }
            }
            Destroy (gameObject , 2);
        }
    }
}
