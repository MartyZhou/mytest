using System.Collections.Generic;

namespace Cluj.Photo
{
    public static class TripSpanCache
    {
        private static Dictionary<string, TripDuration> cache = new Dictionary<string, TripDuration>();

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

            foreach (var duration in cache.Values)
            {
                foreach (var span in duration.Spans)
                {
                    result = meta.TakenDate >= span.StartTime && meta.TakenDate <= span.EndTime;

                    if (result)
                    {
                        address = duration.Location;
                        break;
                    }
                }
            }

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
            }
        }
    }
}