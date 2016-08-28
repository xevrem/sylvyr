using System;
using System.Collections.Generic;
using System.Linq;


public enum BehaviorReturnCode
{
    Failure,
    Success,
    Running
}

public delegate BehaviorReturnCode behavior_return(Entity entity);


