using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instance = SpawnUtils.Spawn(_prefab, _target.position);
            instance.transform.localScale = _target.lossyScale;
            instance.SetActive(true);
        }

		public void SetPrefab(GameObject prefab)
		{
			_prefab = prefab;
		}
	}
}
