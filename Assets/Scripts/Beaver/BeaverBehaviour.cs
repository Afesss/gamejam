using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

internal class BeaverBehaviour : MonoBehaviour, IPoolObject
{
    [SerializeField] private BeaverSettings beaverSettings;
    [SerializeField] private float rayDistance;
    [SerializeField] private float rayRadius;
    [SerializeField] private LayerMask housLayer;
    [SerializeField] private BoxCollider houseCollider;
    [SerializeField] private GameObject renderObject;
    [SerializeField] private AudioSource runAudioSource;
    [SerializeField] private AudioSource sweemAudioSource;


    internal bool move { get; private set; }


    private int stealedChocolateAmount;
    private int hashRun = Animator.StringToHash("Run");
    private int hashWalk = Animator.StringToHash("Walk");

    internal Vector3 targetPosition;
    private Vector3 nextMove;
    private Vector3 savedPosition;

    private float angleRotate = 90;
    private float rotation = 0;
    private float spawnPointX;

    
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private GameObject _gameObject;
    private HouseBeaverDetector houseBeaverDetector;
    private HouseChocolate houseChocolate;
    private Collider triggerCollider;
    private bool isSweeming;
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
        stealedChocolateAmount = 0;
    }
    internal void Die()
    {
        houseBeaverDetector.OnDestroyBeaver -= Die;

        houseChocolate?.ReturnStealdChocolate(stealedChocolateAmount);
        stealedChocolateAmount = 0;
        
        houseChocolate.OnChocolateSteal -= OnChocolateSteal;
        houseChocolate = null;
        houseBeaverDetector = null;
        BeaversController.RemoveFromAvailableList(this);
        renderObject.SetActive(true);
        ReturnToPool();
        if (BeaversController.AvailableBeavers == 0 && BeaversController.ChocolateAmount < BeaversController.CurrentPrice)
        {
            EventBroker.GameOverInvoke();
        }
        triggerCollider.enabled = true;
        triggerCollider = null;

        if (isSweeming)
            sweemAudioSource.Stop();
        else
            runAudioSource.Stop();

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

        if (isSweeming)
            sweemAudioSource.Play();
        else
            runAudioSource.Play();

    }
    internal void ToStealState(Vector3 target)
    {
        currentState = State.Steal;
        move = true;
        targetPosition = target;

        if (isSweeming)
            sweemAudioSource.Play();
        else
            runAudioSource.Play();

    }
    internal void ToQueueState()
    {
        savedPosition = _transform.position;
        currentState = State.Queue;

        if (isSweeming)
            sweemAudioSource.Play();
        else
            runAudioSource.Play();

    }
    internal void GoHome(Vector3 target)
    {
        targetPosition = target;
        _transform.position = targetPosition - Vector3.forward * houseCollider.size.z;
        _transform.rotation = Quaternion.Euler(0, 180, 0);
        move = true;
        currentState = State.GoHome;
        renderObject.SetActive(true);

        houseChocolate.OnChocolateSteal -= OnChocolateSteal;
        houseChocolate = null;
        houseBeaverDetector.OnDestroyBeaver -= Die;
        houseBeaverDetector = null;

        if (isSweeming)
            sweemAudioSource.Play();
        else
            runAudioSource.Play();

    }

    private void FixedUpdate()
    {
        //Debug.Log(stealedChocolateAmount);
        if (_rigidbody.IsSleeping())
            _rigidbody.WakeUp();

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
                    Mathf.Lerp(rotation, 0, Time.deltaTime * beaverSettings.RotationSpeed), 0));
            }
            else if (_rigidbody.position.z > targetPosition.z - houseCollider.size.z)
            {
                rotation = Mathf.Sign(nextMove.x) > 0 ? angleRotate : -angleRotate;
                _rigidbody.MoveRotation(Quaternion.Euler(0,
                    Mathf.Lerp(0, rotation, Time.deltaTime * beaverSettings.RotationSpeed), 0));
            }
            
            nextMove = _transform.forward * beaverSettings.RunSpeed * Time.deltaTime;
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
                    (0, Mathf.Lerp(0, rotation, Time.deltaTime * beaverSettings.RotationSpeed), 0));
            }
            else
            {
                _rigidbody.MoveRotation(Quaternion.Euler
                    (0, Mathf.Lerp(0, 180, Time.deltaTime * beaverSettings.RotationSpeed), 0));
            }

            nextMove = _transform.forward * beaverSettings.RunSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + nextMove);

            _animator.SetBool(hashRun, true);
        }

        ToWaitState();

    }
    private void ToWaitState()
    {
        if ((BeaversController.QueuePosition() - _transform.position).magnitude < 0.3f)
        {
            if (stealedChocolateAmount > 0)
            {
                BeaversController.AddChocolateToStock(stealedChocolateAmount);
                stealedChocolateAmount = 0;
            }
            move = false;
            currentState = State.Wait;
            _transform.rotation = Quaternion.Euler(0, 0, 0);
            BeaversController.AddToQueue(this);
            _animator.SetBool(hashRun, false);

            if (isSweeming)
                sweemAudioSource.Stop();
            else
                runAudioSource.Stop();

        }
    }
    
    private void QueueLogic()
    {
        _animator.SetBool(hashWalk, true);
        if (_rigidbody.position.x > savedPosition.x + 1.5f)
        {
            _rigidbody.MoveRotation(Quaternion.Euler
                    (0, Mathf.Lerp(angleRotate, 0, Time.deltaTime * beaverSettings.RotationSpeed), 0));
            if(_transform.rotation.eulerAngles.y == 0)
            {
                currentState = State.Wait;
                _animator.SetBool(hashWalk, false);
            }
        }
        else
        {
            _rigidbody.MoveRotation(Quaternion.Euler
                        (0, Mathf.Lerp(0, angleRotate, Time.deltaTime * beaverSettings.RotationSpeed), 0));

            nextMove = _transform.forward * beaverSettings.WalkSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + nextMove);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        triggerCollider = other;
        houseChocolate = other.GetComponent<HouseChocolate>();
        houseChocolate.OnChocolateSteal += OnChocolateSteal;
        houseBeaverDetector = other.GetComponent<HouseBeaverDetector>();
        houseBeaverDetector.OnDestroyBeaver += Die;
        _animator.SetBool(hashRun, false);
        move = false;
        renderObject.SetActive(false);

        if (isSweeming)
            sweemAudioSource.Stop();
        else
            runAudioSource.Stop();
    }

    private void OnChocolateSteal(int remain)
    {
        if (remain == 0)
            return;
        stealedChocolateAmount++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up, rayRadius);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _animator.SetBool("Swimming", false);
            isSweeming = false;
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            _animator.SetBool("Swimming", true);
            isSweeming = true;
        }
    }

}
