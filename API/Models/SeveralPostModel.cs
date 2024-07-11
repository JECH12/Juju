using System.Collections.Generic;

namespace API.Models
{
    public class SeveralPostModel
    {
        public int CustomerId { get; set; }

        public List<PostModel> ListPosts { get; set; }
    }
}
