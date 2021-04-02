using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class FSM
{
    internal State CurrentState { get; private set; }
    internal State PreviousState { get; private set; }
    private Type currentStateType;
    internal void SetStartState(State startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
        currentStateType = CurrentState.GetType();
    }
    internal void ChangeState(State newState)
    {
        if (newState == CurrentState)
            return;

        if (CurrentState != null)
        {
            PreviousState = CurrentState;
            CurrentState.Exit();
        }

        CurrentState = newState;
        CurrentState.Enter();
        currentStateType = CurrentState.GetType();
    }

    
    private Dictionary<Type, List<Func<bool>>> transitions = new Dictionary<Type, List<Func<bool>>>();

    private Type stateType;

    internal void AddTransition(State from,State to, Func<bool> transitionTo)
    {
        stateType = from.GetType();
        if (!this.transitions.TryGetValue(stateType, out var transitions)) 
        {
            transitions = new List<Func<bool>>();
            this.transitions.Add(stateType, transitions);
        }
        transitions.Add(transitionTo);
        stateType = to.GetType();
        if (!this.transitions.ContainsKey(stateType))
            this.transitions.Add(stateType, new List<Func<bool>>());
    }
    private int iterationNum;
    internal void TransitionsUpdate()
    {
        for(iterationNum = 0; iterationNum < transitions[currentStateType].Count; iterationNum++)
        {
            if (transitions[currentStateType][iterationNum].Invoke())
                return;
        }
    }
}

