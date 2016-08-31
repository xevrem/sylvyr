//
//  UtilitySelector.cs
//
//  Author:
//       Erika V. Jonell <@xevrem>
//
//  Copyright (c) 2014 Erika V. Jonell
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate UtilityVector utility_vector_func();

public class UtilitySelector : IBehavior
{
    private UtilityPair[] _utility_pairs;
	private utility_vector_func _utility_function;

	public BehaviorReturnCode ReturnCode{ get; set;}
    
	public UtilitySelector(utility_vector_func utility_function, params UtilityPair[] pairs)
    {
        this._utility_pairs = pairs;
		this._utility_function = utility_function;
    }

	public BehaviorReturnCode Behave(Entity entity)
    {
		try{
			UtilityVector func_vector = this._utility_function();

			float min = -2.0f;
			UtilityPair best_match = null;

			//find max pair match
			foreach(UtilityPair pair in this._utility_pairs){ 
				float val = func_vector.dot(pair.vector);
				if(val > min){
					min = val;
					best_match = pair;
				}
			}

			//make sure we found a match
			if(best_match == null){
#if DEBUG
				Console.WriteLine("best_match not defined...");
#endif
				this.ReturnCode = BehaviorReturnCode.Failure;
				return this.ReturnCode;
			}

			//execute best pair match and return result
			this.ReturnCode = best_match.behavior.Behave(entity);
			return this.ReturnCode;
		}catch(Exception e){
#if DEBUG
			Console.WriteLine(e.ToString());
#endif
			this.ReturnCode = BehaviorReturnCode.Failure;
			return BehaviorReturnCode.Failure;
		}
    }
}

