using UnityEngine;

public class HudController : MonoBehaviour
{
	[SerializeField] private ProgressBarWidget _healthBar;

	private GameSession _session;

	private void Start()
	{
		_session = FindObjectOfType<GameSession>();
		_session.Data.Hp.OnChanged += OnHealthChanged;

		OnHealthChanged(_session.Data.Hp.Value, 0);
	}

	private void OnHealthChanged(int newValue, int oldValue)
	{
		var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
		var value = (float) newValue / maxHealth;
		_healthBar.SetProgress(value);
	}

	public void OnSettings()
	{
		WindowUtils.CreateWindow("UI/InGameMenuWindow");
	}

	public void OnDebug()
	{
		WindowUtils.CreateWindow("UI/PlayerStatsWindow");
	}

	private void OnDestroy()
	{
		_session.Data.Hp.OnChanged -= OnHealthChanged;
	}
}
