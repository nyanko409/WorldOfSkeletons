using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionBar : MonoBehaviour 
{
	public static bool isOpen;
	private GameObject skillSelect;
	private string selectedActionbarString;
	private KeyCode selectedActionbarKeycode;
	
	private Button fireballButton;
	
	// Use this for initialization
	void Start () 
	{
		GameObject.Find("Actionbar1").GetComponent<Button>().onClick.AddListener (delegate {Actionbar1();});
		GameObject.Find("Actionbar2").GetComponent<Button>().onClick.AddListener (delegate {Actionbar2();});
		GameObject.Find("Actionbar3").GetComponent<Button>().onClick.AddListener (delegate {Actionbar3();});
		GameObject.Find("Actionbar4").GetComponent<Button>().onClick.AddListener (delegate {Actionbar4();});

		skillSelect = GameObject.Find ("Select Skills Mage");
		fireballButton = skillSelect.transform.Find ("Fireball").GetComponent<Button> ();
		skillSelect.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkSpellsLearned ();

		//open skill select menu if the condition is true
		if (isOpen && !IngameMenu.isPaused && !PlayerStats.isDead && !SkillTreeMage.isOpen && !CharacterInfo.isOpen && !Inventory.isOpen)
		{
			skillSelect.SetActive (true);
		} 
		else 
		{
			isOpen = false;
			skillSelect.SetActive(false);
		}
	}

	void skillPressed(string skillName) 
	{
		switch (skillName) 
		{
			case "Fireball": 	
				checkDuplicate("FireballPNG");
				GameObject.Find(selectedActionbarString).GetComponent<Image>().sprite = Resources.Load<Sprite>("FireballPNG");
				GetComponent<Fireball>().key = selectedActionbarKeycode;
				break;

			default: 			
				break;
		}
		isOpen = false;
	}

	public void Actionbar1() 
	{
		if (!IngameMenu.isPaused) 
		{
			isOpen = true;
			selectedActionbarString = "Actionbar1";
			selectedActionbarKeycode = KeyCode.Alpha1;
		}
	}

	public void Actionbar2() 
	{
		if (!IngameMenu.isPaused)
		{
			isOpen = true;
			selectedActionbarString = "Actionbar2";
			selectedActionbarKeycode = KeyCode.Alpha2;
		}
	}

	public void Actionbar3() 
	{
		if (!IngameMenu.isPaused) 
		{
			isOpen = true;
			selectedActionbarString = "Actionbar3";
			selectedActionbarKeycode = KeyCode.Alpha3;
		}
	}

	public void Actionbar4()
	{
		if (!IngameMenu.isPaused)
		{
			isOpen = true;
			selectedActionbarString = "Actionbar4";
			selectedActionbarKeycode = KeyCode.Alpha4;
		}
	}

	void checkSpellsLearned() 
	{
		if (GetComponent<SkillTreeMage> ().lvl_Fireball > 0) 
		{
			fireballButton.gameObject.SetActive(true);
		} 
		else 
		{
			fireballButton.gameObject.SetActive(false);
		}
	}

	void checkDuplicate(string duplicateName) 
	{
		string sprite1 = GameObject.Find ("Actionbar1").GetComponent<Image>().sprite.name;
		string sprite2 = GameObject.Find ("Actionbar2").GetComponent<Image>().sprite.name;
		string sprite3 = GameObject.Find ("Actionbar3").GetComponent<Image>().sprite.name;
		string sprite4 = GameObject.Find ("Actionbar4").GetComponent<Image>().sprite.name;

		// delete duplicate
		if (sprite1 == duplicateName) 
		{
			GameObject.Find("Actionbar1").GetComponent<Image>().sprite = Resources.Load<Sprite>("Actionbar");
		} 
		else if (sprite2 == duplicateName) 
		{
			GameObject.Find("Actionbar2").GetComponent<Image>().sprite = Resources.Load<Sprite>("Actionbar");
		}
		else if (sprite3 == duplicateName)
		{
			GameObject.Find("Actionbar3").GetComponent<Image>().sprite = Resources.Load<Sprite>("Actionbar");
		} 
		else if (sprite4 == duplicateName)
		{
			GameObject.Find("Actionbar4").GetComponent<Image>().sprite = Resources.Load<Sprite>("Actionbar");
		}
	}
}
