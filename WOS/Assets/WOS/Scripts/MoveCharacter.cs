using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MoveCharacter : MonoBehaviour 
{
	public float speed;
	[HideInInspector] public CharacterController controller;
	private Vector3 position;

	// Use this for initialization
	void Start () 
	{
		position = transform.position;
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if player is not dead move the character
		if (!PlayerStats.isDead && !IngameMenu.isPaused) 
		{
			if (!GetComponent<Animation> ().IsPlaying ("attack") && !PlayerStats.attacking) 
			{
				if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject()) 
				{
					//locate mouse position
					locatePosition ();
				}
				//move the player
				moveToPosition ();
			} 
			else if(GetComponent<Animation> () ["attack"].normalizedTime >= 0.9) 
			{
				//if animation is ending, reset position to transform
				GetComponent<PlayerStats>().inCombat = false;
				position = transform.position;
			} 
			else if(PlayerStats.attacking)
			{
				position = transform.position;
			}
		} 
		else 
		{
			//if player is dead and the game is not paused, reset transform position
			if(!IngameMenu.isPaused) 
			{
				position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
			}
		}
	}

	void locatePosition() 
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, 1000)) 
		{
			if(hit.collider.tag != "Player" && hit.collider.tag != "Enemy") 
			{
				GetComponent<PlayerStats>().inCombat = false;
				position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
			} 
			else if(hit.collider.tag == "Enemy") 
			{
				//if clicked on enemy, check range and move towards if not in range
				GetComponent<PlayerStats>().inCombat = true;
				if(GetComponent<PlayerStats>().distanceToEnemy > GetComponent<PlayerStats>().stop) 
				{
					position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
				}
			}
		}
	}

	void moveToPosition() 
	{
		if (Vector3.Distance (transform.position, position) > 1) 
		{
			Quaternion newRotation = Quaternion.LookRotation (position - transform.position);

			newRotation.x = 0f;
			newRotation.z = 0f;

			transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * 10);
			controller.SimpleMove (transform.forward * speed);
			GetComponent<Animation> ().CrossFade ("run");
		}
		else 
		{
			GetComponent<Animation>().CrossFade	("idle");
		}
	}
}

