using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public delegate int index_func();

public class IndexSelector : IBehavior
{

	private IBehavior[] _Behaviors;

	private index_func _index;

	public BehaviorReturnCode ReturnCode{ get; set;}

    /// <summary>
    /// The selector for the root node of the behavior tree
    /// </summary>
    /// <param name="index">an index representing which of the behavior branches to perform</param>
    /// <param name="behaviors">the behavior branches to be selected from</param>
	public IndexSelector(index_func index, params IBehavior[] behaviors)
    {
        _index = index;
        _Behaviors = behaviors;
    }

    /// <summary>
    /// performs the given behavior
    /// </summary>
    /// <returns>the behaviors return code</returns>
	public BehaviorReturnCode Behave(Entity entity)
    {
        try
        {
			switch (_Behaviors[_index()].Behave(entity))
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
                    ReturnCode = BehaviorReturnCode.Running;
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

