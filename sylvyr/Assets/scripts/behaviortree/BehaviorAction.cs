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
					return ReturnCode = BehaviorReturnCode.Success;
                case BehaviorReturnCode.Failure:                    
					return ReturnCode = BehaviorReturnCode.Failure;
                case BehaviorReturnCode.Running:
					return ReturnCode = BehaviorReturnCode.Running;
                default:                    
					return ReturnCode = BehaviorReturnCode.Failure;
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

