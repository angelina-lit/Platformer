using Assets.Scripts.Creatures;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SetFollowComponent : MonoBehaviour
{
	private void Start()
	{
		var vCamera = GetComponent<CinemachineVirtualCamera>();
		vCamera.Follow = FindObjectOfType<Hero>().transform;
	}
}
