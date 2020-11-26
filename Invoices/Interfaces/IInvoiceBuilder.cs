using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Interfaces
{
   public interface IInvoiceBuilder
    {
        Task Build();
    }
}
