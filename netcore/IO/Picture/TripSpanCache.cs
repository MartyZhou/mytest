using System;
using System.Collections.Generic;

namespace Cluj.Photo
{
    public static class TripSpanCache
    {
        private static Dictionary<string, TripDuration> cache = new Dictionary<string, TripDuration>();

        public static bool LocateSameDay { get; set; }

        public static void ExpandDuration(PhotoMetadata meta)
        {
            if (!string.IsNullOrWhiteSpace(meta.Address.place_id))
            {
                if (!cache.ContainsKey(meta.Address.place_id))
                {
                    cache.Add(meta.Address.place_id, InitTripDuration(meta));
                }
                else
                {
                    AdjustSpan(meta, cache[meta.Address.place_id]);
                }
            }
        }

        public static bool TryGetAddress(PhotoMetadata meta, out GoogleAddressInfo address)
        {
            var result = false;
            address = new GoogleAddressInfo();
            var foundAddress = false;
            var levelLimit = 3;
            var currentLevel = 0;

            foreach (var duration in cache.Values)
            {
                foreach (var span in duration.Spans)
                {
                    if (LocateSameDay)
                    {
                        result = meta.TakenDate.Date >= span.StartTime.Date && meta.TakenDate.Date <= span.EndTime.Date;
                    }
                    else
                    {
                        result = meta.TakenDate >= span.StartTime && meta.TakenDate <= span.EndTime;
                    }

                    if (result)
                    {
                        foundAddress = true;
                        currentLevel = duration.Location.address_components.Length;

                        if (string.IsNullOrWhiteSpace(address.place_id))
                        {
                            address = duration.Location;
                        }
                        else if (address.address_components.Length < currentLevel)
                        {
                            address = duration.Location;
                        }

                        if (currentLevel >= levelLimit)
                        {
                            break;
                        }
                    }
                }
            }

            result = foundAddress;

            return result;
        }

        private static TripDuration InitTripDuration(PhotoMetadata meta)
        {
            TripDuration duration = new TripDuration();
            duration.Location = meta.Address;
            duration.Spans = new TripSpan[1];
            TripSpan span = new TripSpan();
            span.StartTime = meta.TakenDate;
            span.EndTime = meta.TakenDate;

            duration.Spans[0] = span;

            var addressComponents = meta.Address.address_components;
            if (addressComponents != null && addressComponents.Length > 1)
            {
                duration.Parents = new TripDuration[addressComponents.Length - 1];

                for (uint i = 1; i < addressComponents.Length; i++)
                {
                    var placeId = string.Format("{0}_{1}", meta.Address.place_id, addressComponents[i].types[0]);
                    if (!cache.ContainsKey(placeId))
                    {
                        var address = new GoogleAddressInfo();
                        address.place_id = placeId;
                        address.address_components = new GoogleAddressType[addressComponents.Length - i];
                        Array.Copy(addressComponents, 1, address.address_components, 0, address.address_components.Length);

                        var higherDuration = new TripDuration();
                        higherDuration.Location = address;
                        higherDuration.Spans = new TripSpan[1];
                        higherDuration.Spans[0] = span;

                        cache.Add(placeId, higherDuration);
                    }

                    duration.Parents[i - 1] = cache[placeId];
                }
            }

            return duration;
        }

        private static void AdjustSpan(PhotoMetadata meta, TripDuration duration)
        {
            for (uint i = 0; i < duration.Spans.Length; i++)
            {
                if (meta.TakenDate > duration.Spans[i].EndTime)
                {
                    duration.Spans[i].EndTime = meta.TakenDate;
                }

                if (meta.TakenDate < duration.Spans[i].StartTime)
                {
                    duration.Spans[i].StartTime = meta.TakenDate;
                }

                if (duration.Parents != null)
                {
                    for (uint j = 0; j < duration.Parents.Length; j++)
                    {
                        if (meta.TakenDate > duration.Parents[j].Spans[0].EndTime)
                        {
                            duration.Parents[j].Spans[0].EndTime = meta.TakenDate;
                        }

                        if (meta.TakenDate < duration.Parents[j].Spans[0].StartTime)
                        {
                            duration.Parents[j].Spans[0].StartTime = meta.TakenDate;
                        }
                    }
                }
            }
        }
    }
}