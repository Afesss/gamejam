using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaverStateHandler : MonoBehaviour
{
    internal bool attack { get; private set; }
    internal  Vector3 TargetPosition { get; private set; }

    private Rigidbody _rigidbody;
    private Transform _transform;

    private FSM fSM;

    private BeaverIdleState idleState;
    private BeaverMoveState moveState;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        fSM = new FSM();

        moveState = new BeaverMoveState(this, _rigidbody, _transform);
        idleState = new BeaverIdleState(this);

        fSM.SetStartState(idleState);

    }
    internal void ToAttackState(Vector3 target)
    {
        attack = true;
        TargetPosition = target;
        fSM.ChangeState(moveState);
    }
    private void Update()
    {
        fSM.CurrentState.Update();
    }
    private void FixedUpdate()
    {
        fSM.CurrentState.PhysicsUpdate();
    }
}
