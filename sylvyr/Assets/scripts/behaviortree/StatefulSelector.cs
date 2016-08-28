using System;
using BehaviorLib.Components;


public class StatefulSelector : IBehavior
{
	private IBehavior[] _Behaviors;

	private int _LastBehavior = 0;

	public BehaviorReturnCode ReturnCode{ get; set;}

	/// <summary>
	/// Selects among the given behavior components (stateful on running) 
	/// Performs an OR-Like behavior and will "fail-over" to each successive component until Success is reached or Failure is certain
	/// -Returns Success if a behavior component returns Success
	/// -Returns Running if a behavior component returns Running
	/// -Returns Failure if all behavior components returned Failure
	/// </summary>
	/// <param name="behaviors">one to many behavior components</param>
	public StatefulSelector(params IBehavior[] behaviors){
		this._Behaviors = behaviors;
	}

	/// <summary>
	/// performs the given behavior
	/// </summary>
	/// <returns>the behaviors return code</returns>
	public BehaviorReturnCode Behave(Entity entity){

		for(; _LastBehavior < _Behaviors.Length; _LastBehavior++){
			try{
				switch (_Behaviors[_LastBehavior].Behave(entity)){
				case BehaviorReturnCode.Failure:
					continue;
				case BehaviorReturnCode.Success:
					_LastBehavior = 0;
					ReturnCode = BehaviorReturnCode.Success;
					return ReturnCode;
				case BehaviorReturnCode.Running:
					ReturnCode = BehaviorReturnCode.Running;
					return ReturnCode;
				default:
					continue;
				}
			}
			catch (Exception e){
#if DEBUG
				Console.Error.WriteLine(e.ToString());
#endif
				continue;
			}
		}

		_LastBehavior = 0;
		ReturnCode = BehaviorReturnCode.Failure;
		return ReturnCode;
	}
}
