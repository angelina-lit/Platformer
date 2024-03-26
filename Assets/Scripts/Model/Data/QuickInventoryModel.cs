using Assets.Scripts.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickInventoryModel
{
	private readonly PlayerData _data;

	public InventoryItemData[] Inventory {  get; private set; }

	public readonly IntProperty SelectedIndex = new IntProperty();

	public QuickInventoryModel(PlayerData data)
	{
		_data = data;

		Inventory = _data.Inventory.GetAll();
		_data.Inventory.OnChanged += OnChanged;
	}

	private void OnChanged(string id, int value)
	{
		SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
	}
}
