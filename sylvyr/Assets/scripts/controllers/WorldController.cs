using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public static WorldController instance{ get; private set; }

	private ECSInstance ecs_instance;

	private EntitySystem sprite_system;
	private EntitySystem control_system;
	private EntitySystem follower_system;

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
		sprite_system = ecs_instance.system_manager.set_system(new SpriteSystem(), new SpriteData(), new GOData());
		control_system = ecs_instance.system_manager.set_system (new ControlSystem (), new Controllable(), new GOData (), new Heading());
		follower_system = ecs_instance.system_manager.set_system (new FollowSystem (), new Follower (), new GOData (), new Heading ());

		//initialize all systems
		ecs_instance.system_manager.initialize_systems();

	}

	void Start(){
		//do any final setup here
		//=======================

		//create any inital entities here
		Entity player = world.create_ship();
		Entity ship1 = world.create_follower (new Vector3(5f,5f), player);
		Entity ship2 = world.create_follower (new Vector3(-5f,-5f), ship1);
		world.create_follower (new Vector3(5f,-5f), ship2);

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
		control_system.process();
		follower_system.process ();

		sprite_system.process ();
	}


}
