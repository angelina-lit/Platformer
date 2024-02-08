using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
	public abstract class Patrol : MonoBehaviour
	{
		public abstract IEnumerator DoPatrol();
	}
}
