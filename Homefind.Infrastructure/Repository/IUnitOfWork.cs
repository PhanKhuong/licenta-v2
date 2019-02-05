using System;
using System.Threading.Tasks;

namespace Homefind.Infrastructure.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        EstateDbContext Context { get; }

        Task SaveChangesAsync();
    }
}
