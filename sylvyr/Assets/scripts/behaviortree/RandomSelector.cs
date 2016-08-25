﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLib.Components.Composites
{
	public class RandomSelector : BehaviorComponent
    {

		private BehaviorComponent[] _Behaviors;

        //use current milliseconds to set random seed
        private Random _Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Randomly selects and performs one of the passed behaviors
        /// -Returns Success if selected behavior returns Success
        /// -Returns Failure if selected behavior returns Failure
        /// -Returns Running if selected behavior returns Running
        /// </summary>
        /// <param name="behaviors">one to many behavior components</param>
		public RandomSelector(params BehaviorComponent[] behaviors) 
        {
            _Behaviors = behaviors;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            _Random = new Random(DateTime.Now.Millisecond);

            try
            {
                switch (_Behaviors[_Random.Next(0, _Behaviors.Length)].Behave())
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
}
