﻿namespace Tapir.Services.News.Application.Models
{
    public class NewsModel
    {
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Content { get; set; }
    }
}
