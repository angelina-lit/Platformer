using Assets.Scripts.Components;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
	public class Hero : Creature
	{
		[SerializeField] private CheckCircleOverlap _intaractionCheck;
		[SerializeField] private LayerCheck _wallCheck;

		[SerializeField] private float _slamDownVelocity;
		[SerializeField] private float _intaractionRadius;

		[SerializeField] private AnimatorController _armed;
		[SerializeField] private AnimatorController _disarmed;

		[Space]
		[Header("Particles")]
		[SerializeField] private ParticleSystem _hitParticles;

		private bool _allowDoubleJump;
		private bool _isOnWall;

		private GameSession _session;
		private float _defaultGravityScale;

		protected override void Awake()
		{
			base.Awake();

			_defaultGravityScale = Rigidbody.gravityScale;
		}

		private void Start()
		{
			_session = FindObjectOfType<GameSession>();
			var health = GetComponent<HealthComponent>();

			health.SetHealth(_session.Data.Hp);
			UpdateHeroWeapon();
		}

		public void OnHealthChanged(int currentHealth)
		{
			_session.Data.Hp = currentHealth;
		}

		protected override void Update()
		{
			base.Update();

			if (_wallCheck.IsTouchingLayer && Direction.x == transform.localScale.x)
			{
				_isOnWall = true;
				Rigidbody.gravityScale = 0;
			}
			else
			{
				_isOnWall = false;
				Rigidbody.gravityScale = _defaultGravityScale;
			}
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
			if (!IsGrounded && _allowDoubleJump)
			{
				_particles.Spawn("Jump");
				_allowDoubleJump = false;
				return _jumpSpeed;
			}

			return base.CalculateJumpVelocity(yVelocity);
		}

		public void AddCoins(int coinAmount)
		{
			_session.Data.Coins += coinAmount;
			Debug.Log($"{coinAmount} coins added. Total coins: {_session.Data.Coins}");
		}

		public override void TakeDamage()
		{
			base.TakeDamage();

			if (_session.Data.Coins > 0)
			{
				SpawnCoins();
			}
		}

		public void SpawnCoins()
		{
			var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
			_session.Data.Coins -= numCoinsToDispose;

			var burst = _hitParticles.emission.GetBurst(0);
			burst.count = numCoinsToDispose;
			_hitParticles.emission.SetBurst(0, burst);

			_hitParticles.gameObject.SetActive(true);
			_hitParticles.Play();
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

				//if (contact.relativeVelocity.y >= _damageVelocity)
				//{
				//	GetComponent<HealthComponent>().ModifyHealth(-1);
				//}
			}
		}

		public override void Attack()
		{
			if (!_session.Data.IsArmed) return;

			base.Attack();
		}

		public void ArmHero()
		{
			_session.Data.IsArmed = true;
			UpdateHeroWeapon();
		}

		private void UpdateHeroWeapon()
		{
			Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed; //����� �������� ������ if|else
		}
	}
}
