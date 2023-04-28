using UnityEngine;
using System.IO;
using System.Xml;
using UnityEditor;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Location XML Reader")]
    public class LocationXMLReader : ScriptableObject
    {
        [SerializeField] private LocationPreset[] locationPresets;
        [SerializeField] private LocationType[] locationTypes;
        public void LoadXMLFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            var doc = Resources.Load("Locations/presetLocations") as TextAsset;
            xmlDoc.Load(new StringReader(doc.text));

            string xmlPathPattern = "//location-data/location";
            XmlNodeList nodeList = xmlDoc.SelectNodes(xmlPathPattern);

            var locations = new LocationData[nodeList.Count];

            for (int i = 0; i < locations.Length; i++)
            {
                SetValues(nodeList[i]);
            }
        }

        private void SetValues(XmlNode node)
        {
            XmlNode xCoord = node.FirstChild;
            XmlNode yCoord = xCoord.NextSibling;
            XmlNode name = yCoord.NextSibling;
            XmlNode type = name.NextSibling;

            int x = int.Parse(xCoord.InnerXml);
            int y = int.Parse(yCoord.InnerXml);
            string locationName = name.InnerXml;
            //LocationsType locationType = Resources.Load("Locations/Types/" + type.InnerXml) as LocationsType;
            LocationType locationType = FindType(type.InnerXml);

            FindPreset(locationName).SetValues(x, y, locationName, locationType);
        }

        private LocationType FindType(string name)
        {
            for (int i = 0; i < locationTypes.Length; i++)
            {
                if (locationTypes[i].name == name)
                {
                    return locationTypes[i];
                }
            }
            return null;
        }

        private LocationPreset FindPreset(string name)
        {
            for (int i = 0; i < locationPresets.Length; i++)
            {
                if (locationPresets[i].name == name)
                {
                    return locationPresets[i];
                }
            }

            return CreateNewPreset(name);
        }

        private LocationPreset CreateNewPreset(string name)
        {
            var preset = ScriptableObject.CreateInstance<LocationPreset>();
            preset.name = name;

            if (!AssetDatabase.IsValidFolder("Assets/Locations/Presets"))
                AssetDatabase.CreateFolder("Locations", "Presets");

            AssetDatabase.CreateAsset(preset, $"Assets/Locations/Presets/{name}.asset");
            AssetDatabase.SaveAssets();

            return preset;
        }

        private LocationData ProcessNode(XmlNode node)
        {
            XmlNode xCoord = node.FirstChild;
            XmlNode yCoord = xCoord.NextSibling;
            XmlNode name = yCoord.NextSibling;
            XmlNode type = name.NextSibling;

            int x = int.Parse(xCoord.InnerXml);
            int y = int.Parse(yCoord.InnerXml);
            string locationName = name.InnerXml;
            //LocationType locationType = (LocationType)System.Enum.Parse(typeof(LocationType), type.InnerXml);
            LocationType altType = Resources.Load("Locations/Types/" + type.InnerXml) as LocationType;

            return new LocationData(x, y, locationName, altType);
        }
    }
}