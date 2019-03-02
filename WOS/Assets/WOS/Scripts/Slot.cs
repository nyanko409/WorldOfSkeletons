using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Item item; // the item on this slot
	private GameObject itemSlot; // the gameobject displaying icon sprites
	private Text stackText; // display current amount if item can be stacked
	public int currentStack; // current stacksize
	public GameObject tooltip; // tooltip gameobject

	public static bool isDraggingItem; // is true when currently dragging an item, must be static

	// Use this for initialization
	void Start () 
	{
		itemSlot = transform.Find ("ItemIcon").gameObject;
		stackText = transform.Find ("Amount").GetComponent<Text> ();
		stackText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//set the sprite 
		if (item != null)
		{
			itemSlot.GetComponent<Image> ().sprite = item.itemIcon;
		}
		else
		{
			// no item is on the slot
			itemSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("SlotBackground");
			stackText.enabled = false;
			currentStack = 0;
		}

		// show/hide stacktext
		if (item != null)
		{
			if(item.stackSize > 1)
			{
				//item is stackable, show stack text
				stackText.enabled = true;
				stackText.text = currentStack.ToString ();
			}
			else
			{
				stackText.enabled = false;
			}
		}
	}

	// called when mouse is over this slot // use it to show tooltip when an item is inside
	public void OnPointerEnter(PointerEventData data)
	{
		if (item != null)
		{
			tooltip.SetActive (true);
			updateTooltip();
		}
	}

	void updateTooltip()
	{
		tooltip.transform.position = new Vector3(transform.position.x - 240, transform.position.y, 0);
		tooltip.transform.Find("Item Name").GetComponent<Text>().text = item.itemName;

		if (item.itemType != Item.ItemType.TwohandWeapon && item.itemType != Item.ItemType.OnehandWeapon_Shield)
		{
			tooltip.transform.Find ("Item Info").GetComponent<Text> ().text = item.itemType.ToString ();
		} 
		else if (item.itemType == Item.ItemType.TwohandWeapon)
		{
			tooltip.transform.Find ("Item Info").GetComponent<Text> ().text = "Twohand Weapon";
		}
		else if (item.itemType == Item.ItemType.OnehandWeapon_Shield && item.itemDamage > 0)
		{
			tooltip.transform.Find ("Item Info").GetComponent<Text> ().text = "Onehand Weapon";
		}
		else
		{
			tooltip.transform.Find ("Item Info").GetComponent<Text> ().text = "Shield";
		}

		tooltip.transform.Find ("Item Desc").GetComponent<Text> ().text = "";
		if(item.itemDamage > 0)
			tooltip.transform.Find("Item Desc").GetComponent<Text>().text += "Damage:  " + item.itemDamage.ToString();
		if (item.itemArmor > 0)
			tooltip.transform.Find ("Item Desc").GetComponent<Text> ().text += "\nArmor:  " + item.itemArmor.ToString ();
		tooltip.transform.Find ("Item Desc").GetComponent<Text> ().text += "\n\n" + item.itemDesc;
	}

	// mouse exits the gameobject // hide tooltip
	public void OnPointerExit(PointerEventData data)
	{
		tooltip.SetActive (false);
	}

	//called when the gameobject is clicked // use it to drag/drop items
	public void OnPointerClick(PointerEventData data)
	{
		// left click
		if (data.button == PointerEventData.InputButton.Left)
		{
			// item is now following the mouse
			if (item != null && !isDraggingItem)
			{
				GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = item;
				GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount = currentStack;
				tooltip.SetActive (false);
				item = null;
				isDraggingItem = true;
				SoundEffects.audios[8].Play(); 
			}
			// drop the dragging item on this empty slot
			else if (item == null && isDraggingItem)
			{
				isDraggingItem = false;
				item = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem;
				currentStack = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount;
				GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = null;
				tooltip.SetActive (true);
				updateTooltip ();
				SoundEffects.audios[8].Play(); 
			}
			//switch the item on the mouse with the one in the slot
			else if (item != null && isDraggingItem)
			{
				// if it is not the same item id
				if ((item.itemID != GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemID) ||
					(item.stackSize == 1 && GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.stackSize == 1))
				{
					Item temp = item;
					int amount = currentStack;
					item = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem;
					currentStack = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount;
					GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = temp;
					GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount = amount;
					SoundEffects.audios[8].Play(); 
				}
				else if(item.stackSize == 1 && GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.stackSize == 1)
				{
					
				}
				//check if stackable
				else if (item.itemID == GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemID && (currentStack + GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount) <= item.stackSize)
				{
					// if the id is the same and the max stack doenst exceed, stack the item
					currentStack += GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount;
					isDraggingItem = false;
					SoundEffects.audios[8].Play(); 
				}
				//if item is the same and max stacks will exceed
				else if (item.itemID == GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemID && (currentStack + GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount) > item.stackSize && item.stackSize != 1)
				{
					while (currentStack < 20 && GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount > 0)
					{
						currentStack++;
						GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount--;
					}
					if (GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItemAmount == 0)
					{
						GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = null;
						isDraggingItem = false;
					}
					SoundEffects.audios[8].Play(); 
				}
			}
		}
		// right click, use consumable if possible
		else if (item != null && data.button == PointerEventData.InputButton.Right && item.itemType == Item.ItemType.Consumable)
		{
			// use consumable and delete the item if none is left
			if(item.itemID == 1) // heal potion 50% heal
			{
				if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().healPlayerByPercentage(50))
				{
					currentStack--;
					SoundEffects.audios[0].Play(); // heal potion sound
				}
				else
				{
					SoundEffects.audios[6].Play(); // dont need to use it sound
				}
			}
			// delete if none left
			if(currentStack < 1)
			{
				currentStack = 0;
				item = null;
				tooltip.SetActive(false);
			}
		}
	}
}
