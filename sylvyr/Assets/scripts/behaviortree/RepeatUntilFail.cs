using System;

public class RepeatUntilFail : IBehavior
{
    private IBehavior _Behavior;

	public BehaviorReturnCode ReturnCode{ get; set;}
    
    /// <summary>
    /// executes the behavior every time again
    /// </summary>
    /// <param name="timeToWait">maximum time to wait before executing behavior</param>
    /// <param name="behavior">behavior to run</param>
    public RepeatUntilFail(IBehavior behavior)
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
            return BehaviorReturnCode.Failure;
        } else {
            ReturnCode = BehaviorReturnCode.Running;
            return BehaviorReturnCode.Running;
        }
    }
}

