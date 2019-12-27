using casln.Infrastructure.Persistence;
using System;

namespace casln.Application.UnitTests.Common
{
    public class CommandTestBase : IDisposable
    {
        public CommandTestBase()
        {
            Context = ApplicationDbContextFactory.Create();
        }

        public ApplicationDbContext Context { get; }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }
}