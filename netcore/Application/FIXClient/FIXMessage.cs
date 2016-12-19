namespace SmallFIX
{
    public sealed class FIXMessage
    {
        [FIXTag(FIXTags.BeginString)]
        public string BeginString { get; set; }

        [FIXTag(FIXTags.AvgPx)]
        public float AvgPx { get; set; }
    }
}