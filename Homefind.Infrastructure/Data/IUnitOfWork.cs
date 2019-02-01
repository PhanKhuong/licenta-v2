using System;
using System.Threading.Tasks;

namespace Homefind.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        EstateDbContext Context { get; }

        Task SaveChangesAsync();
    }
}
