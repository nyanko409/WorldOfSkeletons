using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IPointerClickHandler
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerClick(PointerEventData data)
	{
		if (data.button == PointerEventData.InputButton.Left)
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().SendMessage("dropDraggingItemOnGround");
		}
	}
}
