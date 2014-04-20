using UnityEngine;
using System.Collections;



public class Ladder : MonoBehaviour {

	public Vector3 transportLocation;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if(other.gameObject.tag == "Player"){
			Debug.Log("In ladder");	
			//other.gameObject.GetComponent<CharacterController>().Move(new Vector3(0.0f, 4.0f, -2.0f));
			other.gameObject.transform.position = transportLocation;
			
		}			
	}
	
	
	void OnTriggerExit (Collider other) {
		
		if(other.gameObject.tag == "Player"){
			Debug.Log("Leaving ladder");
		}		
	}
}
