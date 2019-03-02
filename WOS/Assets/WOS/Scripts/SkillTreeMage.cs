using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SkillTreeMage : MonoBehaviour
{
	public int skillPoints;

	public KeyCode keyCode;
	private GameObject skillTree;
	private GameObject spellTooltip;
	public static bool isOpen;

	private Text skillPointsText;

	//save skill lvls
	public int lvl_Fireball;

	// Use this for initialization
	void Start ()
	{
		skillTree = GameObject.Find ("Skill Tree Mage").gameObject;
		skillTree.transform.Find ("Skill Fireball").GetComponent<Button> ().onClick.AddListener (delegate {GameObject.FindGameObjectWithTag("Player").GetComponent<SkillTreeMage>().SkillFireball();});
		skillTree.transform.Find ("Skill Fireball").Find ("Text").GetComponent<Text> ().text = lvl_Fireball.ToString ();
		skillPointsText = GameObject.Find ("Skill Points").GetComponent<Text> ();
		spellTooltip = GameObject.Find("Tooltip Spell");

		skillTree.SetActive (false);
		spellTooltip.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (keyCode) && !isOpen && !IngameMenu.isPaused && !PlayerStats.isDead) 
		{
			//open skilltree
			Inventory.isOpen = false;
			skillTree.SetActive (true);
			isOpen = true;
		} 
		else if (Input.GetKeyDown (keyCode) && isOpen && !IngameMenu.isPaused) 
		{
			//close skilltree
			skillTree.SetActive (false);
			isOpen = false;
		} 
		else if (!isOpen) 
		{
			spellTooltip.SetActive(false);
			skillTree.SetActive(false);
		}

		//setSkills
		setSkills ();
		skillPointsText.text = "Skill Points left: " + skillPoints;
	}

	void setSkills()
	{
		//activate / deactivate and modify skills depending on skill level and set damage/mana cost
		if (lvl_Fireball > 0) 
		{
			GetComponent<Fireball> ().enabled = true;
			GetComponent<Fireball>().damage = 10 + (10 * lvl_Fireball);
			GetComponent<Fireball>().manaCost = 15 + (15 * lvl_Fireball);
		} 
		else 
		{
			GetComponent<Fireball>().enabled = false;
		}
	}

	public void SkillFireball() 
	{
		//fireball skill was clicked // is called from fireball button
		if (skillPoints > 0) 
		{
			lvl_Fireball = int.Parse (skillTree.transform.Find ("Skill Fireball").Find ("Text").GetComponent<Text> ().text);
			lvl_Fireball++;
			skillTree.transform.Find ("Skill Fireball").Find ("Text").GetComponent<Text> ().text = lvl_Fireball.ToString ();
			skillPoints--;
			setSkills();
		}
	}

	public void showTooltip(string spellName, string description, Vector3 position)
	{
		SpellInfo spellToShow = getSpell(spellName);
		SpellInfo spellToShowNextLvl = getNextLvlSpell(spellName);
		spellTooltip.SetActive(true);

		spellTooltip.transform.position = new Vector3(position.x - 270, position.y - 20, 0);
		spellTooltip.transform.Find("Spell Name").GetComponent<Text>().text = spellName;
		spellTooltip.transform.Find("Spell Info").GetComponent<Text>().text = description;
		if(spellToShow.hasLearnedSpell)
		{
			spellTooltip.transform.Find("Manacost").GetComponent<Text>().text = "Manacost: " + spellToShow.manacost.ToString();
			spellTooltip.transform.Find("Cooldown").GetComponent<Text>().text = "Cooldown: " + spellToShow.cooldown.ToString();
			spellTooltip.transform.Find("Damage").GetComponent<Text>().text = "Damage: " + spellToShow.damage.ToString();
		}
		else
		{
			spellTooltip.transform.Find("Manacost").GetComponent<Text>().text = "";
			spellTooltip.transform.Find("Cooldown").GetComponent<Text>().text = "";
			spellTooltip.transform.Find("Damage").GetComponent<Text>().text = "";
		}
		spellTooltip.transform.Find("Manacost NextLevel").GetComponent<Text>().text = "Manacost: " + spellToShowNextLvl.manacost.ToString();
		spellTooltip.transform.Find("Cooldown NextLevel").GetComponent<Text>().text = "Cooldown: " + spellToShowNextLvl.cooldown.ToString();
		spellTooltip.transform.Find("Damage NextLevel").GetComponent<Text>().text = "Damage: " + spellToShowNextLvl.damage.ToString();
	}

	public void hideTooltip()
	{
		spellTooltip.SetActive(false);
	}

	SpellInfo getSpell(string spellName)
	{
		if(spellName == "Fireball")
		{
			int temp = int.Parse (skillTree.transform.Find ("Skill Fireball").Find ("Text").GetComponent<Text> ().text);
			return new SpellInfo(GetComponent<Fireball>().damage, GetComponent<Fireball>().cooldown, GetComponent<Fireball>().manaCost, temp > 0);
		}
		return null;
	}

	SpellInfo getNextLvlSpell(string spellName)
	{
		if(spellName == "Fireball")
		{
			return new SpellInfo(10 + (10 * (lvl_Fireball + 1)), GetComponent<Fireball>().cooldown, 15 + (15 * (lvl_Fireball + 1)));
		}
		return null;
	}









	public class SpellInfo
	{
		public float damage;
		public float cooldown;
		public float manacost;
		public bool hasLearnedSpell;

		public SpellInfo(float damage, float cooldown, float manacost, bool hasLearnedSpell)
		{
			this.damage = damage;
			this.cooldown = cooldown;
			this.manacost = manacost;
			this.hasLearnedSpell = hasLearnedSpell;
		}

		public SpellInfo(float damage, float cooldown, float manacost)
		{
			this.damage = damage;
			this.cooldown = cooldown;
			this.manacost = manacost;
		}
	}
}
