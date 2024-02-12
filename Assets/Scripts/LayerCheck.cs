using UnityEngine;

namespace Assets.Scripts
{
	public class LayerCheck : MonoBehaviour
	{
		[SerializeField] private LayerMask _layer;
		[SerializeField] private bool _isTouchingLayer;
		private Collider2D _collider;

		public bool IsTouchingLayer => _isTouchingLayer;

		//public LayerMask CheckLayer { get => _layer; }

		private void Awake()
		{
			_collider = GetComponent<Collider2D>();
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			_isTouchingLayer = _collider.IsTouchingLayers(_layer);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			_isTouchingLayer = _collider.IsTouchingLayers(_layer);
		}
	}
}