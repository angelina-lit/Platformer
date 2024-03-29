using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Model
{
	public class GameSession : MonoBehaviour
	{
		[SerializeField] private PlayerData _data;
		[SerializeField] private string _defaultCheckPoint;

		public PlayerData Data => _data;
		private PlayerData _save;

		private readonly CompositeDisposable _trash = new CompositeDisposable();
		public QuickInventoryModel QuickInventory { get; private set; }

		private readonly List<string> _checkPoints = new List<string>();

		private void Awake()
		{
			var existsSession = GetExistsSession();


			if (existsSession != null)
			{
				existsSession.StartSession(_defaultCheckPoint);
				DestroyImmediate(gameObject);
			}
			else
			{
				Save();
				InitModels();
				DontDestroyOnLoad(this);
				StartSession(_defaultCheckPoint);
			}
		}

		private void StartSession(string defaultCheckPoint)
		{
			SetChecked(defaultCheckPoint);

			LoadHud();
			SpawnHero();
		}

		private void SpawnHero()
		{
			var checkPoints = FindObjectsOfType<CheckPointComponent>();
			var lastCheckPoint = _checkPoints.Last();

			foreach (var checkPoint in checkPoints)
			{
				if (checkPoint.Id == lastCheckPoint)
				{
					checkPoint.SpawnHero();
					break;
				}

			}
		}

		private void InitModels()
		{
			QuickInventory = new QuickInventoryModel(_data);
			_trash.Retain(QuickInventory);
		}

		private void LoadHud()
		{
			SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
		}

		private GameSession GetExistsSession()
		{
			var sessions = FindObjectsOfType<GameSession>();
			foreach (var gameSession in sessions)
			{
				if (gameSession != this)
					return gameSession;
			}
			return null;
		}

		public void Save()
		{
			_save = _data.Clone();
		}

		public void LoadLastSave()
		{
			_data = _save.Clone();

			_trash.Dispose();
			InitModels();
		}

		public bool IsChecked(string id)
		{
			return _checkPoints.Contains(id);
		}

		public void SetChecked(string id)
		{
			if (!_checkPoints.Contains(id))
			{
				Save();
				_checkPoints.Add(id);
			}
		}

		private void OnDestroy()
		{
			_trash.Dispose();
		}
	}
}
