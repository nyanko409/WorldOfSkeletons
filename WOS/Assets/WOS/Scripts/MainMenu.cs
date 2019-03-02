using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	private GameObject previewMage;

	private Button singlePlayerButton;
	private Button settings;
	private Button quitButton;
	private Button backToMainMenu;
	private Button createCharacter;
	private Button loadCharacter;
	private Button createMage;
	private Button finishCreation;
	private Button loadMage;
	private Button deleteCharacter;
	private Button loadSelectedCharacter;

	public static InputField playerName;
	
	private Text infoText;
	private Text gameName;

	// Use this for initialization
	void Start () 
	{
		previewMage = GameObject.Find("Mage Preview");
		previewMage.SetActive (false);

		playerName = GameObject.Find ("Player Name").GetComponent<InputField> ();

		gameName = GameObject.Find ("Game Name").GetComponent<Text> ();
		infoText = GameObject.Find ("Character Info").GetComponent<Text> ();

		singlePlayerButton = GameObject.Find ("Single Player").GetComponent<Button> ();
		settings = GameObject.Find ("Settings").GetComponent<Button> ();
		quitButton = GameObject.Find ("Exit Game").GetComponent<Button> ();
		backToMainMenu = GameObject.Find ("Back To MainMenu").GetComponent<Button> ();
		createCharacter = GameObject.Find ("Create Character").GetComponent<Button> ();
		loadCharacter = GameObject.Find ("Load Character").GetComponent<Button> ();
		createMage = GameObject.Find ("Create Mage").GetComponent<Button> ();
		finishCreation = GameObject.Find ("Finish Creation").GetComponent<Button> ();
		deleteCharacter = GameObject.Find ("Delete Selected Character").GetComponent<Button> ();
		loadSelectedCharacter = GameObject.Find ("Load Selected Character").GetComponent<Button> ();
		loadMage = GameObject.Find ("Load Mage").GetComponent<Button> ();

		finishCreation.onClick.AddListener (delegate {GameObject.Find("SaveLoad").GetComponent<SaveLoadData>().CreateCharacter();});
		loadSelectedCharacter.onClick.AddListener (delegate {GameObject.Find("SaveLoad").GetComponent<SaveLoadData>().LoadCharacter();});
		deleteCharacter.onClick.AddListener (delegate {GameObject.Find("SaveLoad").GetComponent<SaveLoadData>().DeleteCharacter();});
		deleteCharacter.onClick.AddListener (delegate {DeleteCharacter();});
		singlePlayerButton.onClick.AddListener (delegate {SinglePlayer();});

		backToMainMenu.gameObject.SetActive (false);
		createCharacter.gameObject.SetActive (false);
		loadCharacter.gameObject.SetActive (false);
		createMage.gameObject.SetActive (false);
		playerName.gameObject.SetActive (false);
		finishCreation.gameObject.SetActive (false);
		loadMage.gameObject.SetActive (false);
		deleteCharacter.gameObject.SetActive (false);
		loadSelectedCharacter.gameObject.SetActive (false);
		infoText.enabled = false;
		gameName.enabled = true;
	}

	public void SinglePlayer() 
	{
		GetComponent<AudioSource> ().Play ();
		singlePlayerButton.gameObject.SetActive (false);
		settings.gameObject.SetActive (false);
		quitButton.gameObject.SetActive (false);
		backToMainMenu.gameObject.SetActive (true);
		createCharacter.gameObject.SetActive (true);
		loadCharacter.gameObject.SetActive (true);
		gameName.enabled = false;
	}

	public void SettingMenu() 
	{
		GetComponent<AudioSource> ().Play ();
		settings.gameObject.SetActive (false);
		singlePlayerButton.gameObject.SetActive (false);
		quitButton.gameObject.SetActive (false);
		backToMainMenu.gameObject.SetActive (true);
		gameName.enabled = false;
	}

	public void BackToMainMenu() 
	{
		GetComponent<AudioSource> ().Play ();
		previewMage.GetComponent<RotateShowcase> ().resetPosition ();

		singlePlayerButton.gameObject.SetActive (true);
		settings.gameObject.SetActive (true);
		quitButton.gameObject.SetActive (true);
		backToMainMenu.gameObject.SetActive (false);
		createCharacter.gameObject.SetActive (false);
		loadCharacter.gameObject.SetActive (false);
		previewMage.gameObject.SetActive (false);
		createMage.gameObject.SetActive (false);
		playerName.gameObject.SetActive (false);
		finishCreation.gameObject.SetActive (false);
		loadMage.gameObject.SetActive (false);
		infoText.enabled = false;
		deleteCharacter.gameObject.SetActive (false);
		loadSelectedCharacter.gameObject.SetActive (false);
		GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().errorText.enabled = false;
		gameName.enabled = true;
	}

	public void CreateCharacter() 
	{
		GetComponent<AudioSource> ().Play ();
		createMage.gameObject.SetActive (true);
		loadMage.gameObject.SetActive (false);
		infoText.enabled = false;
		deleteCharacter.gameObject.SetActive (false);
		loadSelectedCharacter.gameObject.SetActive (false);
	}

	public void LoadCharacter() 
	{
		GetComponent<AudioSource> ().Play ();
		previewMage.GetComponent<RotateShowcase> ().resetPosition ();

		createMage.gameObject.SetActive (false);
		previewMage.gameObject.SetActive (false);
		playerName.gameObject.SetActive (false);
		finishCreation.gameObject.SetActive (false);
		infoText.enabled = false;
		deleteCharacter.gameObject.SetActive (false);
		loadSelectedCharacter.gameObject.SetActive (false);
		GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().errorText.enabled = false;

		loadMage.gameObject.SetActive (true);
		if (PlayerPrefs.HasKey ("Name_Mage")) 
		{
			loadMage.gameObject.GetComponentInChildren<Text> ().text = PlayerPrefs.GetString ("Name_Mage");
		} 
		else 
		{
			loadMage.gameObject.GetComponentInChildren<Text> ().text = "";
		}
		if (loadMage.GetComponentInChildren<Text> ().text == "") 
		{
			loadMage.gameObject.SetActive (false);
		}
	}

	public void CreateMage() 
	{
		GetComponent<AudioSource> ().Play ();
		previewMage.SetActive (true);
		playerName.gameObject.SetActive (true);
		finishCreation.gameObject.SetActive (true);
		GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().selectedClass = "Mage";
		GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().errorText.enabled = false;
	}

	public void MageInfo() 
	{
		GetComponent<AudioSource> ().Play ();
		GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().selectedClass = "Mage";
		infoText.enabled = true;
		infoText.text = "Name: " + PlayerPrefs.GetString ("Name_Mage") + "\nClass: " + "Mage";
		deleteCharacter.gameObject.SetActive (true);
		loadSelectedCharacter.gameObject.SetActive (true);
	}

	public void DeleteCharacter() 
	{
		GetComponent<AudioSource> ().Play ();
		// disable gui stuff if character is deleted
		if (GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().selectedClass == "Mage") 
		{
			loadMage.gameObject.SetActive (false);
			deleteCharacter.gameObject.SetActive (false);
			loadSelectedCharacter.gameObject.SetActive (false);
			infoText.enabled = false;
		} 
		else if (GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().selectedClass == "class2") 
		{

		} 
		else if (GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().selectedClass == "class3") 
		{

		}
	}

	public void QuitGame() 
	{
		GetComponent<AudioSource> ().Play ();
		Application.Quit ();
	}
}
