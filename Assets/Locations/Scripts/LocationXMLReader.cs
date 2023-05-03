using UnityEngine;
using System.IO;
using System.Xml;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Location XML Reader")]
    public class LocationXMLReader : ScriptableObject
    {
        [SerializeField] private LocationType[] locationTypes;

        public LocationData[] LoadXMLFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            var doc = Resources.Load("XMLFiles/presetLocations") as TextAsset;
            xmlDoc.Load(new StringReader(doc.text));

            string xmlPathPattern = "//location-data/location";
            XmlNodeList nodeList = xmlDoc.SelectNodes(xmlPathPattern);

            var locations = new LocationData[nodeList.Count];

            for (int i = 0; i < locations.Length; i++)
            {
                locations[i] = ProcessNode(nodeList[i]);
            }

            return locations;
        }

        private LocationData ProcessNode(XmlNode node)
        {
            XmlNode xCoord = node.FirstChild;
            XmlNode yCoord = xCoord.NextSibling;
            XmlNode nodeName = yCoord.NextSibling;
            XmlNode nodeDescription = nodeName.NextSibling;
            XmlNode nodeType = nodeDescription.NextSibling;
            XmlNode nodeAlignment = nodeType.NextSibling;

            int x = int.Parse(xCoord.InnerXml);
            int y = int.Parse(yCoord.InnerXml);
            string name = nodeName.InnerXml;
            string description = nodeDescription.InnerXml;
            LocationType type = FindType(nodeType.InnerXml.ToLower());// Resources.Load("Locations/Types/" + nodeType.InnerXml) as LocationType;

            return new LocationData(x, y, name, type, description);
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
            throw new System.Exception("Location type not found.");
        }
    }
}