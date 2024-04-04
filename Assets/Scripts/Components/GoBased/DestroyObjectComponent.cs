using UnityEngine;

public class DestroyObjectComponent : MonoBehaviour
{
	[SerializeField] private GameObject _objectToDestroy;
	[SerializeField] private RestoreStateComponent _state;

	public void DestroyObject()
	{
		Destroy(_objectToDestroy);
		if (_state != null)
			FindObjectOfType<GameSession>().StoreState(_state.Id);
	}
}
