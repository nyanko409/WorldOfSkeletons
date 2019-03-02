using UnityEngine;
using System.Collections;

public class ItemObj : MonoBehaviour 
{
	public Item item;
	public int amount;
	public float pickUpRadius;

	private GameObject itemNameText;
	private GameObject player;
	private Inventory inventory;

	void Start()
	{
		name = item.itemName;
		player = GameObject.FindGameObjectWithTag ("Player");
		inventory = player.GetComponent<Inventory> ();
		itemNameText = transform.GetChild (0).gameObject;
		itemNameText.SetActive (false);
	}

	void Update()
	{
		
	}

	void OnMouseOver()
	{
		itemNameText.SetActive (true);
		itemNameText.transform.position = Camera.main.WorldToViewportPoint (new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
		itemNameText.GetComponent<GUIText> ().text = name;
	}

	void OnMouseExit()
	{
		itemNameText.SetActive (false);
	}


	// insert the item into inventory if in range
	void OnMouseDown()
	{
		pickUp();
	}

	void pickUp()
	{
		if (!PlayerStats.isDead && !inventory.checkIfInventoryIsFull())
		{
			if (distanceToItem() <= pickUpRadius)
			{
				for (int i=0; i<amount; i++)
				{
					player.GetComponent<Inventory> ().addItemToSlot (item);
				}
				Destroy (gameObject);
			}
		}
		else if(!PlayerStats.isDead && inventory.checkIfInventoryIsFull())  // play sound if inventory is full and in range
		{
			if(distanceToItem() <= pickUpRadius)
			{
				SoundEffects.audios[2].Play();
			}
		}
	}

	float distanceToItem()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			return Vector3.Distance(player.transform.position, transform.position);
		}
		return 1000;
	}
}
