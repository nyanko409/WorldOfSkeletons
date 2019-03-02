using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterInfo : MonoBehaviour {

	public int statPoints;

	public KeyCode keyCode;

	private GameObject characterInfoScreen;
	private Text nameText;
	private Text classText;
	private Text statPointsLeft;

	private Text intellectButtontext;
	private Text staminaButtontext;
	private Text strengthButtontext;

	public int currentStrength;
	public int currentIntellect;
	public int currentStamina;

	public static bool isOpen;

	// Use this for initialization
	void Start () 
	{
		characterInfoScreen = GameObject.Find ("Character Info");
		nameText = GameObject.Find ("Character Name").GetComponent<Text>();
		nameText.text = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ().name;
		classText = GameObject.Find ("Class Name").GetComponent<Text> ();
		classText.text = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ().className;
		statPointsLeft = GameObject.Find ("Stat Points").GetComponent<Text> ();

		intellectButtontext = GameObject.Find ("Intellect").GetComponentInChildren<Text> ();
		staminaButtontext = GameObject.Find ("Stamina").GetComponentInChildren<Text> ();
		strengthButtontext = GameObject.Find ("Strength").GetComponentInChildren<Text> ();

		GameObject.Find("Intellect").GetComponent<Button>().onClick.AddListener (delegate {addIntellect();});
		GameObject.Find("Stamina").GetComponent<Button>().onClick.AddListener (delegate {addStamina();});
		GameObject.Find("Strength").GetComponent<Button>().onClick.AddListener (delegate {addStrength();});

		characterInfoScreen.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (keyCode) && !isOpen && !IngameMenu.isPaused && !PlayerStats.isDead) 
		{
			characterInfoScreen.SetActive (true);
			isOpen = true;
		}
		else if (Input.GetKeyDown (keyCode) && isOpen && !IngameMenu.isPaused)
		{
			characterInfoScreen.SetActive (false);
			isOpen = false;
		} 
		else if (!isOpen)
		{
			characterInfoScreen.SetActive(false);
		}

		statPointsLeft.text = "Stat Points left: " + statPoints;
		strengthButtontext.text = currentStrength.ToString();
		intellectButtontext.text = currentIntellect.ToString();
		staminaButtontext.text = currentStamina.ToString();
	}

	void addStrength() 
	{
		if (statPoints > 0) 
		{
			currentStrength++;
			statPoints--;
		}
	}

	void addIntellect() 
	{
		if (statPoints > 0) 
		{
			currentIntellect++;
			statPoints--;
		}
	}

	void addStamina() 
	{
		if (statPoints > 0)
		{
			currentStamina++;
			statPoints--;
		}
	}
}
