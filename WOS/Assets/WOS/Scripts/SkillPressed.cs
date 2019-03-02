using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillPressed : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		GetComponent<Button>().onClick.AddListener (delegate {Clicked();});
	}

	public void Clicked() 
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<ActionBar> ().SendMessage ("skillPressed", name);
	}
}
