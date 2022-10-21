using System;

namespace SkillBoxAPI.Models
{
    public class Req
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public DateTime Time { get; set; }

    }
}
