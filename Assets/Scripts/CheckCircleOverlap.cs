using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckCircleOverlap : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;

    private Collider2D[] _intaractionResult = new Collider2D[5];

    public GameObject[] GetObjecktsInRange()
    {
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _intaractionResult);

        var overlaps = new List<GameObject>();
        for (var i = 0; i < size; i++)
        {
            overlaps.Add(_intaractionResult[i].gameObject);
        }

        return overlaps.ToArray();
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = HandlesUtils.TransparentRed;
        Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
    }
}
