using System;
using Newtonsoft.Json;

namespace Cluj.PhotoLocation
{
    public enum LocationType
    {
        APPROXIMATE,
        GEOMETRIC_CENTER,
        ROOFTOP,
        RANGE_INTERPOLATED
    }

    public enum AddressComponentType
    {
        political,
        country,
        administrative_area_level_1,
        administrative_area_level_2,
        locality,
        sublocality,
        sublocality_level_1,
        neighborhood,
        route,
        street_number,
        postal_code,
        postal_code_suffix
    }

    public class AddressComponent
    {
        public AddressComponent(AddressType address)
        {
            this.LongName = address.LongName;
            this.ShortName = address.ShortName;
            this.Types = new AddressComponentType[address.Types.Length];

            AddressComponentType componentType;
            for (int i = 0; i < address.Types.Length; i++)
            {
                Enum.TryParse<AddressComponentType>(address.Types[i], out componentType);
                this.Types.SetValue(componentType, i);
            }
        }
        public string LongName { get; }
        public string ShortName { get; }
        public AddressComponentType[] Types { get; }
    }

    public class Node
    {
        public Node(AddressDetails address)
        {
            this.PlaceId = address.PlaceId;
            this.FormattedAddress = address.FormattedAddress;
            this.Bounds = address.Geometry.Bounds;
            LocationType locationType;
            Enum.TryParse<LocationType>(address.Geometry.LocationType, out locationType);
            this.LocationType = locationType;
            this.Location = address.Geometry.Location;

            this.Components = new AddressComponent[address.AddressComponents.Length];

            for (int i = 0; i < address.AddressComponents.Length; i++)
            {
                this.Components.SetValue(new AddressComponent(address.AddressComponents[i]), i);
            }
        }

        public void LinkParent(Node parent)
        {
            this.Parent = parent;
        }

        public string PlaceId { get; set; }
        public Bounds Bounds { get; set; }
        public LocationType LocationType { get; set; }
        public Location Location { get; set; }
        public AddressComponent[] Components { get; set; }
        public string FormattedAddress { get; set; }
        public Node Parent { get; private set; }
    }

    public struct CityLocation
    {
        public string City { get; set; }
        public string AreaLevel2 { get; set; }
        public string AreaLevel1 { get; set; }
        public string Country { get; set; }
    }

    public struct AddressType
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public struct AddressDetails
    {
        [JsonProperty("address_components")]
        public AddressType[] AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public struct Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public struct Bounds
    {
        [JsonProperty("northeast")]
        public Location Norteast { get; set; }

        [JsonProperty("southwest")]
        public Location Southwest { get; set; }
    }

    public struct Geometry
    {
        [JsonProperty("bounds")]
        public Bounds Bounds { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public struct AddressResult
    {
        [JsonProperty("results")]
        public AddressDetails[] Results { get; set; }

        public string City { get; set; }

        public bool IsValid { get; set; }
    }

    public struct TripSpan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public struct TripDuration
    {
        public AddressDetails Location { get; set; }
        public TripSpan[] Spans { get; set; }
        public TripDuration[] Parents { get; set; }
    }

    public struct Config
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("api_key")]
        public string API_KEY { get; set; }

        [JsonProperty("center_tolerance")]
        public double CenterTolerance { get; set; }
    }
}