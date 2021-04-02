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
        Movement();
    }

    internal override void Update()
    {
    }
    Vector3 nextMove;
    private void Movement()
    {
        nextMove = transform.forward;
        Debug.Log(Vector3.Dot(transform.position, beaver.TargetPosition));
        if (Vector3.Dot(transform.position,beaver.TargetPosition) > 0)
        {
            nextMove = Vector3.zero;
        }
        nextMove *= BeaverData.Instance.BeaverSpeed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + nextMove);
    }
}
