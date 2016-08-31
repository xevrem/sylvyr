using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLib.Components.Decorators
{
	public class Counter : IBehavior
    {
		private int _max_count;
		private int _counter = 0;

		private IBehavior _behavior;

		public BehaviorReturnCode ReturnCode{ get; set;} 

        /// <summary>
        /// executes the behavior based on a counter
        /// -each time Counter is called the counter increments by 1
        /// -Counter executes the behavior when it reaches the supplied maxCount
        /// </summary>
        /// <param name="maxCount">max number to count to</param>
        /// <param name="behavior">behavior to run</param>
		public Counter(int maxCount, IBehavior behavior)
        {
            _max_count = maxCount;
            _behavior = behavior;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
		public  BehaviorReturnCode Behave(Entity entity)
        {
            try
            {
                if (_counter < _max_count)
                {
                    _counter++;
                    ReturnCode = BehaviorReturnCode.Running;
                    return BehaviorReturnCode.Running;
                }
                else
                {
                    _counter = 0;
                    ReturnCode = _behavior.Behave(entity);
                    return ReturnCode;
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
}
