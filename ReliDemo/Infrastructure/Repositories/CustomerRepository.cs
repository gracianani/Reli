using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;
using System.Linq.Expressions;

namespace ReliDemo.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private xz2013Entities db = new xz2013Entities();

        public IQueryable<Customer> SearchFor(Expression<Func<Customer, bool>> predicate)
        {
            return db.Customers.Where(predicate);
        }
    }
}