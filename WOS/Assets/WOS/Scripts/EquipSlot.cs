using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Item item;
	private GameObject itemSlot;  // the gameobject displaying icon sprites
	public GameObject tooltip; // tooltip gameobject

	public Item.ItemType thisItemtype;
	public Item.ItemType thisSecondItemtype;

	private GameObject weaponSlot1; // check if the weapon/shield is equipable depending on two hand / one hand + shield / 2x one hand
	private GameObject weaponSlot2;

	// Use this for initialization
	void Start () 
	{
		itemSlot = transform.Find ("ItemIcon").gameObject;

		weaponSlot1 = GameObject.Find ("Weapon Slot 1");
		weaponSlot2 = GameObject.Find ("Weapon Slot 2");
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
		}
	}

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

	bool checkIfEquipableWeapon(Item draggingItem)
	{
		if (draggingItem.itemType == Item.ItemType.TwohandWeapon)
		{
			if (weaponSlot1.GetComponent<EquipSlot> ().item != null || weaponSlot2.GetComponent<EquipSlot> ().item != null)
			{
				return false;
			}
		}
		else if (draggingItem.itemType == Item.ItemType.OnehandWeapon_Shield)
		{
			if((weaponSlot1.GetComponent<EquipSlot> ().item != null && weaponSlot1.GetComponent<EquipSlot> ().item.itemType == Item.ItemType.TwohandWeapon) ||  
			   (weaponSlot2.GetComponent<EquipSlot> ().item != null && weaponSlot2.GetComponent<EquipSlot> ().item.itemType == Item.ItemType.TwohandWeapon))
			{
				return false;
			}
		}
		return true;
	}

	public void OnPointerClick(PointerEventData data)
	{
		// left click
		if (data.button == PointerEventData.InputButton.Left)
		{
			// if it is the weapon slot, check for 2hand, one hand and shield etc.
			if(name == "Weapon Slot 1" || name == "Weapon Slot 2")
			{
				// item is now following the mouse
				if (item != null && !Slot.isDraggingItem)
				{
					GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = item;
					tooltip.SetActive (false);
					item = null;
					Slot.isDraggingItem = true;
					SoundEffects.audios[8].Play(); 
				}
				// drop the dragging item on this empty slot only if it is the same item type
				else if (item == null && Slot.isDraggingItem && (GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemType == thisItemtype || 
				                                                 GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemType == thisSecondItemtype))
				{
					if(checkIfEquipableWeapon(GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem))
					{
						Slot.isDraggingItem = false;
						item = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem;
						GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = null;
						tooltip.SetActive (true);
						updateTooltip ();
						SoundEffects.audios[8].Play(); 
					}
					else
					{
						SoundEffects.audios[7].Play();
					}
				}
				//switch the item on the mouse with the one in the slot if it is the same item type
				else if (item != null && Slot.isDraggingItem && (GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemType == thisItemtype || 
				                                                 GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemType == thisSecondItemtype))
				{
					if(checkIfEquipableWeapon(GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem))
					{
						Item temp = item;
						item = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem;
						GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = temp;
						SoundEffects.audios[8].Play(); 
					}
					else
					{
						SoundEffects.audios[7].Play();
					}
				}
				// play sound if i cant equip it
				else if(Slot.isDraggingItem && (GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemType != Item.ItemType.OnehandWeapon_Shield ||
				        GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemType != Item.ItemType.TwohandWeapon))
				{
					SoundEffects.audios[7].Play();
				}
			}
			else
			{
				// item is now following the mouse
				if (item != null && !Slot.isDraggingItem)
				{
					GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = item;
					tooltip.SetActive (false);
					item = null;
					Slot.isDraggingItem = true;
					SoundEffects.audios[8].Play(); 
				}
				// drop the dragging item on this empty slot only if it is the same item type
				else if (item == null && Slot.isDraggingItem && (GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemType == thisItemtype))
				{
					Slot.isDraggingItem = false;
					item = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem;
					GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = null;
					tooltip.SetActive (true);
					updateTooltip ();
					SoundEffects.audios[8].Play(); 
				}
				//switch the item on the mouse with the one in the slot if it is the same item type
				else if (item != null && Slot.isDraggingItem && (GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem.itemType == thisItemtype))
				{
					Item temp = item;
					item = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem;
					GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ().draggingItem = temp;
					SoundEffects.audios[8].Play(); 
				}
				else if(Slot.isDraggingItem)
				{
					SoundEffects.audios[7].Play();
				}
			}
		}
	}
}
