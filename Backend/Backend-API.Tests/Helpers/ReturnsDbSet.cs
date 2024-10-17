using Microsoft.EntityFrameworkCore;
using Moq;

public static class DbSetMockingExtensions
{
    public static DbSet<T> ReturnsDbSet<T>(this Mock<DbSet<T>> dbSetMock, List<T> data) where T : class
    {
        var queryableData = data.AsQueryable();
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
        return dbSetMock.Object;
    }
}