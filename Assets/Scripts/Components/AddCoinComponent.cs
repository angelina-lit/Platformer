using Assets.Scripts.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Components
{
    public class AddCoinComponent : MonoBehaviour
    {
        [SerializeField] private int coinAmount;

        public void AddCoins(GameObject target)
        {
            var hero = target.GetComponent<Hero>();
            if (hero != null)
                hero.AddCoins(coinAmount);
        }
    }
}
