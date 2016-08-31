using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Selector : IBehavior
{

	protected IBehavior[] _Behaviors;

	public BehaviorReturnCode ReturnCode{ get; set;}

    /// <summary>
    /// Selects among the given behavior components
    /// Performs an OR-Like behavior and will "fail-over" to each successive component until Success is reached or Failure is certain
    /// -Returns Success if a behavior component returns Success
    /// -Returns Running if a behavior component returns Running
    /// -Returns Failure if all behavior components returned Failure
    /// </summary>
    /// <param name="behaviors">one to many behavior components</param>
	public Selector(params IBehavior[] behaviors)
    {
        _Behaviors = behaviors;
    }

    /// <summary>
    /// performs the given behavior
    /// </summary>
    /// <returns>the behaviors return code</returns>
	public BehaviorReturnCode Behave(Entity entity)
    {
        
        for (int i = 0; i < _Behaviors.Length; i++)
        {
            try
            {
                switch (_Behaviors[i].Behave(entity))
                {
                    case BehaviorReturnCode.Failure:
                        continue;
                    case BehaviorReturnCode.Success:
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    case BehaviorReturnCode.Running:
                        ReturnCode = BehaviorReturnCode.Running;
                        return ReturnCode;
                    default:
                        continue;
                }
            }
            catch (Exception e)
            {
				Debug.Log ("oopsie..." + e.ToString());

                continue;
            }
        }

        ReturnCode = BehaviorReturnCode.Failure;
        return ReturnCode;
    }
}

