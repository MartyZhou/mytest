using System;
using System.Collections.Generic;
using Cluj.Exif;
using Cluj.PhotoLocation;

namespace Cluj.PhotoHelper
{
    internal class PhotoInfo
    {
        public PhotoMetadata PhotoMetadata { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string NewPath { get; set; }
        public Node Node { get; set; }
    }

    internal class NodeTimeSpan
    {
        public NodeTimeSpan()
        {
            Children = new Dictionary<string, NodeTimeSpan>();
        }
        public Node Node { get; set; }

        public DateTime LeftDate { get; set; }

        public DateTime RightDate { get; set; }

        public Dictionary<string, NodeTimeSpan> Children { get; set; }
    }
}