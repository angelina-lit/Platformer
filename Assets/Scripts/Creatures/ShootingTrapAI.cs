using Assets.Scripts;
using Assets.Scripts.Components;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootingTrapAI : MonoBehaviour
{
	[SerializeField] private LayerCheck _vision;

	[Header("Melle")]
	[SerializeField] private CoolDown _melleCoolDown;
	[SerializeField] private CheckCircleOverlap _melleAttack;
	[SerializeField] private LayerCheck _melleCanAttack;

	[Header("Range")]
	[SerializeField] private CoolDown _rangeCoolDown;
	[SerializeField] private SpawnComponent _rangeAttack;

	private static readonly int Mellee = Animator.StringToHash("mellee");
	private static readonly int Range = Animator.StringToHash("range");

	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (_vision.IsTouchingLayer)
		{
			if (_melleCanAttack.IsTouchingLayer)
			{
				if (_melleCoolDown.IsReady)
					MelleAttack();
				return;
			}

			if (_rangeCoolDown.IsReady)
				RangeAttack();
		}
	}

	private void RangeAttack()
	{
		_rangeCoolDown.Reset();
		_animator.SetTrigger(Range);
	}

	private void MelleAttack()
	{
		_melleCoolDown.Reset();
		_animator.SetTrigger(Mellee);
	}

	public void OnMelleeAttack()
	{
		_melleAttack.Check();
	}

	public void OnRangeAttack()
	{
		_rangeAttack.Spawn();
	}
}
