using Business.Dto;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface IPost
    {
        void VerifyCustomerPosts(CustomerDto customer);
        List<Post> GetAllCustomerPosts(CustomerDto customer);
        void DeletePost(List<Post> listPostEntities);

        ResponseServiceDto<bool> InsertPost(List<PostDto> listPost);
    }
}
