using EasyBuildSystem.Features.Scripts.Core.Base.Builder.Enums;
using System.Collections.Generic;
using EasyBuildSystem.Features.Scripts.Core.Base.Builder;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;


namespace TauntGames
{
    [RequireComponent(typeof(BuilderBehaviour))]
    [AddComponentMenu("Easy Build System/Features/Builders Behaviour/Inputs/Taunt Builder Input")]
    public class TauntBuilderInput : MonoBehaviour
    {

        #region Class Variables
        
        //Private
        private DemoInputActions.BuildingActions _building;
        private DemoInputActions _inputs;
        private bool _wheelRotationReleased;
        private BuilderBehaviour _buildBehaviour;
        private bool uiBlocking;
        [SerializeField] private NetworkIdentity netID;
        [SerializeField] private GameObject buildUI;


        #endregion

        #region Start/Awake
        public virtual void OnEnable()
        {
            _inputs.Building.Enable();
        }

        public virtual void OnDisable()
        {
            _inputs.Building.Disable();
        }


        public virtual void OnDestroy()
        {
            _inputs.Building.Disable();
        }

        public virtual void Awake()
        {
            _buildBehaviour = GetComponent<BuilderBehaviour>();
            uiBlocking = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _inputs = new DemoInputActions();
            _building = _inputs.Building;
        }
        #endregion

        #region Update

        public virtual void Update()
        {
            
            if (!netID.isLocalPlayer)
                return;
            
            #region Build Mode Updates
            
            if (_buildBehaviour.CurrentMode == BuildMode.Placement)
            {
                if (IsPointerOverUIElement())
                {
                    return;
                }

                if (_building.Validate.triggered)
                {
                    _buildBehaviour.PlacePrefab();
                }

                float wheelAxis = _building.Rotate.ReadValue<float>();

                if (wheelAxis > 0 && !_wheelRotationReleased)
                {
                    _wheelRotationReleased = true;
                    _buildBehaviour.RotatePreview(_buildBehaviour.SelectedPrefab.RotationAxis);
                }
                else if (wheelAxis < 0 && !_wheelRotationReleased)
                {
                    _wheelRotationReleased = true;
                    _buildBehaviour.RotatePreview(-_buildBehaviour.SelectedPrefab.RotationAxis);
                }
                else if (wheelAxis == 0)
                {
                    _wheelRotationReleased = false;
                }
                //
                // if (_building.Cancel.triggered)
                // {
                //     _buildBehaviour.ChangeMode(BuildMode.None);
                // }
            }
            else if (_buildBehaviour.CurrentMode == BuildMode.Destruction)
            {
                if (IsPointerOverUIElement())
                {
                    return;
                }

                if (_building.Validate.triggered)
                {
                    if (_buildBehaviour.CurrentRemovePreview != null)
                    {
                        _buildBehaviour.DestroyPrefab();
                    }
                }
                //
                // if (_building.Cancel.triggered)
                // {
                //     _buildBehaviour.ChangeMode(BuildMode.None);
                // }
            }
            #endregion
        }

        #endregion


        #region Class Methods

        public void ChangeBuildMode(BuildMode buildMode)
        {
            _buildBehaviour.ChangeMode(buildMode);
        }


        public void BuildMenuTrigger()
        {
            buildUI.SetActive(!buildUI.activeSelf);
            Cursor.visible = buildUI.activeSelf;
            Cursor.lockState = !buildUI.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public NetworkIdentity getNetID()
        {
            return netID;
        }

        #endregion


        #region Helper Methods

        private bool IsPointerOverUIElement()
        {
            if (!uiBlocking)
            {
                return false;
            }

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                return false;
            }

            if (EventSystem.current == null)
            {
                return false;
            }

            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        #endregion
       

      
    }
}