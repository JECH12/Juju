using Business.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public  interface ICustomer
    {
        CustomerDto GetCostumer(CustomerDto customer);
        List<CustomerDto> ListCustomer();
        ResponseServiceDto<bool> InsertCustomer(CustomerDto objCustomer);

        bool CustomerNameExist(CustomerDto objCustomer);

        ResponseServiceDto<bool> UpdateCustomer(CustomerDto objCustomer);

        ResponseServiceDto<bool> DeleteCustomer(CustomerDto objCustomer);
    }
}
