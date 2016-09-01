using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public delegate float elapsed_time_func();

public class Timer : IBehavior
{

	private elapsed_time_func _ElapsedTimeFunction;

    private IBehavior _Behavior;

    private float _TimeElapsed = 0;

    private float _WaitTime;

	public BehaviorReturnCode ReturnCode{ get; set;}

    /// <summary>
    /// executes the behavior after a given amount of time in miliseconds has passed
    /// </summary>
    /// <param name="elapsedTimeFunction">function that returns elapsed time</param>
    /// <param name="timeToWait">maximum time to wait before executing behavior</param>
    /// <param name="behavior">behavior to run</param>
	public Timer(elapsed_time_func elapsedTimeFunction, float timeToWait, IBehavior behavior)
    {
        _ElapsedTimeFunction = elapsedTimeFunction;
        _Behavior = behavior;
        _WaitTime = timeToWait;
    }

    /// <summary>
    /// performs the given behavior
    /// </summary>
    /// <returns>the behaviors return code</returns>
	public BehaviorReturnCode Behave(Entity entity)
    {
        try
        {
			_TimeElapsed += _ElapsedTimeFunction();

            if (_TimeElapsed >= _WaitTime)
            {
                _TimeElapsed = 0;
                ReturnCode = _Behavior.Behave(entity);
                return ReturnCode;
            }
            else
            {
                ReturnCode = BehaviorReturnCode.Running;
                return BehaviorReturnCode.Running;
            }
        }
        catch (Exception e)
        {
			Debug.Log ("oopsie..." + e.ToString());

            ReturnCode = BehaviorReturnCode.Failure;
            return BehaviorReturnCode.Failure;
        }
    }
}

