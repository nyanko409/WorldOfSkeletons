using UnityEngine;
using System.Collections;

public class FireballObj : MonoBehaviour
{

	[HideInInspector] public float damage;

	void Awake() 
	{
		AudioSource.PlayClipAtPoint (GetComponent<AudioSource> ().clip, transform.position);
	}

	void setDamage(float damage)
	{
		this.damage = damage;
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Environment") 
		{
			Destroy(gameObject);
		}
		if(other.tag == "Enemy") // deal damage
		{
			other.gameObject.SendMessage("applyDamage", damage);
			Destroy(gameObject);
		}
	}
}
