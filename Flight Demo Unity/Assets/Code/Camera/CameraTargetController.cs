using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;

public class CameraTargetController : MonoBehaviour
{

    public VectorReference playerDirection;
    public Transform target;
    public float lookSpeed;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.LookAt(transform.position + playerDirection.CurrentValue);
        transform.LookAt(transform.position + Vector3.MoveTowards(transform.forward, playerDirection.CurrentValue, lookSpeed * Time.deltaTime), Vector3.up);
        transform.position = target.position;
    }

}
