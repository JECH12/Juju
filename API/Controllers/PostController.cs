using API.Models;
using AutoMapper;
using Business;
using Business.Dto;
using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace API.Controllers.Post
{
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        public readonly IPost _postServices;
        public readonly ICustomer _customerService;
        public PostController(IPost postServices, ICustomer customer) 
        {
            _postServices = postServices;
            _customerService = customer;
        }

        [HttpPost]
        public IActionResult InsertPost([FromBody] SeveralPostModel posts)
        {
            var rtaInsert = new ResponseServiceDto<bool>();
            CustomerDto customer = new CustomerDto();
            customer.CustomerId = posts.CustomerId;
            var existCustomer = _customerService.GetCostumer(customer);
            if (existCustomer != null)
            {
                
                List<PostDto> postsDto = new List<PostDto>();
                var ProductAnnouncementList = posts.ListPosts;
                foreach (var post in ProductAnnouncementList)
                {
                    PostDto postDto = Mapper.Map<PostDto>(post);
                    postDto.CustomerId = posts.CustomerId;
                    postsDto.Add(postDto);
                }

                rtaInsert = _postServices.InsertPost(postsDto);

                return !rtaInsert.Result ? new JsonResult(BadRequest(rtaInsert)) : new JsonResult(Created(string.Empty, rtaInsert));
            }

            rtaInsert.Message = "Customer NotFound";
            rtaInsert.HttpStatusCode = HttpStatusCode.NotFound;

            return NotFound(rtaInsert);

        }


    }
}
