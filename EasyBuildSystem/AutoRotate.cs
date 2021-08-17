using UnityEngine;
using EasyBuildSystem.Features.Scripts.Core.Base.Condition;
using EasyBuildSystem.Features.Scripts.Core.Base.Condition.Enums;


namespace EasyBuildSystem.Features.Scripts.Core.Templates
{

	[Condition("Auto-Rotate", "Will rotate Based On Object Hit Normal Direction.", ConditionTarget.PieceBehaviour)]
	public class AutoRotate : ConditionBehaviour
	{
		[Tooltip("Suggest Ignoring Sockets")]
		public LayerMask layers;
		/// <summary>
		/// Called before the placement of the piece.
		/// </summary>
		public override bool CheckForPlacement()
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit,100,layers))
			{
				if (!hit.collider)
				{
					return false;
				}
				transform.SetParent(hit.transform);

				Vector3 hitRot = hit.normal;
				Vector3 spawn = hitRot * 20 + transform.parent.position;
				
				transform.LookAt(spawn,Vector3.up);
			}

			return true;
		}

		/// <summary>
        /// Called before the destruction of the piece.
        /// </summary>
		public override bool CheckForDestruction() { return true; }

		/// <summary>
        /// Called before the edition of the piece.
        /// </summary>
		public override bool CheckForEdition() { return true; }
	}
}