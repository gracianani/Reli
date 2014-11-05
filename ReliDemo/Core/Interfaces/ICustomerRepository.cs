using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReliDemo.Models;
using System.Linq.Expressions;

namespace ReliDemo.Core.Interfaces
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> SearchFor(Expression<Func<Customer, bool>> predicate);
    }
}
