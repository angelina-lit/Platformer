using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCheck : MonoBehaviour
{
	[SerializeField] private LayerMask _checkLayer;
	[SerializeField] private bool _isTouchingLayer;
	public bool IsTouchingLayer => _isTouchingLayer;

	private Collider2D _collider;

	public LayerMask CheckLayer { get => _checkLayer; }

	private void Awake()
	{
		_collider = GetComponent<Collider2D>();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		_isTouchingLayer = _collider.IsTouchingLayers(_checkLayer);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_isTouchingLayer = _collider.IsTouchingLayers(_checkLayer);
	}
}
