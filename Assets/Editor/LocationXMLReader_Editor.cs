using UnityEngine;
using UnityEditor;

namespace SD.LocationSystem
{
    [CustomEditor(typeof(LocationXMLReader))]
    public class LocationXMLReader_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LocationXMLReader reader = (LocationXMLReader)target;

            EditorGUI.BeginChangeCheck();
            var file = EditorGUILayout.ObjectField("Preset Location XML", reader.XMLRawFile, typeof(TextAsset), false);

            if (EditorGUI.EndChangeCheck())
            {
                reader.XMLRawFile = file as TextAsset;
            }
        }
    }
}