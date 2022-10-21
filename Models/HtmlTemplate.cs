using System;

namespace SkillBoxAPI.Models
{
    public class HtmlTemplate
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }
        public string Info { get; set; }
        public DateTime Time { get; set; }
    }
}
