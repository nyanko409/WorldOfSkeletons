using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireball : MonoBehaviour 
{
	public GameObject spell;

	[HideInInspector] public float damage;
	[HideInInspector] public float manaCost;
	public float speed;
	public float cooldown;

	private float remainingCooldown;
	private bool isInCooldown;

	[HideInInspector] public KeyCode key;

	void Update () 
	{
		cooldownCounter();

		if (!PlayerStats.isDead) 
		{
			attackCooldown ();
			Plane playerPlane = new Plane (Vector3.up, transform.position);
		
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			float hitdist = 0.0f;
			// If the ray is parallel to the plane, Raycast will return false.
			if (playerPlane.Raycast (ray, out hitdist)) 
			{
				// Get the point along the ray that hits the calculated distance.
				Vector3 targetPoint = ray.GetPoint (hitdist);
			
				// Determine the target rotation.  This is the rotation if the transform looks at the target point.
				Quaternion targetRotation = Quaternion.LookRotation (targetPoint - transform.position);

				if (Input.GetKeyDown (key) && !isInCooldown && !GetComponent<Animation> ().IsPlaying ("attack") && GetComponent<PlayerStats>().checkMana(manaCost))
				{
					// rotate towards the target point.
					GetComponent<Animation> ().CrossFade ("attack");
					if (!PlayerStats.attacking) 
					{
						isInCooldown = true;
						remainingCooldown = cooldown; // attacking, set the cooldown
						PlayerStats.attacking = true;
						transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, 100 * Time.deltaTime);
						GameObject obj = (GameObject)Instantiate (spell, transform.position, transform.rotation);
						//set shooter tag
						if (gameObject.tag == "Player") 
						{
							obj.tag = "PlayerSpell";
						} 
						else if (gameObject.tag == "Enemy") 
						{
							obj.tag = "EnemySpell";
						}
						//save damage variable to the spell
						obj.SendMessage ("setDamage", damage);
						obj.GetComponent<Rigidbody> ().velocity = transform.TransformDirection (new Vector3 (0, 0, speed));
						Destroy (obj, 5f);
						PlayerStats.attacking = true;
					}
				}
			}
		}
	}

	void attackCooldown()
	{
		if (!GetComponent<Animation> ().IsPlaying ("attack"))
		{
			PlayerStats.attacking = false;
		}
	}

	void cooldownCounter()
	{
		if(isInCooldown)
		{
			remainingCooldown -= Time.deltaTime;
			if(remainingCooldown <= 0)
			{
				isInCooldown = false;
				remainingCooldown = 0;
			}
		}
	}

}
