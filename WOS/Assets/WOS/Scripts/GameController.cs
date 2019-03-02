using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public GameObject[] playerObjects;
	// Use this for initialization
	void Start ()
	{
		if (GameObject.Find ("SaveLoad") != null) 
		{
			if (GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().selectedClass == "Mage") 
			{
				Instantiate (playerObjects [0], GameObject.FindGameObjectWithTag ("Respawn").transform.position, Quaternion.identity);
			} 
			else if (GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().selectedClass == "class2") 
			{
				Instantiate (playerObjects [1], GameObject.FindGameObjectWithTag ("Respawn").transform.position, Quaternion.identity);
			} 
			else if (GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().selectedClass == "class3") 
			{
				Instantiate (playerObjects [2], GameObject.FindGameObjectWithTag ("Respawn").transform.position, Quaternion.identity);
			}
		}
	}
}
