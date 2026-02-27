using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private GameObject worldCamera;

    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector2 inputVector;
    private Rigidbody2D rb;

    [SerializeField] private LayerMask enemiesLayer;

    private Animator animator;

    public GameObject WorldCamera => worldCamera;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool IsMoving => isMoving;
    public Vector2 InputVector => inputVector;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        // MOVIMIENTO
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        inputVector = new Vector2(moveX, moveY).normalized;

        if (!GameManager.instance.Chatting || !GameManager.instance.InStore || !MenuManager.instance.menu.activeInHierarchy)
        {
            // ANIMACIONES
            animator.SetFloat("Horizontal", inputVector.x);
            animator.SetFloat("Vertical", inputVector.y);
            animator.SetFloat("Speed", inputVector.sqrMagnitude);

        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputVector * moveSpeed * Time.deltaTime);
    }
}
