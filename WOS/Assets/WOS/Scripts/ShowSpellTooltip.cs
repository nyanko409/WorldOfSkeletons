using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ShowSpellTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public string spellName;
	public string description;
	private SkillTreeMage skillTreeMage;

	// Use this for initialization
	void Start () 
	{
		
	}

	public void OnPointerEnter(PointerEventData data)
	{
		GameObject.FindWithTag("Player").GetComponent<SkillTreeMage>().showTooltip(spellName, description, transform.position);
	}

	public void OnPointerClick(PointerEventData data)
	{
		GameObject.FindWithTag("Player").GetComponent<SkillTreeMage>().showTooltip(spellName, description, transform.position);
	}

	public void OnPointerExit(PointerEventData data)
	{
		GameObject.FindWithTag("Player").GetComponent<SkillTreeMage>().hideTooltip();
	}
}
