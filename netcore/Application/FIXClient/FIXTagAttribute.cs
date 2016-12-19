using System;

namespace SmallFIX
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FIXTagAttribute : Attribute
    {
        public FIXTagAttribute(FIXTags tag)
        {
            Tag = tag;
        }

        public FIXTags Tag { get; set; }
    }
}