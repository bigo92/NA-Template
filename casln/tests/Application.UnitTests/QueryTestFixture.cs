using System;
using AutoMapper;
using casln.Application.Common.Mappings;
using casln.Infrastructure.Persistence;
using Xunit;

namespace casln.Application.UnitTests.Common
{
    public sealed class QueryTestFixture : IDisposable
    {
        public QueryTestFixture()
        {
            Context = ApplicationDbContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }
        public ApplicationDbContext Context { get; }

        public IMapper Mapper { get; }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryTests")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}