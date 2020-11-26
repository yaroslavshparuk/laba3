using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Models
{
   public class User
    {
        public User(string externalId, string name)
        {
            ExternalId = externalId;
            Name = name;
        }
        public virtual int Id { get; set; }
        public virtual string ExternalId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
    }
}
