using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IBehavior
{
	BehaviorReturnCode ReturnCode{ get; set;}

	BehaviorReturnCode Behave(Entity entity);
}

