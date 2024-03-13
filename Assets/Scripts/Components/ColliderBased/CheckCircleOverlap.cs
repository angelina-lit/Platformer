using Assets.Scripts.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	public class CheckCircleOverlap : MonoBehaviour
	{
		[SerializeField] private float _radius = 1f;
		[SerializeField] private LayerMask _mask;
		[SerializeField] private string[] _tags;
		[SerializeField] private OnOverlapEvent _onOverlap;

		private Collider2D[] _intaractionResult = new Collider2D[10];

		private void OnDrawGizmosSelected()
		{
			Handles.color = HandlesUtils.TransparentRed;
			Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
		}

		public void Check()
		{
			var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _intaractionResult, _mask);

			for (var i = 0; i < size; i++)
			{
				var overlapResult = _intaractionResult[i];
				var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
				if (isInTags) _onOverlap?.Invoke(_intaractionResult[i].gameObject);
			}
		}

		[Serializable]
		public class OnOverlapEvent : UnityEvent<GameObject>
		{
		}
	}
}