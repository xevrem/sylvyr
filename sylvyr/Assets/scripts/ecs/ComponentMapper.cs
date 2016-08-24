//
//  ComponentMapper.cs
//
//  Author:
//       erika <>
//
//  Copyright (c) 2016 erika
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Reflection;

public class ComponentMapper
{
	private static ECSInstance ecs_instance;

	private int _type_id;


	public ComponentMapper ()
	{
	}



	public ComponentMapper(IComponent component, ECSInstance instance){
		this._type_id = component.type_id;
		if (ecs_instance == null)
			ComponentMapper.ecs_instance = instance;
	}

	public T get<T>(int entity_id) where T: IComponent{
		return (T) ecs_instance.component_manager.components[this._type_id][entity_id];
	}

	public T get<T>(Entity entity) where T: IComponent{		
		return (T) ecs_instance.component_manager.components[this._type_id][entity.id];
	}

	public static T get_simple<T>(Entity entitiy) where T: new() {
		IComponent t = (IComponent) new T();
		return (T) ecs_instance.component_manager.components[t.type_id][entitiy.id];
	}
}


