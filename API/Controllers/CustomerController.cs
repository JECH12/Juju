using API.Models;
using AutoMapper;
using Business.Dto;
using Business.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace API.Controllers.Customer
{
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customerService;
        public CustomerController(ICustomer customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Route("getCustomer")]
        public IActionResult GetCustomer([FromBody] CustomerModel customer)
        {
            var responseServiceDto = new ResponseServiceDto<CustomerDto>();
            CustomerDto customerDto = Mapper.Map<CustomerDto>(customer);
            var result = _customerService.GetCostumer(customerDto);
            if (result != null)
            {
                responseServiceDto.Message = "Customer Found";
                responseServiceDto.Result = result;
                responseServiceDto.HttpStatusCode = HttpStatusCode.OK;

                return Ok(responseServiceDto);       
            }

            responseServiceDto.Message = "Customer NotFound";
            responseServiceDto.HttpStatusCode = HttpStatusCode.NotFound;

            return NotFound(responseServiceDto);
        }


        [HttpGet()]
        public IActionResult GetAll()
        {
            var responseServiceDto = new ResponseServiceDto<List<CustomerDto>>();
            var customers = _customerService.ListCustomer();

            responseServiceDto.Message = "Customer List";
            responseServiceDto.Result = customers;
            responseServiceDto.HttpStatusCode = HttpStatusCode.OK;

            return customers.Count.Equals(0) ? new JsonResult(NoContent()) : new JsonResult(Ok(responseServiceDto));
        }


        [HttpPost()]
        public IActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            var rtaInsert = new ResponseServiceDto<bool>();
            CustomerDto customerDto = Mapper.Map<CustomerDto>(customer);
            if (_customerService.CustomerNameExist(customerDto))
            {
                rtaInsert.Message = "El nombre introducido ya existe";
                rtaInsert.HttpStatusCode = HttpStatusCode.Conflict;
                rtaInsert.Result = false;               
            }
            else
            {
                rtaInsert = _customerService.InsertCustomer(customerDto);
            }
            

            return !rtaInsert.Result ? new JsonResult(BadRequest(rtaInsert)) : new JsonResult(Created(string.Empty, rtaInsert));
        }

        [HttpPut()]
        public IActionResult Update([FromBody] CustomerModel customer)
        {
            var rta = new ResponseServiceDto<bool>();
            CustomerDto customerDto = Mapper.Map<CustomerDto>(customer);
            rta = _customerService.UpdateCustomer(customerDto);

            return Ok(rta);
        }

        [HttpDelete()]
        public IActionResult Delete([FromBody] CustomerModel customer)
        {
            var rta = new ResponseServiceDto<bool>();
            CustomerDto customerDto = Mapper.Map<CustomerDto>(customer);
            rta = _customerService.DeleteCustomer(customerDto);

            return Ok(rta);
        }
    }
}
