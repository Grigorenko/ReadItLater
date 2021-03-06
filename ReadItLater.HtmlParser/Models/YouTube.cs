namespace ReadItLater.HtmlParser
{
    public class YouTube
    {
        public VideoDetail? VideoDetails { get; set; }

        public class VideoDetail
        {
            public string? Title { get; set; }
            public ThumbnailWrapper? Thumbnail { get; set; }

            public class ThumbnailWrapper
            {
                public ThumbnailItem[]? Thumbnails { get; set; }

                public class ThumbnailItem
                {
                    public string? Url { get; set; }
                    public int? Width { get; set; }
                    public int? Height { get; set; }
                }
            }
        }
    }
}
