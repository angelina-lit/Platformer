using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkWidget : MonoBehaviour, IItemRenderer<string>
{
	[SerializeField] private GameObject _icon;
	[SerializeField] private GameObject _isLocked;
	[SerializeField] private GameObject _isUsed;
	[SerializeField] private GameObject _isSelected;

	public void SetData(string data, int index)
	{
		
	}

	public void OnSelect()
	{
		
	}
}
