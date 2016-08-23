using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public static WorldController instance{ get; private set; }

	private ECSInstance ecs_instance;

	private EntitySystem sprite_system;
	private EntitySystem move_stuff_system;

	private World world;

	// Use this for initialization
	void OnEnable () {
		//make sure we're the only one
		if (instance != null) {
			Debug.LogError ("dubplicate world detected... destroying...");
			Destroy (this);
		}
		instance = this;

		//do all initialization here:
		//==========================

		ecs_instance = new ECSInstance();

		world = new World (ecs_instance);

		ResourcePool.load_all ();

		//setup all systems
		sprite_system = ecs_instance.system_manager.set_system(new SpriteSystem(), new SpriteData(), new Position());
		move_stuff_system = ecs_instance.system_manager.set_system (new MoveStuffSystem (), new Position ());

		//initialize all systems
		ecs_instance.system_manager.initialize_systems();

	}

	void Start(){
		//do any final setup here
		//=======================

		//create any inital entities here
		world.create_ship();

		//early entity reslove
		ecs_instance.resolve_entities();

		//load system content
		ecs_instance.system_manager.systems_load_content();
	}
	
	// Update is called once per frame
	void Update () {
		
		//update delta time
		ecs_instance.delta_time = Time.deltaTime;

		//resolve any entity updates as needed
		ecs_instance.resolve_entities();

		//do system processing
		move_stuff_system.process();


	}
}
