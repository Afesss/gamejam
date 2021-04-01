using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaverMoveState : BeaverIdleState
{
    protected Rigidbody rigidbody;
    protected Transform transform;
    internal BeaverMoveState(BeaverStateHandler beaver,Rigidbody rigidbody,Transform transform) : base(beaver)
    {
        this.rigidbody = rigidbody;
        this.transform = transform;
    }

    internal override void Enter()
    {
        Debug.Log("Move");
    }

    internal override void Exit()
    {
    }

    internal override void PhysicsUpdate()
    {
    }

    internal override void Update()
    {
    }
}
