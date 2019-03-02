using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveLoadData : MonoBehaviour 
{
	public string selectedClass;
	public string playerName;

	public Text errorText;

	public static ItemDatabase database;
	public static GameObject inventoryScreen;

	void Awake() 
	{
		DontDestroyOnLoad (gameObject);

		//destroy duplicates
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () 
	{
		database = gameObject.GetComponent<ItemDatabase> ();
		errorText = GameObject.Find ("Error Message").GetComponent<Text> ();
		errorText.enabled = false;
	}

	void OnLevelWasLoaded(int level) 
	{
		if (level == 0) 
		{
			errorText = GameObject.Find ("Error Message").GetComponent<Text> ();
			errorText.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateCharacter() 
	{
		GameObject.Find("MenuController").GetComponent<AudioSource> ().Play ();
		//returns if name is null
		if (MainMenu.playerName.GetComponentInChildren<Text>().text == "") 
		{
			errorText.enabled = true;
			errorText.text = "The Character\nneeds a name!";
			return;
		}
		playerName = MainMenu.playerName.text;
		if (!saveDataAtCreation ()) 
		{
			//if the selected class already exist
			errorText.enabled = true;
			errorText.text = "You have already\ncreated this class!";
			return;
		}
		SceneManager.LoadScene(1);
	}

	public void LoadCharacter() 
	{
		GameObject.Find("MenuController").GetComponent<AudioSource> ().Play ();
		SceneManager.LoadScene(1);
	}

	public void DeleteCharacter() 
	{
		//delete selected character data 
		if (selectedClass == "Mage") 
		{
			PlayerPrefs.DeleteKey("Name_Mage");
			PlayerPrefs.DeleteKey("Level_Mage");
			PlayerPrefs.DeleteKey("Exp_Mage");

			PlayerPrefs.DeleteKey("SkillPoints_Mage");
			PlayerPrefs.DeleteKey("SkillFireball_Mage");

			PlayerPrefs.DeleteKey("StatPoints_Mage");
			PlayerPrefs.DeleteKey("CurrentStrength_Mage");
			PlayerPrefs.DeleteKey("CurrentStamina_Mage");
			PlayerPrefs.DeleteKey("CurrentIntellect_Mage");

			PlayerPrefs.DeleteKey("Actionbar1_Mage");
			PlayerPrefs.DeleteKey("Actionbar2_Mage");
			PlayerPrefs.DeleteKey("Actionbar3_Mage");
			PlayerPrefs.DeleteKey("Actionbar4_Mage");

			PlayerPrefs.DeleteKey("KeycodeFireball_Mage");

			// deleting inventory items
			for(int i = 1; i <= 24; i++)
			{
				PlayerPrefs.DeleteKey("Slot" + i.ToString() + "_Mage");
			}
			for(int i = 1; i <= 24; i++)
			{
				PlayerPrefs.DeleteKey("Slot" + i.ToString() + "_amount_Mage");
			}
			PlayerPrefs.DeleteKey("Chest_Slot_Mage");
			PlayerPrefs.DeleteKey("Shoes_Slot_Mage");
			PlayerPrefs.DeleteKey("Gloves_Slot_Mage");
			PlayerPrefs.DeleteKey("Head_Slot_Mage");
			PlayerPrefs.DeleteKey("Weapon1_Slot_Mage");
			PlayerPrefs.DeleteKey("Weapon2_Slot_Mage");
		} 
		else if (selectedClass == "class2") 
		{
			PlayerPrefs.DeleteKey("Name2");
		} 
		else if (selectedClass == "class3") 
		{
			PlayerPrefs.DeleteKey("Name3");
		}
	}

	public bool saveDataAtCreation() 
	{
		// 3 save slots for now // at creation, save only name and class
		if(!PlayerPrefs.HasKey("Name_Mage") && selectedClass == "Mage") 
		{
			PlayerPrefs.SetString ("Name_Mage", playerName);
			return true;
		} 
		else if(!PlayerPrefs.HasKey("Name2") && selectedClass == "class2") 
		{
			PlayerPrefs.SetString ("Name2", playerName);
			return true;
		} 
		else if(!PlayerPrefs.HasKey("Name3") && selectedClass == "class3") 
		{
			PlayerPrefs.SetString ("Name3", playerName);
			return true;
		}
		return false;
	}

	public static void saveDataAtQuittingGame() 
	{
		inventoryScreen.SetActive(true);
		PlayerStats playerStats = GameObject.FindGameObjectWithTag ("Player").gameObject.GetComponent<PlayerStats>();
		//save dynamic data like lvl, exp, activated waypoints, items etc.
		if (GameObject.Find ("Player_Mage(Clone)")) 
		{
			PlayerPrefs.SetInt("Level_Mage", playerStats.level);
			PlayerPrefs.SetInt("Exp_Mage", playerStats.currentExp);

			PlayerPrefs.SetInt("SkillPoints_Mage", playerStats.GetComponent<SkillTreeMage>().skillPoints);
			PlayerPrefs.SetInt("SkillFireball_Mage", playerStats.GetComponent<SkillTreeMage>().lvl_Fireball);

			PlayerPrefs.SetInt("StatPoints_Mage", playerStats.GetComponent<CharacterInfo>().statPoints);
			PlayerPrefs.SetInt("CurrentStrength_Mage", playerStats.GetComponent<CharacterInfo>().currentStrength);
			PlayerPrefs.SetInt("CurrentStamina_Mage", playerStats.GetComponent<CharacterInfo>().currentStamina);
			PlayerPrefs.SetInt("CurrentIntellect_Mage", playerStats.GetComponent<CharacterInfo>().currentIntellect);

			PlayerPrefs.SetString("Actionbar1_Mage", GameObject.Find("Actionbar1").GetComponent<Image>().sprite.name);
			PlayerPrefs.SetString("Actionbar2_Mage", GameObject.Find("Actionbar2").GetComponent<Image>().sprite.name);
			PlayerPrefs.SetString("Actionbar3_Mage", GameObject.Find("Actionbar3").GetComponent<Image>().sprite.name);
			PlayerPrefs.SetString("Actionbar4_Mage", GameObject.Find("Actionbar4").GetComponent<Image>().sprite.name);

			PlayerPrefs.SetString("KeycodeFireball_Mage", playerStats.GetComponent<Fireball>().key.ToString());

			// save inventory items and amount
			for(int i = 1; i <= 24; i++)
			{
				PlayerPrefs.SetInt("Slot" + i.ToString() + "_Mage", playerStats.GetComponent<Inventory>().getItemID(i-1));
			}
			for(int i = 1; i <= 24; i++)
			{
				PlayerPrefs.SetInt("Slot" + i.ToString() + "_amount_Mage", playerStats.GetComponent<Inventory>().slots[i-1].GetComponent<Slot>().currentStack);
			}

			if(GameObject.Find("Chest Slot").GetComponent<EquipSlot>().item != null)
				PlayerPrefs.SetInt("Chest_Slot_Mage", GameObject.Find("Chest Slot").GetComponent<EquipSlot>().item.itemID);
			else
				PlayerPrefs.SetInt("Chest_Slot_Mage", 0);																		// id 0 == set item null

			if(GameObject.Find("Shoes Slot").GetComponent<EquipSlot>().item != null)
				PlayerPrefs.SetInt("Shoes_Slot_Mage", GameObject.Find("Shoes Slot").GetComponent<EquipSlot>().item.itemID);
			else
				PlayerPrefs.SetInt("Shoes_Slot_Mage", 0);

			if(GameObject.Find("Gloves Slot").GetComponent<EquipSlot>().item != null)
				PlayerPrefs.SetInt("Gloves_Slot_Mage", GameObject.Find("Gloves Slot").GetComponent<EquipSlot>().item.itemID);
			else
				PlayerPrefs.SetInt("Gloves_Slot_Mage", 0);

			if(GameObject.Find("Head Slot").GetComponent<EquipSlot>().item != null)
				PlayerPrefs.SetInt("Head_Slot_Mage", GameObject.Find("Head Slot").GetComponent<EquipSlot>().item.itemID);
			else
				PlayerPrefs.SetInt("Head_Slot_Mage", 0);

			if(GameObject.Find("Weapon Slot 1").GetComponent<EquipSlot>().item != null)
				PlayerPrefs.SetInt("Weapon1_Slot_Mage", GameObject.Find("Weapon Slot 1").GetComponent<EquipSlot>().item.itemID);
			else
				PlayerPrefs.SetInt("Weapon1_Slot_Mage", 0);

			if(GameObject.Find("Weapon Slot 2").GetComponent<EquipSlot>().item != null)
				PlayerPrefs.SetInt("Weapon2_Slot_Mage", GameObject.Find("Weapon Slot 2").GetComponent<EquipSlot>().item.itemID);
			else
				PlayerPrefs.SetInt("Weapon2_Slot_Mage", 0);
		} 
		else if (GameObject.Find ("Player_class2")) 
		{
			
		} 
		else if (GameObject.Find ("Player_class3")) 
		{
			
		}
	}

	public static void loadData() 
	{
		inventoryScreen.SetActive(true);
		PlayerStats playerStats = GameObject.FindGameObjectWithTag ("Player").gameObject.GetComponent<PlayerStats>();
		//load character
		if(GameObject.Find("Player_Mage(Clone)") && PlayerPrefs.HasKey("Actionbar1_Mage")) 
		{
			playerStats.name = PlayerPrefs.GetString("Name_Mage");
			playerStats.level = PlayerPrefs.GetInt("Level_Mage");
			playerStats.currentExp = PlayerPrefs.GetInt("Exp_Mage");
			playerStats.className = "Mage";

			playerStats.GetComponent<SkillTreeMage>().skillPoints = PlayerPrefs.GetInt("SkillPoints_Mage");
			playerStats.GetComponent<SkillTreeMage>().lvl_Fireball = PlayerPrefs.GetInt("SkillFireball_Mage");

			playerStats.GetComponent<CharacterInfo>().statPoints = PlayerPrefs.GetInt("StatPoints_Mage");
			playerStats.GetComponent<CharacterInfo>().currentStamina = PlayerPrefs.GetInt("CurrentStamina_Mage");
			playerStats.GetComponent<CharacterInfo>().currentIntellect = PlayerPrefs.GetInt("CurrentIntellect_Mage");
			playerStats.GetComponent<CharacterInfo>().currentStrength = PlayerPrefs.GetInt("CurrentStrength_Mage");

			GameObject.Find("Actionbar1").GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("Actionbar1_Mage"));
			GameObject.Find("Actionbar2").GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("Actionbar2_Mage"));
			GameObject.Find("Actionbar3").GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("Actionbar3_Mage"));
			GameObject.Find("Actionbar4").GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("Actionbar4_Mage"));

			playerStats.GetComponent<Fireball>().key = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeycodeFireball_Mage"));

			//loading inventory items
			for(int i = 1; i <= 24; i++)
			{
				playerStats.GetComponent<Inventory>().loadItem(i-1, PlayerPrefs.GetInt("Slot" + i.ToString() + "_Mage"), PlayerPrefs.GetInt("Slot" + i.ToString() + "_amount_Mage"));
			}
			GameObject.Find("Chest Slot").GetComponent<EquipSlot>().item = database.getItem(PlayerPrefs.GetInt("Chest_Slot_Mage"));
			GameObject.Find("Shoes Slot").GetComponent<EquipSlot>().item = database.getItem(PlayerPrefs.GetInt("Shoes_Slot_Mage"));
			GameObject.Find("Gloves Slot").GetComponent<EquipSlot>().item = database.getItem(PlayerPrefs.GetInt("Gloves_Slot_Mage"));
			GameObject.Find("Head Slot").GetComponent<EquipSlot>().item = database.getItem(PlayerPrefs.GetInt("Head_Slot_Mage"));
			GameObject.Find("Weapon Slot 1").GetComponent<EquipSlot>().item = database.getItem(PlayerPrefs.GetInt("Weapon1_Slot_Mage"));
			GameObject.Find("Weapon Slot 2").GetComponent<EquipSlot>().item = database.getItem(PlayerPrefs.GetInt("Weapon2_Slot_Mage"));
		} 
		else if(GameObject.Find("Player_class2")) 
		{

		} 
		else if(GameObject.Find("Player_class3")) 
		{

		}
		inventoryScreen.SetActive(false);
	}
}
