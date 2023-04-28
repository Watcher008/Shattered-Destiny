namespace SD.LocationSystem
{
    public class LocationData
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public string Name { get; private set; }
        public LocationType type { get; private set; }
        public bool IsDiscovered { get; private set; } = false;

        public LocationData(int x, int y, string name, LocationType type)
        {
            this.x = x;
            this.y = y;
            Name = name;
            this.type = type;
        }

        public void OnDiscovered()
        {
            IsDiscovered = true;

            //probably have some other events here for registering in player journal 
        }
    }
}