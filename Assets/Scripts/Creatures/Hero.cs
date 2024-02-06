using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Hero : Creature
{
	[SerializeField] private float _intaractionRadius;
	//[SerializeField] private int _damageVelocity; реализовать урон от падения (2.2.49 мин)

	[SerializeField] private LayerMask _intaractionLayer;
	[SerializeField] private AnimatorController _armed;
	[SerializeField] private AnimatorController _disarmed;

	[SerializeField] private bool _doubleJumpForbidden;

	[Space]
	[Header("Particles")]
	[SerializeField] private ParticleSystem _hitParticles;

	private Collider2D[] _intaractionResult = new Collider2D[1];
	private GameSession _session;

	private bool _allowDoubleJump;
	private bool _isOnWall;

	protected override void Awake()
	{
		base.Awake();
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
	}

	protected override float CalculateYVelocity()
	{
		var isJumpPressing = _direction.y > 0;

		if (IsGrounded) _allowDoubleJump = true;

		if (!isJumpPressing && _isOnWall)
		{
			return 0f;
		}

		return base.CalculateYVelocity();
	}

	//protected override float CalculateYVelocity()
	//{
	//	var isJumpPressing = _direction.y > 0;

	//	if (IsGrounded && !_doubleJumpForbidden || _isOnWall)
	//	{
	//		_allowDoubleJump = true;
	//	}

	//	if (!isJumpPressing && _isOnWall)
	//	{
	//		return 0f;
	//	}
	//	return base.CalculateYVelocity();
	//}

	// protected override float CalculateJumpVelocity(float yVelocity)
	// {
	//     if (!_isGrounded || _allowDoubleJump)
	//     {
	//_particles.Spawn("Jump");
	//         _allowDoubleJump = false;
	//         return _jumpSpeed;
	//     }

	//     return base.CalculateJumpVelocity(yVelocity);
	// }

	protected override float CalculateJumpVelocity(float yVelocity)
	{
		if (!IsGrounded && _allowDoubleJump && !_doubleJumpForbidden && !_isOnWall)
		{
			//Sounds.Play("Jump");
			//_particles.Spawn("Jump");
			_allowDoubleJump = false;

			return _jumpForce;
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
		var size = Physics2D.OverlapCircleNonAlloc(transform.position, _intaractionRadius, _intaractionResult, _intaractionLayer);

		for (int i = 0; i < size; i++)
		{
			var interactable = _intaractionResult[i].GetComponent<InteractableComponent>();

			if (interactable != null)
			{
				interactable.Interact();
			}
		}
	}


	//private void OnCollisionEnter2D(Collision2D other)
	//{
	//	if (other.gameObject.IsInLayer(_groundLayer))
	//	{
	//		var contact = other.contacts[0];
	//		if (contact.relativeVelocity.y >= _slamDownVelocity)
	//		{
	//			_particles.Spawn("SlamDown");
	//		}
	//	}
	//}

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
		_animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed; //более короткая запись if|else
	}
}
