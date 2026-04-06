using UnityEngine;

[ExecuteAlways]
public class UIParticleValidator : MonoBehaviour
{
        private const string LayerName = "UIParticlesLayer";
        
        [SerializeField] private Camera _uiParticleCamera;
        [SerializeField] private GameObject[] _particleEffects;
        
        private void OnEnable()
        {
            ValidateLayerSetup();
        }
        
        private void ValidateLayerSetup()
        {
            int layerIndex = LayerMask.NameToLayer(LayerName);
            
            if (layerIndex == -1)
            {
                Debug.LogError($"Layer '{LayerName}' not found. Please create it in Project Settings -> Tags and Layers");
                return;
            }
            
            ValidateCameraLayer(layerIndex);
            ValidateParticleEffectsLayer(layerIndex);
        }
        
        private void ValidateCameraLayer(int layerIndex)
        {
            if (_uiParticleCamera == null)
            {
                return;
            }
            
            if ((_uiParticleCamera.cullingMask & (1 << layerIndex)) != 0)
            {
                return;
            }
            
            Debug.LogWarning($"Camera {_uiParticleCamera.name} does not render layer '{LayerName}'. Updating...");
            _uiParticleCamera.cullingMask |= (1 << layerIndex);
        }
        
        private void ValidateParticleEffectsLayer(int layerIndex)
        {
            foreach (GameObject effect in _particleEffects)
            {
                if (effect == null)
                {
                    continue;
                }
                
                if (effect.layer != layerIndex)
                {
                    SetLayerRecursively(effect, layerIndex);
                }
            }
        }
        
        private void SetLayerRecursively(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            
            foreach (Transform child in gameObject.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
        
#if UNITY_EDITOR
        [ContextMenu("Fix Layer Setup")]
        private void FixLayerSetup()
        {
            ValidateLayerSetup();
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log("Layer setup validated and fixed.");
        }
#endif
    }