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

    //Represents the progress to the destination
    public float Progress
    {
        get;
        private set;
    }

    //Represents the health of the ship
    public float ShipHealth
    {
        get;
        private set;
    }

    //the time, in milliseconds, which will correspond to one unit of movement
    private const int UNIT_OF_MOVEMENT_TIMEFRAME = 200;

    //the time, in milliseconds, between potential 1% progress gains
    private const int UNIT_OF_PROGRESS_UPDATE_TIMEFRAME = 2;

    //the time, in milliseconds, between potential 1% heading changes
    private const int UNIT_OF_HEADING_CHANGE = 1;

    //the time, in seconds, between boat spawns when enemies are not present on the ship
    private const int SPAWN_RATE_WHEN_UNOCCUPIED = 7;

    //the time, in seconds, between boat spawns when enemies are present on the ship
    private const int SPAWN_RATE_WHEN_OCCUPIED = 12;

    //the current forward velocity of the ship
    private float velocity;

    //the current heading of the ship, represented as a float. 0 = backwards, 1 = directly towards the goal ( 0, 1 ) = percentage of heading
    private float heading;
	
	//the time of the last spawned enemy
	private float lastEnemySpawned;

    //the number of enemies currently on the ship
    private int numberOfEnemiesOnShip;

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

    //a dictionary which represents the occupied state of each room
    private Dictionary<ShipRoom, bool> roomStatus;

    // Lets us know if user is in the turrent to switch some UI information
    public bool inTurret;

    // Objects of current active enemies
    public List<GameObject> activeEnemies;

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
        inTurret = false;

		roomHealth = new Dictionary<ShipRoom, float>();
		foreach( ShipRoom room in (ShipRoom[]) Enum.GetValues( typeof( ShipRoom ) ) )
		{
			roomHealth.Add( room, 1f );
		}

        //we start with 0 velocity but are pointed directly towards the goal
        velocity = 0f;
        heading = 1f;

        //we have 100% of ships health, but 0% progress
        ShipHealth = 1f;
        Progress = 0f;

        //initialize the status of each room
        roomStatus = new Dictionary<ShipRoom, bool>();
        foreach (ShipRoom room in (ShipRoom[])Enum.GetValues(typeof(ShipRoom)))
        {
            roomStatus.Add(room, false);
        }


		randy = new System.Random();
		lastEnemySpawned = -1f;
        numberOfEnemiesOnShip = 0;
		UpdateRoomHealth( ShipRoom.ENGINE, .5f );
        activeEnemies = new List<GameObject>();
	}



	public void Update()
	{
        float dT = Time.deltaTime;
        float time = Time.time;

        //update the properties of the ship
		UpdateShipVelocity( dT );
        UpdateGameProgress( dT );

        //determine if it's time to spawn an enemy
        int spawnRate = numberOfEnemiesOnShip > 0 ? SPAWN_RATE_WHEN_OCCUPIED : SPAWN_RATE_WHEN_UNOCCUPIED;
        if (((int)Time.time) % spawnRate == 0 && lastEnemySpawned < Math.Floor(time))
		{
            lastEnemySpawned = time;
			SpawnBoat();
		}
	}



    /**
     * updates the velocity of the ship based on the status of the varying rooms
     * @param delta - the amount of time passed since the last update, in milliseconds
     */
    public void UpdateShipVelocity(float delta)
    {
        //determine the potential velocity based on the status of three rooms: Power, Engine, and Control
        //float vPotential = roomHealth[ ShipRoom.CONTROL ] * roomHealth[ ShipRoom.ENGINE ] * roomHealth[ ShipRoom.POWER ];
        float vPotential = (roomStatus[ShipRoom.ENGINE] || roomStatus[ShipRoom.POWER]) ? -1f : 1f;

        //the amount of change possible depends on the amount of time passed
        float dPotential = delta / UNIT_OF_MOVEMENT_TIMEFRAME;

        //the actual amount of change possible
        float potential = dPotential * vPotential;

        //if( potential == 0f )
        //{
        //    potential = velocity - dPotential;
        //}
        //else
        //{
        //    potential = ( velocity < vPotential ? velocity + potential : velocity - potential );
        //}

        velocity = Mathf.Max(Mathf.Min(velocity + potential, 1f), 0f);
    }



    /**
     * updates the velocity of the ship based on the status of the varying rooms
     * @param delta - the amount of time passed since the last update, in milliseconds
     */
    public void UpdateShipHeading(float delta)
    {
        //determine the potential velocity based on the status of three rooms: Power, Engine, and Control
        float hPotential = (roomStatus[ShipRoom.ENGINE] || roomStatus[ShipRoom.POWER]) ? -1f : 1f;

        //the amount of change possible depends on the amount of time passed
        float dHeading = delta / UNIT_OF_HEADING_CHANGE;

        //the actual amount of change possible
        float potential = hPotential * dHeading;

        heading = Mathf.Max(Mathf.Min(heading + potential, 1f), 0f);
    }



    /**
     * updates progress towards the goal based on the time passed since the last update
     */
    public void UpdateGameProgress(float delta)
    {
        float dPotential = delta / UNIT_OF_PROGRESS_UPDATE_TIMEFRAME;
        float potential = heading * velocity * dPotential;
        Progress += ( potential * 0.01f );
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
     * changes the state of a room, given the room and the status to modify
     */
    public void SetRoomStatus(ShipRoom room, bool occupied)
    {
        roomStatus[room] = occupied;
    }


	/**
	 * spawns an enemy boat at a random water-based spawning point
	 */
	public void SpawnBoat()
	{
        InstantiateAtRandomSpawnPoint( boatGameObject, boatSpawnPoints );
	}



    /**
     * spawns a single enemy into a random room
     */
    public void SpawnEnemy()
    {
        InstantiateAtRandomSpawnPoint( enemyGameObject, roomSpawnPoints );
        ++numberOfEnemiesOnShip;
    }


    /**
     * invoked when a rowboat hits the ship
     */
    public void onBoatCollision( GameObject gameObject )
    {
        int numberOfEnemies = gameObject.transform.childCount - 1;

        GameObject.Destroy( gameObject );

        //each enemy on the boat gives a chance of spawning an enemy onto the boat
        for ( int i = 0; i < numberOfEnemies; ++i )
        {
            double r = randy.NextDouble();
            
            //25% chance to spawn an enemy on the boat
            if ( r >= 0.75 )
            {
                SpawnEnemy();
                break;
            }
        }
    }


    /**
     * invoked when an enemy on the ship is killed
     */
    public void onEnemyKilled( GameObject gameObject )
    {
        activeEnemies.Remove(gameObject);
        GameObject.Destroy( gameObject );
        --numberOfEnemiesOnShip;
    }

    /**
     * duplicate code is a bad thing.
     */
    private void InstantiateAtRandomSpawnPoint( GameObject gameObject, GameObject[] spawnPoints )
    {
        if ( gameObject == null || spawnPoints == null ) return;

        //select a random room
        int size = spawnPoints.Length;
        int selected = randy.Next( size );

        //set the position equal to the spawn point
        Vector3 translation = spawnPoints[selected].transform.position;

        //set the rotation equal to the spawning point's rotation
        Quaternion rotation = spawnPoints[selected].transform.rotation;

        //create the new enemy GameObject
        GameObject obj = (GameObject)GameObject.Instantiate(gameObject, translation, rotation);
        if(gameObject.name == "Enemy")
            activeEnemies.Add(obj);
    }
}
