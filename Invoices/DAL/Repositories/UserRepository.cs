using Invoices.Data.Entities.UserAggregate;
using Invoices.Data.Records;
using Invoices.Data.Repositories;
using Invoices.EF;
using Invoices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InvoiceContext _context;
        public UserRepository(InvoiceContext context)
        {
            _context = context;
        }
        public User GetOrCreateUser(UserRecord userRecord)
        {
            if (userRecord == null)
                return null;
            var user = _context.Users.FirstOrDefault(x => x.Name == userRecord.Name);
            if (user == null)
            {
                var newUser = _context.Users.Add(new User(userRecord.Id, userRecord.Name)).Entity;
                _context.SaveChanges();
                return newUser;
            }
            else
                return user;
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}
