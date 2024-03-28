using UnityEngine;

public class SettingsWindow : AnimatedWindow
{
	[SerializeField] private AudioSettingsWidget _music;
	[SerializeField] private AudioSettingsWidget _sfx;

	protected override void Start()
	{
		base.Start();

		_music.SetModel(GameSettings.I.Music);
		_sfx.SetModel(GameSettings.I.Sfx);
	}
}
