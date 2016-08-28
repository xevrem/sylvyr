using System;


public class Repeater : IBehavior
{
    private IBehavior _Behavior;

	public BehaviorReturnCode ReturnCode{ get; set;}
    
    /// <summary>
    /// executes the behavior every time again
    /// </summary>
    /// <param name="timeToWait">maximum time to wait before executing behavior</param>
    /// <param name="behavior">behavior to run</param>
    public Repeater(IBehavior behavior)
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
        ReturnCode = BehaviorReturnCode.Running;
        return BehaviorReturnCode.Running;
    }
}

