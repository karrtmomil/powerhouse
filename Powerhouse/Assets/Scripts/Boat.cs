using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
		float dtime = Time.deltaTime;
		Vector3 forward = this.transform.forward;
		transform.position += forward * 4 * dtime;
	}


	private void OnCollisionEnter( Collision o )
	{
        GameController.Instance.onBoatCollision( this.gameObject );
	}
}
