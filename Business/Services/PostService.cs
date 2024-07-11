using AutoMapper;
using Business.Dto;
using Business.Interfaces;
using Core.UnitOfWork;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Business.Services
{
    public class PostService:IPost
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void  VerifyCustomerPosts(CustomerDto customer)
        {
            var customerPosts = GetAllCustomerPosts(customer);

            DeletePost(customerPosts);

        }

        public List<Post> GetAllCustomerPosts(CustomerDto customer)
        {
            List<Post> customerPosts = _unitOfWork.PostRepository.FindAll(p => p.CustomerId == customer.CustomerId).ToList();
                                 
            return customerPosts;
        }

        public ResponseServiceDto<bool> InsertPost(List<PostDto> listPost)
        {
            ResponseServiceDto<bool> responseServiceDto = new ResponseServiceDto<bool>();

            List<Post> listPostEntities = new List<Post>();
            foreach (var post in listPost)
            {
                if (post.Body.Length > 200 )
                {
                    post.Body = post.Body.Substring(0, 96).TrimEnd();
                    post.Body += "...";
                }

                switch (post.Type)
                {
                    case 1:
                        post.Category = "Farándula";
                        break;

                    case 2:
                        post.Category = "Politica";
                        break;

                    case 3:
                        post.Category = "Futbol";
                        break;
                }

                var postEntity = Mapper.Map<PostDto, Post>(post);

                listPostEntities.Add(postEntity);
            }
            
            _unitOfWork.PostRepository.Insert(listPostEntities);
            var rta = _unitOfWork.Save();

            if (rta == listPostEntities.Count())
            {
                responseServiceDto.Message = "Posts Created";
                responseServiceDto.Result = true;
                responseServiceDto.HttpStatusCode = HttpStatusCode.OK;
                return responseServiceDto;
            }

            responseServiceDto.Message = "Posts Not Created";
            responseServiceDto.Result = true;
            responseServiceDto.HttpStatusCode = HttpStatusCode.BadRequest;

            return responseServiceDto;
        }

        public void DeletePost(List<Post> listPostEntities)
        {
            if (listPostEntities.Count > 0)
            {
                _unitOfWork.PostRepository.Delete(listPostEntities);
                _unitOfWork.Save();
            } 
        }
    }
}
