using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingCube : MonoBehaviour
{
    //自身变量
    public float coefficient;
    public float index;


    //AI控制
    private GameObject Controller;
    private Vector3 positionDir;
    public float bounceRange;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GameObject.FindWithTag("Controller");
        positionDir = Controller.transform.position - transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    Vector3 RandVector3()
    {
        return new Vector3(Random.value, Random.value, Random.value);
    }

    float GetAngle()
    {
        return Mathf.Abs(Vector3.Angle(-Controller.transform.forward, positionDir));
    }
}
