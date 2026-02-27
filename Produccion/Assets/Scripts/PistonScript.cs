using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonScript : MonoBehaviour
{
    [SerializeField] float goingSpeed;
    [SerializeField] float returningSpeed;
    float moveSpeed;
    Rigidbody2D rb;
    [SerializeField] GameObject limit;
    [SerializeField] GameObject origin;
    [SerializeField] GameObject piston;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = returningSpeed;
    }

    void Update()
    {
        Move();
        piston.transform.localScale = new Vector3(Mathf.Abs(origin.transform.position.x - transform.position.x), piston.transform.localScale.y, piston.transform.localScale.z) ; 
    }

    void Move()
    {
        if (transform.position.x <= limit.transform.position.x)
            moveSpeed = returningSpeed;
        if (transform.position.x >= origin.transform.position.x)
            moveSpeed = goingSpeed;

        rb.velocity = new Vector2(moveSpeed, 0);
    }
}