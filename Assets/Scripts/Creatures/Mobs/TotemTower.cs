using Assets.Scripts.Components;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TotemTower : MonoBehaviour
{
	[SerializeField] private List<ShootingTrapAI> _traps;
	[SerializeField] private CoolDown _coolDown;

	private int _currentTrap;

	private void Start()
	{
		foreach (var shootingTrapAI in _traps)
		{
			shootingTrapAI.enabled = false;
			var hp = shootingTrapAI.GetComponent<HealthComponent>();
			hp._onDie.AddListener( () => OnTrapDead(shootingTrapAI));
		}
	}

	private void OnTrapDead(ShootingTrapAI shootingTrapAI)
	{
		var index = _traps.IndexOf(shootingTrapAI);
		_traps.Remove(shootingTrapAI);
		if (index < _currentTrap)
		{
			_currentTrap--;
		}
	}

	private void Update()
	{
		if (_traps.Count == 0)
		{
			enabled = false;
			Destroy(gameObject, 1f);
		}

		var hasAnyTarget = _traps.Any(x => x._vision.IsTouchingLayer);
		if (hasAnyTarget)
		{
			if (_coolDown.IsReady)
			{
				_traps[_currentTrap].Shoot();
				_coolDown.Reset();
				_currentTrap = (int)Mathf.Repeat(_currentTrap + 1, _traps.Count);
			}
		}
	}
}
