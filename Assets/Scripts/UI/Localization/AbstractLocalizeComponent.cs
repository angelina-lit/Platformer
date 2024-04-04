using UnityEngine;

public abstract class AbstractLocalizeComponent : MonoBehaviour
{
	protected virtual void Awake()
	{
		LocalizationManager.I.OnLocaleChanged += OnLocaleChanged;
		Localize();
	}

	private void OnLocaleChanged()
	{
		Localize();
	}

	protected abstract void Localize();

	private void OnDestroy()
	{
		LocalizationManager.I.OnLocaleChanged -= OnLocaleChanged;
	}
}
