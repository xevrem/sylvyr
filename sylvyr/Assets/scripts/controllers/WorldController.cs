using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public static WorldController instance{ get; private set; }

	public QuadTree<Entity> quad_tree;

	private ECSInstance ecs_instance;

	private EntitySystem sprite_system;
	private EntitySystem control_system;
	private EntitySystem behavior_system;
	private EntitySystem damage_system;
	private EntitySystem life_system;
	private EntitySystem projectile_system;



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


		quad_tree = new QuadTree<Entity> (new Vector2 (-10, 10), new Vector2 (10, -10));
		quad_tree.build_quad_tree (6);


		//set appropriate ECSInstance references
		EntityFactory.ecs_instance = ecs_instance;
		SimpleBehaviors.ecs_instance = ecs_instance;
		UtilFactory.ecs_instance = ecs_instance;

		ResourcePool.load_all ();

		//setup all systems
		sprite_system = ecs_instance.system_manager.set_system(new SpriteSystem(), new SpriteData(), new GOData());
		control_system = ecs_instance.system_manager.set_system (new ControlSystem (), new Controllable(), new GOData (), new Heading());
		behavior_system = ecs_instance.system_manager.set_system (new BehaviorSystem (), new Behavior ());
		damage_system = ecs_instance.system_manager.set_system (new DamageSystem (), new Damage ());
		life_system = ecs_instance.system_manager.set_system (new LifeSystem (), new Health ());
		projectile_system = ecs_instance.system_manager.set_system (new ProjectileSystem (), new Projectile ());

		//register components not explicitly tied to systems
		ecs_instance.component_manager.register_component_type (new Target ());
		ecs_instance.component_manager.register_component_type (new Quadrent ());

		//initialize all systems
		ecs_instance.system_manager.initialize_systems();

	}

	void Start(){
		//do any final setup here
		//=======================

		//create any inital entities here
		Entity player = EntityFactory.create_player_ship(Vector3.zero);
		for (int i = 0; i < 100; i++) {
			EntityFactory.create_follower (new Vector3(Random.Range(-5f,5f),Random.Range(-5f,5f)), player);
		}


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
		behavior_system.process ();
		damage_system.process ();
		life_system.process ();
		projectile_system.process ();

		sprite_system.process ();
	}


}
