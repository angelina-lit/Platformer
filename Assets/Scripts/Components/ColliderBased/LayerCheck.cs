using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LayerCheck : MonoBehaviour
{
	[SerializeField] protected LayerMask _layer;
	[SerializeField] protected bool _isTouchingLayer;

	public bool IsTouchingLayer => _isTouchingLayer;
}
