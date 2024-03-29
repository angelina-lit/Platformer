using UnityEngine;

public class InventoryAddComponent : MonoBehaviour
{
	[InventoryId] [SerializeField] private string _id;
	[SerializeField] private int _count;

	public void Add(GameObject go)
	{
		var hero = go.GetInterface<ICanAddInInventory>();
		hero?.AddInInventory(_id, _count);
	}
}
