using Assets.Scripts.Components;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
	public class Hero : Creature, ICanAddInInventory
	{
		[SerializeField] private CheckCircleOverlap _intaractionCheck;
		[SerializeField] private LayerCheck _wallCheck;

		[SerializeField] private float _slamDownVelocity;
		[SerializeField] private CoolDown _throwCooldown;
		[SerializeField] private AnimatorController _armed;
		[SerializeField] private AnimatorController _disarmed;

		[Header("Super throw")]
		[SerializeField] private CoolDown _superThrowCoolDown;

		[SerializeField] private int _superThrowParticles;
		[SerializeField] private float _superThrowDelay;
		[SerializeField] private ProbabilityDropComponent _hitDrop;
		[SerializeField] private SpawnComponent _throwSpawner;

		private static readonly int ThrowKey = Animator.StringToHash("throw");
		private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

		private bool _allowDoubleJump;
		private bool _isOnWall;
		private bool _superThrow;

		private GameSession _session;
		private HealthComponent _health;
		private float _defaultGravityScale;

		private const string SwordId = "Sword";
		private int CoinsCount => _session.Data.Inventory.Count("Coin");
		private int SwordCount => _session.Data.Inventory.Count(SwordId);
		private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
		private bool CanThrow
		{
			get
			{
				if (SelectedItemId == SwordId)
					return SwordCount > 1;

				var def = DefsFacade.I.Items.Get(SelectedItemId);
				return def.HasTag(ItemTag.Throwable);
			}
		}

		protected override void Awake()
		{
			base.Awake();

			_defaultGravityScale = Rigidbody.gravityScale;
		}

		private void Start()
		{
			_session = FindObjectOfType<GameSession>();
			_health = GetComponent<HealthComponent>();
			_session.Data.Inventory.OnChanged += OnInventoryChanged;

			_health.SetHealth(_session.Data.Hp.Value);
			UpdateHeroWeapon();
		}

		private void OnDestroy()
		{
			_session.Data.Inventory.OnChanged -= OnInventoryChanged;
		}

		private void OnInventoryChanged(string id, int value)
		{
			if (id == SwordId)
				UpdateHeroWeapon();
		}

		public void OnHealthChanged(int currentHealth)
		{
			_session.Data.Hp.Value = currentHealth;
		}

		protected override void Update()
		{
			base.Update();

			var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
			if (_wallCheck.IsTouchingLayer && moveToSameDirection)
			{
				_isOnWall = true;
				Rigidbody.gravityScale = 0;
			}
			else
			{
				_isOnWall = false;
				Rigidbody.gravityScale = _defaultGravityScale;
			}

			Animator.SetBool(IsOnWall, _isOnWall);
		}

		protected override float CalculateYVelocity()
		{
			var isJumpPressing = Direction.y > 0;

			if (IsGrounded || _isOnWall) _allowDoubleJump = true;

			if (!isJumpPressing && _isOnWall) return 0f;

			return base.CalculateYVelocity();
		}

		protected override float CalculateJumpVelocity(float yVelocity)
		{
			if (!IsGrounded && _allowDoubleJump && !_isOnWall)
			{
				_allowDoubleJump = false;
				DoJumpVfx();
				return _jumpSpeed;
			}

			return base.CalculateJumpVelocity(yVelocity);
		}

		public void AddInInventory(string id, int value)
		{
			_session.Data.Inventory.Add(id, value);
		}

		public override void TakeDamage()
		{
			base.TakeDamage();
			if (CoinsCount > 0)
			{
				SpawnCoins();
			}
		}

		public void SpawnCoins()
		{
			var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
			_session.Data.Inventory.Remove("Coin", numCoinsToDispose);

			_hitDrop.SetCount(numCoinsToDispose);
			_hitDrop.CalculateDrop();
		}

		public void Interact()
		{
			_intaractionCheck.Check();
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.IsInLayer(_groundLayer))
			{
				var contact = other.contacts[0];
				if (contact.relativeVelocity.y >= _slamDownVelocity)
				{
					_particles.Spawn("SlamDown");
				}
			}
		}

		public override void Attack()
		{
			if (SwordCount <= 0) return;

			base.Attack();
		}

		private void UpdateHeroWeapon()
		{
			Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed; //����� �������� ������ if|else
		}

		public void OnDoThrow()
		{
			if (_superThrow)
			{
				var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
				var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
				
				var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
				StartCoroutine(DoSuperThrow(numThrows));
			}
			else
			{
				ThrowAndRemoveFromInventory();
			}

			_superThrow = false;
		}

		private IEnumerator DoSuperThrow(int numThrows)
		{
			for (int i = 0; i < numThrows; i++)
			{
				ThrowAndRemoveFromInventory();
				yield return new WaitForSeconds(_superThrowDelay);
			}
		}

		private void ThrowAndRemoveFromInventory()
		{
			Sounds.Play("Range");
			
			var throwableId = _session.QuickInventory.SelectedItem.Id;
			var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
			_throwSpawner.SetPrefab(throwableDef.Projectile);
			_throwSpawner.Spawn();

			_session.Data.Inventory.Remove(throwableId, 1);
		}

		public void StartThrowing()
		{
			_superThrowCoolDown.Reset();
		}

		public void PerformThrowing()
		{
			if (!_throwCooldown.IsReady || !CanThrow) return;

			if (_superThrowCoolDown.IsReady) _superThrow = true;

			Animator.SetTrigger(ThrowKey);
			_throwCooldown.Reset();
		}

		internal void NextItem()
		{
			_session.QuickInventory.SetNextItem();
		}
	}
}
