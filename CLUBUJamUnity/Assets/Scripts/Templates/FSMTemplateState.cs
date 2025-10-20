using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTemplateState
{
    protected FSMTemplateMachine _fsm;
    public FSMTemplateState(FSMTemplateMachine fsm)
    {
        _fsm = fsm;
    }


    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
