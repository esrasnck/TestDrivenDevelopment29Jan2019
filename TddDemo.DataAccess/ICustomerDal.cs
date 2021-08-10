using System.Collections.Generic;
using TddDemo.Entities;

namespace TddDemo.DataAccess
{
    public interface ICustomerDal
    {
        List<Customer> GetAll();
    }
}