using UnityEngine;
using System.IO;
using System.Xml;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Location XML Reader")]
    public class LocationXMLReader : ScriptableObject
    {
        [HideInInspector] public TextAsset XMLRawFile;

        public Location[] LoadXMLFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(XMLRawFile.text));

            string xmlPathPattern = "//location-data/location";
            XmlNodeList nodeList = xmlDoc.SelectNodes(xmlPathPattern);

            var locations = new Location[nodeList.Count];

            for (int i = 0; i < locations.Length; i++)
            {
                locations[i] = ProcessNode(nodeList[i]);
            }

            return locations;
        }

        private Location ProcessNode(XmlNode node)
        {
            XmlNode xCoord = node.FirstChild;
            XmlNode yCoord = xCoord.NextSibling;
            XmlNode name = yCoord.NextSibling;
            XmlNode type = name.NextSibling;

            int x = int.Parse(xCoord.InnerXml);
            int y = int.Parse(yCoord.InnerXml);
            string locationName = name.InnerXml;
            //LocationType locationType = (LocationType)System.Enum.Parse(typeof(LocationType), type.InnerXml);
            LocationsType altType = Resources.Load("Locations/Types/" + type.InnerXml) as LocationsType;

            return new Location(x, y, locationName, altType);
        }
    }
}