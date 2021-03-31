using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private Vector3 rotate;
    private Vector3 nextMove;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private InputController input;
    private Animator animator;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        input = GetComponent<InputController>();
        animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        animator.SetFloat("VerticalInput", input.verticalInput);
    }
    private void FixedUpdate()
    {
        Movement();
    }
    
    private void Movement()
    {
        rotate += Vector3.up * input.horizontalInput * rotationSpeed * Time.deltaTime;
        _rigidbody.MoveRotation(Quaternion.Euler(rotate));
        nextMove = _transform.forward * input.verticalInput * moveSpeed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + nextMove);
    }
}
