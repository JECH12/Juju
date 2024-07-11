using AutoMapper;
using Business.Dto;
using Business.Interfaces;
using Core.UnitOfWork;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Business.Services
{
    public  class CustomerService:ICustomer
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPost _postService;
        public CustomerService(IUnitOfWork unitOfWork, IPost postService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }
        public List<CustomerDto> ListCustomer()
        {
            var customer = new CustomerDto();
            var resultCustomer = _unitOfWork.CustomerRepository.GetAll();

            var result = (from cust in resultCustomer
                          select new CustomerDto
                          {
                              CustomerId = cust.CustomerId,
                              Name = cust.Name,
                          }).ToList();

            return result;
        }

        public CustomerDto GetCostumer(CustomerDto customer)
        {
            var resultCustomer = _unitOfWork.CustomerRepository.Find(x => x.CustomerId.Equals(customer.CustomerId));

            if (resultCustomer != null)
            {
                customer.Name = resultCustomer.Name;
                return customer;
            }
            return customer = null;

        }

        public bool CustomerNameExist(CustomerDto objCustomer)
        {
            var exist = _unitOfWork.CustomerRepository.Find(x => x.Name.Trim().Replace(" ", "").Equals(objCustomer.Name.Trim().Replace(" ", "")));
            if (exist != null)
            {
                return true;
            }
            return false;
        }

        public ResponseServiceDto<bool> InsertCustomer(CustomerDto objCustomer)
        {
            ResponseServiceDto<bool> responseServiceDto = new ResponseServiceDto<bool>();
            var customerEntity = Mapper.Map<CustomerDto, Customer>(objCustomer);
            _unitOfWork.CustomerRepository.Insert(customerEntity);
            var rta = _unitOfWork.Save();

            if (rta == 1)
            {
                responseServiceDto.Message = "Customer Created";
                responseServiceDto.Result = true;
                responseServiceDto.HttpStatusCode = HttpStatusCode.OK;
                return responseServiceDto;
            }

            responseServiceDto.Message = "Customer Not Created";
            responseServiceDto.Result = true;
            responseServiceDto.HttpStatusCode = HttpStatusCode.BadRequest;

            return responseServiceDto;
        }

        public ResponseServiceDto<bool> UpdateCustomer(CustomerDto objCustomer)
        {
            ResponseServiceDto<bool> responseServiceDto = new ResponseServiceDto<bool>();
            var objEntity = _unitOfWork.CustomerRepository.Find(c => c.CustomerId.Equals(objCustomer.CustomerId));
            if (objEntity == null)
            {
                responseServiceDto.Message = "Customer Not Found";
                responseServiceDto.Result = false;
                responseServiceDto.HttpStatusCode = HttpStatusCode.NotFound;

                return responseServiceDto;
            }

            objEntity.Name = objCustomer.Name;

            _unitOfWork.CustomerRepository.Update(objEntity);

            var result = _unitOfWork.Save();

            if (result == 1)
            {
                responseServiceDto.Message = "Customer Updated";
                responseServiceDto.Result = true;
                responseServiceDto.HttpStatusCode = HttpStatusCode.OK;

                return responseServiceDto;
            }

            responseServiceDto.Message = "Customer Not Updated";
            responseServiceDto.Result = false;
            responseServiceDto.HttpStatusCode = HttpStatusCode.BadRequest;

            return responseServiceDto;
        }

        public ResponseServiceDto<bool> DeleteCustomer(CustomerDto objCustomer)
        {
            ResponseServiceDto<bool> responseServiceDto = new ResponseServiceDto<bool>();
            var objEntity = _unitOfWork.CustomerRepository.Find(c => c.CustomerId.Equals(objCustomer.CustomerId));

            if (objEntity == null)
            {
                responseServiceDto.Message = "Customer Not Found";
                responseServiceDto.Result = false;
                responseServiceDto.HttpStatusCode = HttpStatusCode.NotFound;

                return responseServiceDto;
            }

            _postService.VerifyCustomerPosts(objCustomer);
            _unitOfWork.CustomerRepository.Delete(objEntity);

            var result = _unitOfWork.Save();

            if (result == 1)
            {
                responseServiceDto.Message = "Customer Deleted";
                responseServiceDto.Result = true;
                responseServiceDto.HttpStatusCode = HttpStatusCode.OK;

                return responseServiceDto;
            }

            responseServiceDto.Message = "Customer Not Deleted";
            responseServiceDto.Result = false;
            responseServiceDto.HttpStatusCode = HttpStatusCode.BadRequest;

            return responseServiceDto;

        }


    }
}
