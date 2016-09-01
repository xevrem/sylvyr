//
//  GroupManager.cs
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
using System.Collections.Generic;

public class GroupManager
{
	public GroupManager ()
	{
	}

	private Dictionary<string, List<Entity>> _groups = new Dictionary<string, List<Entity>>();

	public void add_entity_to_group(string group, Entity e){
		if (_groups.ContainsKey (group) == false) {
			_groups.Add(group, new List<Entity>());
		}

		if (_groups [group].Contains (e) == false) {
			_groups [group].Add (e);
		}
	}

	public List<Entity> get_group(string group){
		//TODO

		if (_groups.ContainsKey (group) == true)
			return _groups [group];
		else
			return null;
			
	}

	public void refresh(Entity e){
		//TODO
	}

	public void delete_entity(Entity e){
		foreach (string key in _groups.Keys) {
			_groups [key].Remove (e);
		}
	}

	public void clean_up(){
		//TODO
	}
}


