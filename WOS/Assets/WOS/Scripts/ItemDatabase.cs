using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour 
{
	public List<Item> items = new List<Item>();

	// Use this for initialization
	void Start () 
	{
		// PLACEHOLDER INFO

		// 0 - 49 == CONSUMABLES
		// 50 - 249 == CHEST
		// 250 - 449 == HEAD
		// 450 - 649 == SHOES
		// 650 - 849 == GLOVES
		// 850 - 1049 == ONEHAND WEAPON
		// 1050 - 1249 == SHIELD
		// 1250 - 1449 == TWOHAND WEAPON

		// item database // add new items here

		//				      --name-- 					 --id--   --description--  				--attack damage--  --armor-- 		 --item type--  				--max stack--	--sprite name--          								 --item model--
		// consumables
		items.Add(new Item(	"HP Potion", 					1, 		"Heals 50% HP", 					0, 		 	  0, 		 Item.ItemType.Consumable,  			 20, 	 	  "P_Red01",    				 Resources.Load<GameObject>("ItemModels/ItemModelPlaceholder")));
		// chest
		items.Add(new Item(	"Chest Armor", 					50, 	"Nice Armor", 						0, 		 	  15, 		 Item.ItemType.Chest,					 1, 		  "A_Armor04", 					 Resources.Load<GameObject>("ItemModels/ItemModelPlaceholder")));
		// head
		items.Add(new Item(	"epic Hat", 					250, 	"epic hat", 						0, 		 	  20, 		 Item.ItemType.Head, 					 1,	 		  "C_Hat02",		 			 Resources.Load<GameObject>("ItemModels/ItemModelPlaceholder")));
		// shoes
		items.Add(new Item(	"Boots", 						450, 	"hot boots", 						0, 		 	  6, 		 Item.ItemType.Shoes, 					 1,	 		  "A_Shoes01",		 			 Resources.Load<GameObject>("ItemModels/ItemModelPlaceholder")));
		// gloves
		items.Add(new Item(	"small gloves", 				650, 	"normal gloves", 					0, 		 	  2, 		 Item.ItemType.Gloves, 					 1,	 		  "Ac_Gloves01",				 Resources.Load<GameObject>("ItemModels/ItemModelPlaceholder")));
		// onehand weapons
		items.Add(new Item(	"Dagger", 						850, 		"stumpf", 						5, 		 	  0, 		 Item.ItemType.OnehandWeapon_Shield, 	 1,	 		  "W_Dagger003",				 Resources.Load<GameObject>("ItemModels/ItemModelPlaceholder")));
		// shields
		items.Add(new Item(	"Wooden Shield", 				1050, 		"wood", 						0, 		 	  100, 		 Item.ItemType.OnehandWeapon_Shield,  	 1,	 		  "E_Wood01",					 Resources.Load<GameObject>("ItemModels/ItemModelPlaceholder")));
		// twohand weapons
		items.Add(new Item(	"Katana", 						1250, 		"scharf", 						18, 		  0, 		 Item.ItemType.TwohandWeapon, 			 1,	 		  "W_Sword014",					 Resources.Load<GameObject>("ItemModels/ItemModelPlaceholder")));
	}

	public Item getItem(int itemID)
	{
		for (int i=0; i<items.Count; i++)
		{
			if(items[i].itemID == itemID)
			{
				return items[i];
			}
		}
		return null;
	}

	public Item getRandomItem()
	{
		int temp = -1;

		while(getItem(temp) == null) // loop until an item is found
		{
			temp = Random.Range(1, 1550); 
		}
		return getItem(temp);
	}
}
