using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	private Coroutine _routine;

	private void Start()
	{
		_routine = StartCoroutine(SomeCoroutine());
	}

	[ContextMenu("Kill routine")]
	public void KillRoutine()
	{
		StopCoroutine(_routine);
	}

	private IEnumerator SomeCoroutine()
	{
		Debug.Log(message: "Start coroutine");

		while (enabled)
		{
			Debug.Log(message: "Wait for another coroutine");
			yield return AnotherCoroutine();
			Debug.Log(message: "Is done");
			yield return new WaitForSeconds(1f);
		}
	}

	private IEnumerator AnotherCoroutine()
	{
		yield return new WaitForSeconds(2f);
	}
}
