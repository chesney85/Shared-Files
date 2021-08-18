using EasyBuildSystem.Features.Scripts.Core.Base.Addon;
using EasyBuildSystem.Features.Scripts.Core.Base.Addon.Enums;
using EasyBuildSystem.Features.Scripts.Core.Base.Builder;
using EasyBuildSystem.Features.Scripts.Core.Base.Builder.Enums;
using EasyBuildSystem.Features.Scripts.Core.Base.Piece;
using EasyBuildSystem.Features.Scripts.Core.Scriptables.Collection;
using UnityEngine;

namespace EasyBuildSystem.Features.Scripts.Core.Templates
{
	//Do not forget to set a AddOnTarget to a component in attribute.
	[Addon("Copy Select Piece", "Will copy any piece you are looking at.", AddonTarget.PieceBehaviour)]
	public class CopySelectPiece : AddonBehaviour
	{
		
		[SerializeField] private float RefreshInterval;
		[SerializeField] private LayerMask pieceLayer;
		private BuilderBehaviour builder;
		[Tooltip("Time before Auto Switch Occurs")]
		[SerializeField] private float autoSwitchDelay;
		private Camera cam;
		private void OnEnable()
		{
			cam = Camera.main;
			builder = cam.GetComponent<BuilderBehaviour>();
			InvokeRepeating(nameof(Refresh), autoSwitchDelay, RefreshInterval);
		}

		private void Refresh()
		{
			if (builder.CurrentMode == BuildMode.Placement)
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, 100, pieceLayer))
				{
					PieceBehaviour p = null;
					if (hit.collider.GetComponent<PieceBehaviour>())
					{
						p = hit.collider.GetComponent<PieceBehaviour>();
					}

					else if (hit.collider.GetComponentInParent<PieceBehaviour>())
					{
						p = hit.collider.GetComponentInParent<PieceBehaviour>();
					}
					Debug.Log("Hit");
					builder.ChangePiece(p.Id, p.AppearanceIndex);
				}
				
			}

		}
		
		 //This is ChangePiece to be added to BuilderBehaviour.cs
		 
		 //You may have to change id to int in parameter if you are still using default id type
		
		 //  public void ChangePiece(string id,int skinIndex)
        // {
        //     ChangeMode(BuildMode.None);
        //     SelectedPrefab = BuildManager.Instance.GetPieceById(id);
        //     SelectedPrefab.ChangeSkin(skinIndex);
        //     SelectPrefab(SelectedPrefab);
        //     ChangeMode(BuildMode.Placement);
        // }

	}
}