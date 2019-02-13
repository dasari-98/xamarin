using System;

namespace App1.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
    public class Grievence
    {
        public string Name { get; set; }
        public string CustomGrievence { get; set; }
        public long mobile { get; set; }

        public byte[] imagedata { get; set; }
    }
}