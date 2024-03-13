using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
	public class PlatformPatrol : Patrol
	{
		[SerializeField] private LayerCheck _groundCheck;
		[SerializeField] private LayerCheck _obstrcleCheck;
		[SerializeField] private int _direction;
		[SerializeField] private Creature _creature;


		public override IEnumerator DoPatrol()
		{
			while (enabled)
			{
				if (_groundCheck.IsTouchingLayer && !_obstrcleCheck.IsTouchingLayer)
				{
					_creature.SetDirection(new Vector2(_direction, 0));
				}
				else
				{
					_direction = -_direction;
					_creature.SetDirection(new Vector2(_direction, 0));
				}

				yield return null;
			}
		}
	}
}
