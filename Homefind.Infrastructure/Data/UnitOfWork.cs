using System.Threading.Tasks;

namespace Homefind.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public EstateDbContext Context { get; }

        public UnitOfWork(EstateDbContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
