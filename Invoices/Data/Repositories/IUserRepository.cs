using Invoices.Data.Entities.UserAggregate;
using Invoices.Data.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Data.Repositories
{
   public interface IUserRepository
    {
        User GetOrCreateUser(UserRecord userRecord);
        User GetUser(int id);
    }
}
