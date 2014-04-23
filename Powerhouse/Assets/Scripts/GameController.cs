using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

	public enum ShipRoom
	{
		COMMUNICATIONS,
		CONTROL,
		WEAPON,
		POWER,
		ENGINE,
		STORAGE
	};

    public Camera CameraMain;
    public Camera CameraTurret1;
    public Camera CameraTurret2;
    public Camera CameraTurret3;

	//a boolean value which determines if the game is over
	public bool GameOver
	{
		get;
		private set;
	}

	//a property which controls access to the current forward velocity of the ship
	public float Velocity
	{
		get
		{
			return velocity;
		}
		set
		{
			velocity = Mathf.Max( Mathf.Min( 1f, value ), 0f );
		}
	}

	//the spawn rate, in seconds
	private const int SPAWN_RATE = 15;

	//the current forward velocity of the ship
	private float velocity;
	
	//the time of the last spawned enemy
	private float lastEnemySpawned;

	//a random number generator, nothing to see here
	private System.Random randy;

	//a dictionary storing our room statuses
	private Dictionary<ShipRoom, float> roomHealth;
	
	//the GameObject which represents the enemies in the game, Unity will initialize
	public GameObject enemyGameObject;

	//the GameObject which represents the enemies in the game, Unity will initialize
	public GameObject boatGameObject;

	//an array of GameObjects, used as spawn points, that Unity will initialize
	public GameObject[] roomSpawnPoints;
	
	//an array of GameObjects, used as spawn points, that Unity will initialize
	public GameObject[] boatSpawnPoints;



    // Creates an instance of itself
    private void Awake()
    {
        if (Instance != null)
            GameObject.Destroy(Instance);
        else
            Instance = this;
        DontDestroyOnLoad(this);
    }


	private void Start()
	{
		GameOver = false;

		roomHealth = new Dictionary<ShipRoom, float>();
		foreach( ShipRoom room in (ShipRoom[]) Enum.GetValues( typeof( ShipRoom ) ) )
		{
			roomHealth.Add( room, 1f );
		}

		randy = new System.Random();
		velocity = 0f;
		lastEnemySpawned = -1f;
		UpdateRoomHealth( ShipRoom.ENGINE, .5f );
	}



	public void Update()
	{
		UpdateShipVelocity( Time.deltaTime );
		float currentTime = Time.time;

		if( ( (int)Time.time ) % SPAWN_RATE == 0 && lastEnemySpawned < Math.Floor( currentTime ) )
		{
			lastEnemySpawned = currentTime;
			SpawnBoat();
		}
	}



	/**
	 * updates the velocity of the ship based on the status of the varying rooms
	 * @param delta - the amount of time passed since the last update, in milliseconds
	 */
	public void UpdateShipVelocity( float delta )
	{
		//determine the potential velocity based on the status of three rooms: Power, Engine, and Control
		float vPotential = roomHealth[ ShipRoom.CONTROL ] * roomHealth[ ShipRoom.ENGINE ] * roomHealth[ ShipRoom.POWER ];

		//the amount of change possible depends on the amount of time passed and the power and engines being provided
		float dPotential = delta / 200;

		//the actual amount of change possible
		float potential = dPotential * vPotential;

		if( potential == 0f )
		{
			potential = velocity - dPotential;
		}
		else
		{
			potential = ( velocity < vPotential ? velocity + potential : velocity - potential );
		}

		velocity = Mathf.Max( Mathf.Min ( potential, 1f ), 0f );
	}


	/**
	 * updates the health of the given room
	 */
	public void UpdateRoomHealth( ShipRoom room, float health )
	{
		//clamp room health [ 0, 1.0 ]
		health = Mathf.Max( Mathf.Min ( 1f, health ), 0f );

		switch( room )
		{
		case ShipRoom.COMMUNICATIONS:

			//the communications room is either disabled or enabled
			health = Mathf.Round( health );

			if( health == 0f )
			{
				// TODO DISABLE COMMUNICATIONS ROOM
			}
			else
			{
				// TODO enable communications room
			}
			
			break;
			
			
		case ShipRoom.CONTROL:

			//nothing to do here
			
			break;


		case ShipRoom.WEAPON:
			
			//nothing to do here
			
			break;
			
			
		case ShipRoom.POWER:
			
			//nothing to do here
			
			break;
			
			
		case ShipRoom.ENGINE:
			
			//nothing to do here
			
			break;
			
			
		case ShipRoom.STORAGE:

			if( health == 0f ) GameOver = true;
			
			break;
		}

		//store the new health of the given room into the dictionary
		roomHealth[ room ] = health;
	}


	/**
	 * spawns an enemy at a random spawning point
	 */
	public void SpawnBoat()
	{
		//select a random spawn point
		int size = boatSpawnPoints.Length;
		int selected = randy.Next( size );

		//we want to place the new enemy at the same position as the spawning point GameObject
		Vector3 translation = boatSpawnPoints[ selected ].transform.position;

		//TODO change the Quaternion below to be initialized dynamically based on spawn location
		Quaternion rotation = boatSpawnPoints[ selected ].transform.rotation;

		//create the new enemy GameObject and add one to the total enemies
		GameObject.Instantiate( boatGameObject, translation, rotation );
		print( "i'm on a boat!" );
	}
}
