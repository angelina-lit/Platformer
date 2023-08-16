using TMPro;
using UnityEditor;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _damageJumpSpeed;
    [SerializeField] private LayerCheck _groundCheck;
    [SerializeField] private float _intaractionRadius;
    [SerializeField] private LayerMask _intaractionLayer;
    [SerializeField] private SpawnComponent _footStepParticle;
    [SerializeField] private ParticleSystem _hitParticles;
    [SerializeField] private int _coins;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Animator _animator;
    private bool _isGrounded;
    private bool _allowDoubleJump;
    private Collider2D[] _intaractionResult = new Collider2D[1];
    

    private static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
    private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
    private static readonly int Hit = Animator.StringToHash("hit");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {

        var xVelocity = _direction.x * _speed;
        var yVelocity = CalculateYVelocity();
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        _animator.SetBool(IsRunning, _direction.x != 0);
        _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);
        _animator.SetBool(IsGroundKey, _isGrounded);

        UpdateSpriteDirection();
    }

    private float CalculateYVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
        var isJumpPressing = _direction.y > 0;

        if (_isGrounded) _allowDoubleJump = true;

        if (isJumpPressing)
        {
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_rigidbody.velocity.y > 0)
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        var isFalling = _rigidbody.velocity.y <= 0.001f;
        if (!isFalling) return yVelocity;

        if (_isGrounded)
        {
            yVelocity += _jumpSpeed;
        }
        else if (_allowDoubleJump)
        {
            yVelocity = _jumpSpeed;
            _allowDoubleJump = false;
        }
        return yVelocity;
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private bool IsGrounded()
    {
        return _groundCheck.isTouchingLayer;
    }

    public void SaySomething()
    {
        Debug.Log(message: "Something!");
    }

    public void AddCoins(int coinAmount)
    {
        _coins += coinAmount;
        Debug.Log($"{coinAmount} coins added. Total coins: {_coins}");
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(Hit);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

        if (_coins > 0)
        {
            SpawnCoins();
        }
    }

    public void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(_coins, 5);
        _coins -= numCoinsToDispose;

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

    public void SpawnFootDust()
    {
        _footStepParticle.Spawn();
    }
}
