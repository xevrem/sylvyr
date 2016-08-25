using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLib.Components
{
    public abstract class  BehaviorComponent
    {
        protected BehaviorReturnCode ReturnCode;

        public BehaviorComponent() { }

        public abstract BehaviorReturnCode Behave();
    }
}
