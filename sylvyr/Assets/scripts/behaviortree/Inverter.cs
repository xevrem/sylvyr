using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Inverter : IBehavior
{

    private IBehavior _Behavior;

	public BehaviorReturnCode ReturnCode{ get; set;}

    /// <summary>
    /// inverts the given behavior
    /// -Returns Success on Failure or Error
    /// -Returns Failure on Success 
    /// -Returns Running on Running
    /// </summary>
    /// <param name="behavior"></param>
    public Inverter(IBehavior behavior) 
    {
        _Behavior = behavior;
    }

    /// <summary>
    /// performs the given behavior
    /// </summary>
    /// <returns>the behaviors return code</returns>
	public BehaviorReturnCode Behave(Entity entity)
    {
        try
        {
            switch (_Behavior.Behave(entity))
            {
                case BehaviorReturnCode.Failure:
                    ReturnCode = BehaviorReturnCode.Success;
                    return ReturnCode;
                case BehaviorReturnCode.Success:
                    ReturnCode = BehaviorReturnCode.Failure;
                    return ReturnCode;
                case BehaviorReturnCode.Running:
                    ReturnCode = BehaviorReturnCode.Running;
                    return ReturnCode;
            }
        }
        catch (Exception e)
        {
#if DEBUG
            Console.Error.WriteLine(e.ToString());
#endif
            ReturnCode = BehaviorReturnCode.Success;
            return ReturnCode;
        }

        ReturnCode = BehaviorReturnCode.Success;
        return ReturnCode;

    }

}
