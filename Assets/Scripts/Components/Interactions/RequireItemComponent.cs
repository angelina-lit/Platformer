using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RequireItemComponent : MonoBehaviour
{
    [SerializeField] private InventoryItemData[] _required;
    [SerializeField] private bool _removeAfterUse;

    [SerializeField] private UnityEvent _onSuccess;
    [SerializeField] private UnityEvent _onFail;

    public void Check()
    {
        var session = FindObjectOfType<GameSession>();
        var areAllRequirementMet = true;
        foreach (var item in _required)
        {
			var numItems = session.Data.Inventory.Count(item.Id);
            if (numItems < item.Value)
                areAllRequirementMet = false;
		}
        if (areAllRequirementMet)
        {
            if (_removeAfterUse)
            {
				foreach (var item in _required)
					session.Data.Inventory.Remove(item.Id, item.Value);
			}

            _onSuccess?.Invoke();
        }
        else
        {
            _onFail?.Invoke();
        }
    }
}
