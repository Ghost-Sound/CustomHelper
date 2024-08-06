using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.Entity.Infrastructure;
using System.Reflection.Metadata;
namespace CustomHelper.EFcore.TestHelpers
{
    public static class DbSetMockHelper
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return mockSet;
        }

        public static Mock<DbSet<T>> CreateMockDbSetAsync<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IDbAsyncEnumerable<T>>()
                    .Setup(m => m.GetAsyncEnumerator())
                    .Returns(new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return mockSet;
        }
    }
}
