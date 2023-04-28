using UnityEngine;
using UnityEditor;

namespace SD.LocationSystem
{
    [CustomEditor(typeof(LocationXMLReader))]
    public class LocationXML_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            LocationXMLReader reader = (LocationXMLReader)target;

            if (GUILayout.Button("Load File")) reader.LoadXMLFile();
        }
    }
}

