using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public delegate float rand_func();

public class RandomDecorator : IBehavior
{

    private float _Probability;

	private rand_func _RandomFunction;

    private IBehavior _Behavior;

	public BehaviorReturnCode ReturnCode{ get; set;}

    /// <summary>
    /// randomly executes the behavior
    /// </summary>
    /// <param name="probability">probability of execution</param>
    /// <param name="randomFunction">function that determines probability to execute</param>
    /// <param name="behavior">behavior to execute</param>
	public RandomDecorator(float probability, rand_func randomFunction, IBehavior behavior)
    {
        _Probability = probability;
        _RandomFunction = randomFunction;
        _Behavior = behavior;
    }


	public BehaviorReturnCode Behave(Entity entity)
    {
        try
        {
            if (_RandomFunction() <= _Probability)
            {
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

