using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour 
{
	[HideInInspector] public string name;
	[HideInInspector] public string className;

	[HideInInspector] public int level;
	[HideInInspector] public int currentExp;
	[HideInInspector] [SerializeField] private int expNeeded;

	[HideInInspector] public Transform currentTarget;
	[HideInInspector] public bool inCombat;

	[HideInInspector] public static bool attacking;

	public float damage;

	public float maxHealth;
	[HideInInspector] public float currentHealth;
	public float maxMana;
	[HideInInspector] public float currentMana;
	public float manaPerSec;
	public float hpPerSec;

	[HideInInspector] public float distanceToEnemy;
	private bool hasAttacked;

	public float stop;

	private Text hpText;
	private Text manaText;
	private Text respawnText;
	private Text expText;
	private Text levelText;

    private Image hpTexture;
	private Image mpTexture;
	private Image xpTexture;

	[HideInInspector] public static bool isDead;
	private bool animationPlayed;
	private bool deadSoundPlayed;

	// Use this for initialization
	void Start () 
	{
		//load character data
		SaveLoadData.loadData();
		if (name == "") 
		{
			// first time this character was instantiated, give some lvl 1 starter points here
			name = GameObject.Find ("SaveLoad").GetComponent<SaveLoadData> ().playerName;
			level = 1;
			GetComponent<SkillTreeMage>().skillPoints++;
			GetComponent<CharacterInfo>().statPoints += 3;
		}
		//load complete
		//additional setup
		hpText = GameObject.Find ("HP Text").GetComponent<Text>();
		respawnText = GameObject.Find ("Respawn Text").GetComponent<Text> ();
		expText = GameObject.Find ("Exp Text").GetComponent<Text> ();
		levelText = GameObject.Find ("Level Text").GetComponent<Text> ();
		manaText = GameObject.Find ("Mana Text").GetComponent<Text> ();

		hpTexture = GameObject.Find("Player HP Bar").GetComponent<Image>();
		mpTexture = GameObject.Find("Player MP Bar").GetComponent<Image>();
		xpTexture = GameObject.Find("Player XP Bar").GetComponent<Image>();

		respawnText.enabled = false;
		isDead = false;
		attacking = false;

		//set maxHealth / maxMana depending on stats at start
		maxHealth = GetComponent<CharacterInfo> ().currentStamina * 10 + 100;
		maxMana = GetComponent<CharacterInfo> ().currentIntellect * 10 + 100;
		currentHealth = maxHealth;
		currentMana = maxMana;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//set maxHealth / maxMana depending on stats real time
		maxHealth = GetComponent<CharacterInfo> ().currentStamina * 10 + 100;
		maxMana = GetComponent<CharacterInfo> ().currentIntellect * 10 + 100;

		hpText.text = "HP: " + currentHealth.ToString("F0") + " / " + maxHealth;
		levelText.text = "lvl " + level.ToString ();
		expText.text = currentExp.ToString () + " / " + expNeeded.ToString ();
		manaText.text = "MP: " + currentMana.ToString ("F0") + " / " + maxMana;

		levelUp ();

		// set hp/mp bar height depending on remaining hp and mp
		hpTexture.fillAmount = currentHealth / maxHealth;
		mpTexture.fillAmount = currentMana / maxMana;
		xpTexture.GetComponent<RectTransform>().sizeDelta = new Vector2((currentExp * 1.0f / expNeeded * 1.0f) * 170f, 15f);
		// hp and mana regeneration
		if (!isDead) 
		{
			if (currentHealth < maxHealth) 
			{
				currentHealth += hpPerSec * Time.deltaTime;
			}
			if (currentMana < maxMana) 
			{
				currentMana += manaPerSec * Time.deltaTime;
			}
		}

		// if dead
		if (isDead) 
		{
			dead ();
		}

		// if attacking
		if (!isDead && inCombat && currentTarget != null) 
		{
			attack ();
		}
	}

	public bool healPlayerByPercentage(int healPercentage)
	{
		int amount = (int)((maxHealth / 100) * healPercentage);
		if (currentHealth < maxHealth)
		{
			currentHealth += amount;
			if(currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
			}
			return true;
		}
		return false;
	}

	void applyDamage(float damage) 
	{
		if (!isDead) 
		{
			currentHealth -= damage;
			if (currentHealth <= 0) 
			{
				currentHealth = 0f;
				isDead = true;
			}
		}
	}

	void dead() 
	{
		if(!deadSoundPlayed) // play sound one time
		{
			SoundEffects.audios[3].Play();
			deadSoundPlayed = true;
		}
		CharacterInfo.isOpen = false;
		SkillTreeMage.isOpen = false;
		Inventory.isOpen = false;
		//play animation once
		if (!animationPlayed) 
		{
			GetComponent<Animation> ().CrossFade ("die");
			animationPlayed = true;
		}
		respawnText.enabled = true;
		//if escape is pressed, respawn player at respawn point
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			transform.position = GameObject.FindGameObjectWithTag ("Respawn").transform.position;
			respawnText.enabled = false;
			currentHealth = maxHealth;
			currentMana = maxMana;
			animationPlayed = false;
			isDead = false;
			deadSoundPlayed = false;
		}
	}
	
	void attack() 
	{
		if (!attacking) 
		{
			lookAtEnemy ();
			getDistanceToEnemy ();
			if (distanceToEnemy <= stop) 
			{
				dealDamage ();
			}
		}
	}

	public void getDistanceToEnemy() 
	{
		distanceToEnemy = Vector3.Distance (transform.position, currentTarget.position);
	}

	void dealDamage() 
	{
		//deal damage if attack animation is at >=50%
		if (!GetComponent<Animation> ().IsPlaying ("attack")) 
		{
			hasAttacked = false;
			GetComponent<Animation> ().CrossFade ("attack");
		}
		if (GetComponent<Animation> () ["attack"].normalizedTime >= 0.5 && !hasAttacked)
		{
			currentTarget.SendMessage ("applyDamage", damage);
			hasAttacked = true;
		}
	}

	void lookAtEnemy() 
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentTarget.position - transform.position), 1000 * Time.deltaTime);
	}

	//check if colliding with a spell from enemy
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "EnemySpell") 
		{
			applyDamage(other.GetComponent<FireballObj>().damage);
			Destroy (other.gameObject);
		}
	}

	void levelUp() 
	{
		//algorithm for exp needed
		expNeeded = level * 50 + 10;

		if (currentExp >= expNeeded) 
		{
			//level up
			level++;
			currentExp -= expNeeded;
			currentHealth = maxHealth;
			currentMana = maxMana;
			GetComponent<SkillTreeMage>().skillPoints++;
			GetComponent<CharacterInfo>().statPoints += 3;
		}
	}

	public bool checkMana(float manaCost) 
	{
		if ((currentMana - manaCost) >= 0)
		{
			currentMana -= manaCost;
			return true;
		}
		if(!SoundEffects.audios[4].isPlaying)
		{
			SoundEffects.audios[4].Play();
		}
		return false;
	}
}
