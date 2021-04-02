using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaverBehaviour : MonoBehaviour, IPoolObject
{

    [SerializeField] private float rayDistance;
    [SerializeField] private float rayRadius;
    [SerializeField] private LayerMask housLayer;
    [SerializeField] private BoxCollider houseCollider;
    internal bool move { get; private set; }
    internal bool isAttaks { get; private set; }

    private int hashSpeed = Animator.StringToHash("Speed");

    internal Vector3 targetPosition;
    private Vector3 nextMove;

    private float angleRotate = 90;
    private float rotation = 0;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private GameObject _gameObject;
    internal State currentState { get; private set; }
    internal enum State
    {
        Attack,
        Steal,
        Queue,
        GoHome
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _gameObject = gameObject;
        currentState = State.Queue;


        rightRay = new Ray(_transform.position + Vector3.up, Vector3.right);
        backRay = new Ray(_transform.position +Vector3.up, -_transform.forward);
        leftRay = new Ray(_transform.position + Vector3.up, -Vector3.right);
    }
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
    internal void ToAttackState(Vector3 target)
    {
        currentState = State.Attack;
        move = true;
        targetPosition = target;
    }
    internal void ToStealState(Vector3 target)
    {
        currentState = State.Steal;
        move = true;
        targetPosition = target;
    }
    internal void GoHome(Vector3 target)
    {
        targetPosition = target;
        _transform.position = targetPosition - Vector3.forward * houseCollider.size.z;
        _transform.rotation = Quaternion.Euler(0, 180, 0);
        move = true;
        currentState = State.GoHome;
        _gameObject.SetActive(true);
        
    }
    
    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.GoHome:
                MoveToHome();
                break;
            case State.Queue:
                QueueLogic();
                break;
            default:
                MoveToTarget();
                break;
        }

    }
    private void MoveToTarget()
    {
        if (move)
        {
            nextMove = targetPosition - transform.position;

            if (Mathf.Abs(_rigidbody.position.x) > Mathf.Abs(targetPosition.x))
            {
                _rigidbody.MoveRotation(Quaternion.Euler(0,
                    Mathf.Lerp(rotation, 0, Time.deltaTime * BeaverData.Instance.RotationSpeed), 0));
            }
            else if (_rigidbody.position.z > targetPosition.z - houseCollider.size.z)
            {
                rotation = Mathf.Sign(nextMove.x) > 0 ? angleRotate : -angleRotate;
                _rigidbody.MoveRotation(Quaternion.Euler(0,
                    Mathf.Lerp(0, rotation, Time.deltaTime * BeaverData.Instance.RotationSpeed), 0));
            }
            
            nextMove = _transform.forward * BeaverData.Instance.RunSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + nextMove);

            _animator.SetFloat(hashSpeed, 1);
        }
    }
    
    private void MoveToHome()
    {
        if (move)
        {
            backRay = new Ray(_transform.position + Vector3.up, -Vector3.forward);
            if (Physics.SphereCast(backRay, rayRadius, rayDistance, housLayer))
            {
                rotation = _rigidbody.position.x > BeaverData.Instance.SpawnTransform.position.x ? -angleRotate : angleRotate;

                _rigidbody.MoveRotation(Quaternion.Euler
                    (0, Mathf.Lerp(0, rotation, Time.deltaTime * BeaverData.Instance.RotationSpeed), 0));
            }
            else
            {
                _rigidbody.MoveRotation(Quaternion.Euler
                    (0, Mathf.Lerp(0, 180, Time.deltaTime * BeaverData.Instance.RotationSpeed), 0));
            }

            nextMove = _transform.forward * BeaverData.Instance.RunSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + nextMove);

            _animator.SetFloat(hashSpeed, 1);
        }

        if(/*(BeaverData.Instance.SpawnTransform.position - transform.position).magnitude < 5f ||*/
            transform.position.z < BeaverData.Instance.SpawnTransform.position.z + 3)
        {
            move = false;
            currentState = State.Queue;
        }

    }
    private Ray backRay;
    private Ray rightRay;
    private Ray leftRay;
    [SerializeField] LayerMask beaverLayer;
    private RaycastHit raycastHit;
    private void QueueLogic()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        _animator.SetFloat(hashSpeed, 0);
        move = false;
        _gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up, rayRadius);
    }
}
