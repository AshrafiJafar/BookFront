using Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFront.Models
{
    public class IndexViewModel
    {
        public IList<AuthorDto> Authors { get; set; }
        public IList<TypeDto> Types { get; set; }
        public IList<BookListDto> Books { get; set; }
    }
}
