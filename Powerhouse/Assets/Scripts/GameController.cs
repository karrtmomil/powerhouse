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

	//a boolean value which determines if the game is over
	public bool GameOver
	{
		get;
		private set;
	}
	
	//the total number of enemies currently in the scene
	public int TotalNumberOfEnemies
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

	//the current forward velocity of the ship
	private float velocity;

	//a random number generator, nothing to see here
	private System.Random randy;

	//a dictionary storing our room statuses
	private Dictionary<ShipRoom, float> roomHealth;

	//the GameObject which represents the enemies in the game, Unity will initialize
	public GameObject enemyGameObject;

	//an array of GameObjects, used as spawn points, that Unity will initialize
	public GameObject[] spawnPoints;



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
		randy = new System.Random();
		velocity = 0f;
	}



	/**
	 * updates the velocity of the ship based on the status of the varying rooms
	 * @param delta - the amount of time passed since the last update, in milliseconds
	 */
	public void UpdateShipVelocity( float delta )
	{
		//the potential value determines the possible amount of change in velocity during this update
		float potential = roomHealth[ ShipRoom.ENGINE ] * roomHealth[ ShipRoom.POWER ];

		//determine the potential velocity based on the status of three rooms: Power, Engine, and Control
		float vPotential = roomHealth[ ShipRoom.CONTROL ] * potential;

		//the amount of change possible depends on the amount of time passed and the power and engines being provided
		float dPotential = ( delta * potential ) / 100;

		velocity = ( dPotential * velocity ) + ( vPotential * velocity );
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
		roomHealth.Add( room, health );
	}


	/**
	 * spawns an enemy at a random spawning point
	 */
	public void SpawnEnemy()
	{
		//select a random spawn point
		int size = spawnPoints.Length;
		int selected = randy.Next( size );

		//we want to place the new enemy at the same position as the spawning point GameObject
		Vector3 translation = spawnPoints[ selected ].transform.position;

		//TODO change the Quaternion below to be initialized dynamically based on spawn location
		Quaternion rotation = Quaternion.identity;

		//create the new enemy GameObject and add one to the total enemies
		GameObject.Instantiate( enemyGameObject, translation, rotation );
		TotalNumberOfEnemies++;
	}
}
