using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaverBehaviour : MonoBehaviour, IPoolObject
{
    [SerializeField] private float rayDistance;
    [SerializeField] private float rayRadius;
    [SerializeField] private LayerMask housLayer;
    [SerializeField] private BoxCollider houseCollider;
    [SerializeField] private GameObject renderObject;
    internal bool move { get; private set; }

    private int hashRun = Animator.StringToHash("Run");
    private int hashWalk = Animator.StringToHash("Walk");
    private int hashIdle = Animator.StringToHash("Idle");

    internal Vector3 targetPosition;
    private Vector3 nextMove;

    private float angleRotate = 90;
    private float rotation = 0;
    private float spawnPointX;

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
        GoHome,
        Wait
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _gameObject = gameObject;
        currentState = State.Wait;
        

    }
    internal void Die()
    {
        BeaversController.RemoveFromAvailableList(this);
        ReturnToPool();
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
    internal void ToQueueState()
    {
        savedPosition = _transform.position;
        currentState = State.Queue;
    }
    internal void GoHome(Vector3 target)
    {
        targetPosition = target;
        _transform.position = targetPosition - Vector3.forward * houseCollider.size.z;
        _transform.rotation = Quaternion.Euler(0, 180, 0);
        move = true;
        currentState = State.GoHome;
        renderObject.SetActive(true);
        
    }
    
    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Wait:
                break;
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

            _animator.SetBool(hashRun, true);
        }
    }
    
    private void MoveToHome()
    {
        if (move)
        {
            Ray backRay = new Ray(_transform.position + Vector3.up, -Vector3.forward);
            if (Physics.SphereCast(backRay, rayRadius, rayDistance, housLayer) ||
                _rigidbody.position.z < BeaversController.QueuePosition().z)
            {

                spawnPointX = _rigidbody.position.z > (BeaversController.spawnPoint.z + 5) ?
                    BeaversController.spawnPoint.x : BeaversController.QueuePosition().x;

                rotation = _rigidbody.position.x > spawnPointX ? -angleRotate : angleRotate;

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

            _animator.SetBool(hashRun, true);
        }

        if ((BeaversController.QueuePosition() - _transform.position).magnitude < 0.3f)
        {
            move = false;
            currentState = State.Wait;
            _transform.rotation = Quaternion.Euler(0, 0, 0);
            BeaversController.AddToQueue(this);
            _animator.SetBool(hashRun, false);
        }

    }
    private Vector3 savedPosition;
    
    private void QueueLogic()
    {
        _animator.SetBool(hashWalk, true);
        if (_rigidbody.position.x > savedPosition.x + 1.5f)
        {
            _rigidbody.MoveRotation(Quaternion.Euler
                    (0, Mathf.Lerp(angleRotate, 0, Time.deltaTime * BeaverData.Instance.RotationSpeed), 0));
            if(_transform.rotation.eulerAngles.y == 0)
            {
                currentState = State.Wait;
                _animator.SetBool(hashWalk, false);
            }
        }
        else
        {
            _rigidbody.MoveRotation(Quaternion.Euler
                        (0, Mathf.Lerp(0, angleRotate, Time.deltaTime * BeaverData.Instance.RotationSpeed), 0));

            nextMove = _transform.forward * BeaverData.Instance.WalkSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + nextMove);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        _animator.SetBool(hashRun, false);
        move = false;
        renderObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up, rayRadius);
    }
}
