using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;
		[SerializeField] private HealthChangeEvent _onChange;

		public void ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return;

            _health += healthDelta;
            _onChange?.Invoke(_health);

            if (healthDelta < 0) _onDamage?.Invoke();

            if (healthDelta > 0) _onHeal?.Invoke();

            if (_health <= 0) _onDie?.Invoke();
        }

        [ContextMenu("Update Health")]
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }

        public void SetHealth(int health)
        {
            _health = health;
        }

		[Serializable]
		public class HealthChangeEvent : UnityEvent<int>
		{
		}
	}
}