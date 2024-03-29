using UnityEngine;

public class ShootingTrapAI : MonoBehaviour
{
    [SerializeField] public ColliderCheck _vision;
    [SerializeField] private CoolDown _cooldown;
    [SerializeField] private SpriteAnimation _animation;

	private void Update()
	{
		if (_vision.IsTouchingLayer && _cooldown.IsReady)
		{
			Shoot();
		}
	}

	public void Shoot()
	{
		_cooldown.Reset();
		_animation.SetClip("start-attack");
	}
}
