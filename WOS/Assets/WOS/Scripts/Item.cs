using UnityEngine;
using System.Collections;

public class Item 
{
	public string itemName;
	public int itemID;
	public string itemDesc;
	public Sprite itemIcon;
	public GameObject itemModel;
	public int itemDamage;
	public int itemArmor;
	public ItemType itemType;
	public int stackSize;

	public enum ItemType
	{
		Consumable,
		OnehandWeapon_Shield,
		TwohandWeapon,
		Head,
		Shoes,
		Chest,
		Gloves
	}

	public Item(string name, int id, string desc, int damage, int armor, ItemType type, int stackSize, string spriteName, GameObject itemModel)
	{
		itemName = name;
		itemID = id;
		itemDesc = desc;
		itemDamage = damage;
		itemArmor = armor;
		itemType = type;
		this.stackSize = stackSize;
		this.itemModel = itemModel;
		itemIcon = Resources.Load<Sprite> ("ItemIcons/" + spriteName);
	}

	public Item()
	{

	}
}
