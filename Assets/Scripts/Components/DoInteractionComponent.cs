using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components
{
	public class DoInteractionComponent : MonoBehaviour
	{
		public void DoInteraction(GameObject go)
		{
			var interactable = go.GetComponent<InteractableComponent>();
			if (interactable != null ) interactable.Interact();
		}
	}
}
