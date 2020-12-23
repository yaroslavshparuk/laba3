using System;
using System.Threading.Tasks;

namespace Invoices.Interfaces
{
    public interface ILoadService
    {
        Task LoadAsync(DateTime date);
    }
}
