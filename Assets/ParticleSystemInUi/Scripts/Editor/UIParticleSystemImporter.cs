using UnityEditor;
using UnityEngine;

    [InitializeOnLoad]
    public class UIParticleSystemImporter : AssetPostprocessor
    {
        private const string LayerName = "UIParticlesLayer";
        private const int StartLayerIndex = 8;
        private const int EndLayerIndex = 31;
        
        static UIParticleSystemImporter()
        {
            CreateUIParticleLayer();
        }
        
        private static void OnPostprocessAllAssets(
            string[] importedAssets, 
            string[] deletedAssets, 
            string[] movedAssets, 
            string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                if (assetPath.Contains("ParticleSystemInUI"))
                {
                    CreateUIParticleLayer();
                    break;
                }
            }
        }
        
        [MenuItem("Tools/ParticleSystemInUI/Setup Layer")]
        public static void CreateUIParticleLayer()
        {
            int layerIndex = LayerMask.NameToLayer(LayerName);
            
            if (layerIndex != -1)
            {
                Debug.Log($"Layer '{LayerName}' already exists at index {layerIndex}");
                return;
            }
            
            SerializedObject tagManager = new SerializedObject(
                AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProperty = tagManager.FindProperty("layers");
            
            for (int i = StartLayerIndex; i <= EndLayerIndex; i++)
            {
                SerializedProperty layerProperty = layersProperty.GetArrayElementAtIndex(i);
                
                if (string.IsNullOrEmpty(layerProperty.stringValue))
                {
                    layerProperty.stringValue = LayerName;
                    tagManager.ApplyModifiedProperties();
                    Debug.Log($"Created new layer '{LayerName}' at index {i}");
                    return;
                }
            }
            
            Debug.LogError($"Failed to create layer '{LayerName}'. No available layer slots found.");
        }
        /*
        [MenuItem("Tools/ParticleSystemInUI/Export")]
        public static void ExportPackage()
        {
            string[] assetPaths = new string[]
            {
                "Assets/ParticleSystemInUI",
            };
    
            AssetDatabase.ExportPackage(assetPaths, "ParticleSystemInUI.unitypackage", 
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
    
            Debug.Log("Package exported to ParticleSystemInUI.unitypackage");
        }*/
    }
