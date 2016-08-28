using System;


public class Succeeder : IBehavior
{
    private IBehavior _Behavior;

	public BehaviorReturnCode ReturnCode{ get; set;}
    
    /// <summary>
    /// returns a success even when the decorated component failed
    /// </summary>
    /// <param name="behavior">behavior to run</param>
    public Succeeder(IBehavior behavior)
    {
        _Behavior = behavior;
    }
    
    /// <summary>
    /// performs the given behavior
    /// </summary>
    /// <returns>the behaviors return code</returns>
	public BehaviorReturnCode Behave(Entity entity)
    {
        ReturnCode = _Behavior.Behave(entity);
        if (ReturnCode == BehaviorReturnCode.Failure) {
            ReturnCode = BehaviorReturnCode.Success;
        }
        return ReturnCode;
    }
}

