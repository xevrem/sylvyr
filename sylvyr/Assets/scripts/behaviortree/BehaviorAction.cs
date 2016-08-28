using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


public class BehaviorAction : IBehavior
{

	private behavior_return _action;

	public BehaviorReturnCode ReturnCode{ get; set;}

    public BehaviorAction() { }

    public BehaviorAction(behavior_return action)
    {
        _action = action;
    }

	public BehaviorReturnCode Behave(Entity entity)
    {
        try
        {
			switch (_action(entity))
            {
                case BehaviorReturnCode.Success:
                    ReturnCode = BehaviorReturnCode.Success;
                    return ReturnCode;
                case BehaviorReturnCode.Failure:
                    ReturnCode = BehaviorReturnCode.Failure;
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

			Debug.Log ("oopsie..." + e.ToString());

            ReturnCode = BehaviorReturnCode.Failure;
            return ReturnCode;
        }
    }

}

