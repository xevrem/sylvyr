using System;
using BehaviorLib;
using BehaviorLib.Components;
using BehaviorLib.Components.Composites;
using BehaviorLib.Components.Actions;
using BehaviorLib.Components.Conditionals;
using BehaviorLib.Components.Decorators;
using BehaviorLib.Components.Utility;

namespace BehaviorLib.Components.Decorators
{
    public class RepeatUntilFail : BehaviorComponent
    {
        private BehaviorComponent _Behavior;
        
        /// <summary>
        /// executes the behavior every time again
        /// </summary>
        /// <param name="timeToWait">maximum time to wait before executing behavior</param>
        /// <param name="behavior">behavior to run</param>
        public RepeatUntilFail(BehaviorComponent behavior)
        {
            _Behavior = behavior;
        }
        
        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            ReturnCode = _Behavior.Behave();
            if (ReturnCode == BehaviorReturnCode.Failure) {
                return BehaviorReturnCode.Failure;
            } else {
                ReturnCode = BehaviorReturnCode.Running;
                return BehaviorReturnCode.Running;
            }
        }
    }
}
