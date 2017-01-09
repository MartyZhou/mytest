namespace Cluj.Exif
{
    public struct GPSInfo
    {
        public char LatRef { get; set; }
        public double Lat { get; set; }
        public char LonRef { get; set; }
        public double Lon { get; set; }
    }
}