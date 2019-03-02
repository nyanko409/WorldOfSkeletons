using UnityEngine;
using System.Collections;

public class SoundEffects : MonoBehaviour 
{
	public static AudioSource[] audios;
	// Use this for initialization
	void Start () 
	{
		audios = GetComponents<AudioSource>();
	}

	// 0 = potion used
	// 1 = skeleton attack
	// 2 = inventory full
	// 3 = mage_dead 1
	// 4 = mage_no mana
	// 5 = skeleton dead
	// 6 = dont need to do that mage
	// 7 = i cant do that mage
	// 8 = grab item
}
