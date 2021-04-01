using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaverIdleState : State
{
    protected BeaverStateHandler beaver;
    internal BeaverIdleState(BeaverStateHandler beaver)
    {
        this.beaver = beaver;
    }
}
