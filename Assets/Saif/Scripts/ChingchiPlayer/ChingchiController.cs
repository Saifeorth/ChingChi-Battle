using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingchiController : ChingChiCharacter
{
    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private float turnSpeed = 360;

    [SerializeField]
    private Rigidbody rb;



    private Vector3 _input;



    //private Vector3 chingchiVelocity;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        Look();
        //CalculateMovementVelocity();
    }


    private void FixedUpdate()
    {
        //rb.velocity = chingchiVelocity * speed;

        Move();
    }



    void GatherInput()
    {
        _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }


    void Look()
    {
        if (_input != Vector3.zero)
        {
            var _relative = (transform.position + _input) - transform.position;
            var rot = Quaternion.LookRotation(_relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }

    }

    void Move()
    {
        Vector3 pos = transform.position + (transform.forward * _input.magnitude) * Time.deltaTime *speed;
        rb.MovePosition(pos);
    }

}
