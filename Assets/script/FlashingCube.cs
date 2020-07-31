using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingCube : MonoBehaviour
{
    //自身变量
    public float speed;
    public float rotateSpeed;
    public float deltaSpeed;
    public float maxSpeed;
    public float minSpeed;

    //造成伤害
    public int damage;
    public int damageChange; 


    //AI控制
    private GameObject Controller;
    private Vector3 positionDir;
    public float bounceRange;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GameObject.FindWithTag("Controller");
        positionDir = Controller.transform.position - transform.position;
        Physics.IgnoreCollision(Controller.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), true);
        this.GetComponent<Rigidbody>().velocity = speed * RandVector3().normalized * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.RotateTowards(this.GetComponent<Rigidbody>().velocity, positionDir, rotateSpeed * Time.deltaTime, 0.0f).normalized * speed * Time.deltaTime;
        if(GetAngle ()>90)
        {
            if(speed >minSpeed )
            speed -= deltaSpeed;
        }
        else if(GetAngle ()<90)
        {
            if(speed<maxSpeed )
            speed += deltaSpeed;
        }
        positionDir = Controller.transform.position - transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag ("ControllerTrigger"))
        {
            Controller.GetComponent<Controller>().hp -= damage + Random.Range(-damageChange, damageChange);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider .CompareTag ("wall"))
        {
            this.GetComponent<Rigidbody>().velocity += 2 * Vector3.Project(this.GetComponent<Rigidbody>().velocity, other.GetContact(0).normal);
        }
    }


    Vector3 RandVector3()
    {
        return new Vector3(Random.value, Random.value, Random.value);
    }

    float GetAngle()
    {
        return Mathf.Abs(Vector3.Angle(this.GetComponent<Rigidbody>().velocity, positionDir));
    }
}
