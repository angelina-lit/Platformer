using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _targer;
    [SerializeField] private float _damping;

    private void LateUpdate()
    {
        var destination = new Vector3(_targer.position.x, _targer.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * _damping);
    }
}
