using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyAIAdvanced : MonoBehaviour 
{
	public string name;
	public int level;
	public EnemyType.Type Type;
	[HideInInspector] int expWorth;

	private Image healthBar;
	private Image backgroundBar;
	private Text nameText;

	[HideInInspector] float damage; //will be scaled with enemy lvl
	[HideInInspector] float maxHealth; //will be scaled with enemy lvl
	[HideInInspector] public float currentHealth;
	public float range;
	private float stop = 2f;

	private bool hasAttacked;

	[HideInInspector] Transform goal;
	private UnityEngine.AI.NavMeshAgent agent;
	private Animation animations;

	private bool isDead;
	private bool animationPlayed;
	private bool expGiven;
	public float destroyAfterSeconds;

	private bool deadSoundPlayed;
	private bool itemDropped;
	private int maxItemDrops;

	public int extraHealth;
	public int extraDamage;
	public int extraExp;

	private float typeMultiplikator;

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		animations = GetComponent<Animation>();

		//setting up UI stuff
		healthBar = GameObject.Find ("Enemy HP Bar").GetComponent<Image>();
		backgroundBar = GameObject.Find ("Background HP Bar").GetComponent<Image> ();
		nameText = GameObject.Find ("Enemy Name").GetComponent<Text> ();
		healthBar.enabled = false;
		backgroundBar.enabled = false;
		nameText.enabled = false;

		setMultiplikatorAndItemDrops();
		setStats ();
		currentHealth = maxHealth;
	}

	void setMultiplikatorAndItemDrops()
	{
		typeMultiplikator = 1;
		maxItemDrops = 2;
		if(Type == EnemyType.Type.Champion)
		{
			typeMultiplikator = 1.5f;
			maxItemDrops = 4;
		}
		if(Type == EnemyType.Type.Elite)
		{
			typeMultiplikator = 2;
			maxItemDrops = 6;
		}
		if(Type == EnemyType.Type.Unique)
		{
			typeMultiplikator = 3;
			maxItemDrops = 8;
		}
	}

	void setStats() 
	{
		//set stats depending on enemy lvl and enemytype
		for (int i=1; i <= level; i++)
		{
			maxHealth += 15 * typeMultiplikator;
			damage += 5 * typeMultiplikator;
			expWorth += (int)(10 * typeMultiplikator);
		}
		maxHealth += extraHealth;
		damage += extraDamage;
		expWorth += extraExp;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isDead) 
		{
			if(goal != null && !PlayerStats.isDead)
			{
				if(Vector3.Distance(transform.position, goal.position) <= range && Vector3.Distance(transform.position, goal.position) > stop) 
				{
					//do stuff if player is not dead and also in range
					agent.Resume();
					agent.destination = goal.position;
					playWalkingAnimation();
				} 
				else if(Vector3.Distance(transform.position, goal.position) <= stop) 
				{
					//enemy is in attack range
					lookAtPlayer();
					attackPlayer(damage);
				} 
				else if(Vector3.Distance(transform.position, goal.position) > range) 
				{
					//if not in range, play animation
					agent.Stop();
					playIdleAnimation();
				}
			} 
			else if(GameObject.FindWithTag("Player") != null)
			{
				goal = GameObject.FindWithTag ("Player").transform;
				if(PlayerStats.isDead)
				{
					playIdleAnimation(); // play idle animation if player is dead
				}
			}
		} 
		else 
		{
			dead();
		}
	}

	void attackPlayer(float damage) 
	{
		//deal damage if attack animation is at >=50%
		if (!GetComponent<Animation> ().IsPlaying ("attack")) 
		{
			hasAttacked = false;
			playAttackAnimation();
			if(!SoundEffects.audios[1].isPlaying)
		    {
				SoundEffects.audios[1].Play();
			}
		}
		
		if (GetComponent<Animation> () ["attack"].normalizedTime >= 0.5 && !hasAttacked)
		{
			goal.gameObject.SendMessage ("applyDamage", damage);
			hasAttacked = true;
		}
	}

	void lookAtPlayer() 
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goal.position - transform.position), 10 * Time.deltaTime);
	}

	void dead() 
	{
		GetComponent<BoxCollider> ().enabled = false;
		agent.enabled = false;

		if(!deadSoundPlayed)
		{
			SoundEffects.audios[5].Play();
			deadSoundPlayed = true;
		}
		//play dead animation only once
		if (!animationPlayed) 
		{
			playDeadAnimation();
			animationPlayed = true;
		}
		//give exp
		if (!expGiven) 
		{
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ().currentExp += expWorth;
			expGiven = true;
		}
		if(!itemDropped)
		{
			dropRandomItems();
			itemDropped = true;
		}
		Destroy (gameObject, destroyAfterSeconds);
	}

	void applyDamage(float damage) 
	{
		currentHealth -= damage;
		if (currentHealth <= 0) 
		{
			currentHealth = 0f;
			isDead = true;
		}
	}

	//mouse klicked on this enemy
	void OnMouseDown() 
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ().currentTarget = transform;
	}
	
	//mouse is on this object
	void OnMouseOver() 
	{
		healthBar.enabled = true;
		backgroundBar.enabled = true;
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2( 2f * ((currentHealth/maxHealth) * 100f), 20f);
		nameText.enabled = true;
		setNameColor();
		nameText.text = name + " lvl " + level;
	}
	
	//mouse is leaving this object
	void OnMouseExit() 
	{
		healthBar.enabled = false;
		backgroundBar.enabled = false;
		nameText.enabled = false;
	}

	void setNameColor()
	{
		nameText.color = Color.white;  // default color
		if(Type == EnemyType.Type.Champion)
		{
			nameText.color = Color.blue;
		}
		if(Type == EnemyType.Type.Elite)
		{
			nameText.color = Color.green;
		}
		if(Type == EnemyType.Type.Unique)
		{
			nameText.color = Color.yellow;
		}
	}

	void dropRandomItems()
	{
		int quantity = Random.Range(0, maxItemDrops + 1);

		for(int i = 1; i <= quantity; i++)
		{
			ItemDatabase database = GameObject.Find("SaveLoad").GetComponent<ItemDatabase>();
			Item item = database.getRandomItem();
			GameObject obj = (GameObject)Instantiate(item.itemModel, transform.position, Quaternion.identity);
			obj.GetComponent<ItemObj>().item = item;
			obj.GetComponent<ItemObj>().amount = 1;
		}
	}









	// play animations depending on enemies
	void playWalkingAnimation()
	{
		if(name == "Skeleton Warrior")
		{
			animations.CrossFade("run");
		}
		if(name == "Zombie")
		{
			animations.CrossFade("walk");
		}
	}

	void playAttackAnimation()
	{
		if(name == "Skeleton Warrior" || name == "Zombie")
		{
			animations.CrossFade("attack");
		}
	}

	void playDeadAnimation()
	{
		if(name == "Skeleton Warrior")
		{
			animations.CrossFade("die");
		}
		if(name == "Zombie")
		{
			animations.CrossFade("back_fall");
		}
	}

	void playIdleAnimation()
	{
		if(name == "Skeleton Warrior")
		{
			animations.CrossFade("waitingforbattle");
		}
	}
}
