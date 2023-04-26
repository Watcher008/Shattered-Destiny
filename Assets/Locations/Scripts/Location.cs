namespace SD.LocationSystem
{
    public class Location
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public string name { get; private set; }
        public LocationType type { get; private set; }
        public LocationsType types { get; private set; }

        public Location(int x, int y, string name, LocationType type)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            this.type = type;
        }

        public Location(int x, int y, string name, LocationsType type)
        {
            this.x = x;
            this.y = y;
            this.name = name;
            types = type;
        }
    }

    public enum LocationType
    {
        city,
        town,
        village,
        
        fort,
        tower,
        monument,
        landmark,
        camp,

        cave,
        ruins,
    }
}