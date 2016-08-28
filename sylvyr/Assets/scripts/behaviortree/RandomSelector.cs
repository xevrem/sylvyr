using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RandomSelector : IBehavior
{

	private IBehavior[] _Behaviors;

    //use current milliseconds to set random seed
    private Random _Random = new Random(DateTime.Now.Millisecond);

	public BehaviorReturnCode ReturnCode{ get; set;}

    /// <summary>
    /// Randomly selects and performs one of the passed behaviors
    /// -Returns Success if selected behavior returns Success
    /// -Returns Failure if selected behavior returns Failure
    /// -Returns Running if selected behavior returns Running
    /// </summary>
    /// <param name="behaviors">one to many behavior components</param>
	public RandomSelector(params IBehavior[] behaviors) 
    {
        _Behaviors = behaviors;
    }

    /// <summary>
    /// performs the given behavior
    /// </summary>
    /// <returns>the behaviors return code</returns>
	public BehaviorReturnCode Behave(Entity entity)
    {
        _Random = new Random(DateTime.Now.Millisecond);

        try
        {
            switch (_Behaviors[_Random.Next(0, _Behaviors.Length)].Behave(entity))
            {
                case BehaviorReturnCode.Failure:
                    ReturnCode = BehaviorReturnCode.Failure;
                    return ReturnCode;
                case BehaviorReturnCode.Success:
                    ReturnCode = BehaviorReturnCode.Success;
                    return ReturnCode;
                case BehaviorReturnCode.Running:
                    ReturnCode = BehaviorReturnCode.Running;
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
