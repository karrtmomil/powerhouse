using UnityEngine;
using System.Collections;

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

	private Dictionary<ShipRoom, float> roomHealth;

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
	}


	public void UpdateRoomHealth( ShipRoom room, float health )
	{
		//clamp room health [ 0, 1.0 ]
		health = Mathf.Max( Mathf.Min ( 1f, health ), 0f );

		switch( room )
		{
		case COMMUNICATIONS:

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
			
			
		case CONTROL:

			//nothing to do here
			
			break;


		case WEAPON:
			
			//nothing to do here
			
			break;
			
			
		case POWER:
			
			//nothing to do here
			
			break;
			
			
		case ENGINE:
			
			//nothing to do here
			
			break;
			
			
		case STORAGE:

			if( health == 0f ) GameOver = true;
			
			break;
		}

		//store the new health of the given room into the dictionary
		roomHealth.Add( room, health );
	}
}
