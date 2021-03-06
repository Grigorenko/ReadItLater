using System;
using System.Collections.Generic;

namespace ReadItLater.Data
{
    public static class RefProjectionMapConfig
    {
        public static Action<RefProjection, TagProjection> Action => (r, t) =>
        {
            if (!(t is null))
            {
                if (r.Tags is null)
                    r.Tags = new List<TagProjection>();

                r.Tags.Add(t);
            }
        };
    }
}
