using System.Collections.Generic;

namespace BookFront.Models
{
    public class Root
    {
        public List<Item> items { get; set; }
        public bool has_more { get; set; }
        public int backoff { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }


}
