namespace SD.LocationSystem
{
    public class LocationData
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public string name { get; private set; }
        public LocationType type { get; private set; }
        public bool isDiscovered { get; private set; }
        public string description { get; private set; }

        public LocationData(int x, int y, string name, LocationType type, string description)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            this.type = type;
            this.description = description;
        }
    }
}