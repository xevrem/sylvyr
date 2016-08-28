using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate bool bool_func(Entity entity);

public class Conditional : IBehavior
{

    private bool_func _bool;

	public BehaviorReturnCode ReturnCode{ get; set;}

    /// <summary>
    /// Returns a return code equivalent to the test 
    /// -Returns Success if true
    /// -Returns Failure if false
    /// </summary>
    /// <param name="test">the value to be tested</param>
    public Conditional(bool_func test)
    {
        _bool = test;
    }

    /// <summary>
    /// performs the given behavior
    /// </summary>
    /// <returns>the behaviors return code</returns>
	public BehaviorReturnCode Behave(Entity entity)
    {

        try
        {
			switch (_bool(entity))
            {
                case true:
                    ReturnCode = BehaviorReturnCode.Success;
                    return ReturnCode;
                case false:
                    ReturnCode = BehaviorReturnCode.Failure;
                    return ReturnCode;
                default:
                    ReturnCode = BehaviorReturnCode.Failure;
                    return ReturnCode;
            }
        }
        catch (Exception e)
        {
#if DEBUG
            Console.Error.WriteLine(e.ToString());
#endif
            ReturnCode = BehaviorReturnCode.Failure;
            return ReturnCode;
        }
    }
}

