using UnityEngine;

using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

    /// <summary>
    /// Automatically sets up the appropriate UI input module based on available packages.
    /// Prefers Input System if available, otherwise falls back to Legacy Input.
    /// </summary>
    [RequireComponent(typeof(EventSystem))]
    public class UIParticleInputModuleInstaller : MonoBehaviour
    {
        [SerializeField] private bool _forceLegacyInput = false;
        
        private void Awake()
        {
            SetupInputModule();
        }
        
        private void SetupInputModule()
        {
            EventSystem eventSystem = GetComponent<EventSystem>();
            
            if (eventSystem == null)
            {
                Debug.LogError("EventSystem component not found!");
                return;
            }
            
            // Remove existing input modules
            BaseInputModule[] existingModules = GetComponents<BaseInputModule>();
            foreach (BaseInputModule module in existingModules)
            {
                DestroyImmediate(module);
            }
            
            // Install the appropriate module
            #if ENABLE_INPUT_SYSTEM
            if (!_forceLegacyInput)
            {
                InstallInputSystemModule(eventSystem);
            }
            else
            {
                InstallLegacyModule(eventSystem);
            }
            #else
            InstallLegacyModule(eventSystem);
            #endif
        }
        
        #if ENABLE_INPUT_SYSTEM
        private void InstallInputSystemModule(EventSystem eventSystem)
        {
            InputSystemUIInputModule inputModule = eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            
            // Optional: Configure default actions if needed
            if (inputModule.actionsAsset == null)
            {
                Debug.Log("Input System UI module installed with default actions.");
            }
            else
            {
                Debug.Log("Input System UI module installed with custom actions.");
            }
        }
        #endif
        
        private void InstallLegacyModule(EventSystem eventSystem)
        {
            StandaloneInputModule legacyModule = eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            
            // Configure legacy module for typical UI navigation
            legacyModule.horizontalAxis = "Horizontal";
            legacyModule.verticalAxis = "Vertical";
            legacyModule.submitButton = "Submit";
            legacyModule.cancelButton = "Cancel";
            
            Debug.Log("Legacy Input module installed (StandaloneInputModule)");
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Reconfigure Input Module")]
        private void ReconfigureInputModule()
        {
            SetupInputModule();
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log("Input module reconfigured.");
        }
        #endif
    }
