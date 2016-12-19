using System;

namespace SmallFIX
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FIXTagAttribute : Attribute
    {
        public FIXTagAttribute(FIXTags tag, bool required = false)
        {
            Tag = tag;
            Required = required;
        }

        public FIXTags Tag { get; set; }

        public bool Required { get; set; }
    }
}