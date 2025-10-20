using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTemplateMachine : MonoBehaviour
{
    FSMTemplateState _currentState;

    void Start()
    {
        GetInitialState(out _currentState);
        if (_currentState != null)
        {
            _currentState.Enter();
        }
    }

    protected virtual void GetInitialState(out FSMTemplateState stateMachine)
    {
        stateMachine = null;
    }

    void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateLogic();
        }
    }

    void LateUpdate()
    {
        if (_currentState != null)
        {
            _currentState.UpdatePhysics();
        }
    }

    public void ChangeState(FSMTemplateState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    protected virtual void OnDisable()
    {
        _currentState.Exit();
    }
}
