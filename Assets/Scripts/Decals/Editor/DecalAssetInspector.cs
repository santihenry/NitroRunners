using System.IO;
using UnityEditor;
using UnityEngine;

namespace SamDriver.Decal
{
    [CustomEditor(typeof(DecalAsset))]
    [CanEditMultipleObjects]
    public class DecalAssetInspector : Editor
    {
        //const string editorResourcesPath = "Packages/com.samdriver.driven-decals/Editor/Resources/";
        const string editorResourcesPath = "Assets/Scripts/Editor/Resources/";
        static Material FetchEditorMaterial(string materialName)
        {
            var path = Path.Combine(editorResourcesPath, $"Materials/{materialName}.mat");
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null) throw new FileNotFoundException($"Couldn't find material file at {path}");
            return material;
        }

        static string[] essentialMaterialProperties = new string[] {
            "_MainTex",
        };
      

        SerializedProperty material;
        //SerializedProperty uMin, vMin, uMax, vMax;

        void OnEnable()
        {
            material = serializedObject.FindProperty(nameof(DecalAsset.Material));
        }

        public override void OnInspectorGUI()
        {
            bool isEditingMultipleObjects = (targets != null && targets.Length > 1);
            serializedObject.Update();
            MaterialPickerGUI();
            serializedObject.ApplyModifiedProperties();
        }

        void MaterialPickerGUI()
        {
            EditorGUILayout.PropertyField(material, new GUIContent("Material"));

            var selectedMaterial = (Material)material.objectReferenceValue;
            if (selectedMaterial == null) return;

            // warn about missing properties
            foreach (string propertyName in essentialMaterialProperties)
            {
                if (!selectedMaterial.HasProperty(propertyName))
                {
                    EditorGUILayout.HelpBox($"{selectedMaterial.name} doesn't have a {propertyName} property, which is required for use as a decal material.",
                      MessageType.Error);
                }
            }
           
        }

    
    }
}
