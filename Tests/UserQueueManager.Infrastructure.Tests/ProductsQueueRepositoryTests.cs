using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.Extensions.Options;
using UserQueueManager.FileRepository.Storage;
using UserQueueManager.Contracts.Data;
using System.Collections;
using NuGet.Frameworks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using UserQueueManager.Contracts.Web.Data;


namespace UserQueueManager.Infrastructure.Tests;

public class ProductsQueueRepositoryTests
{
    private readonly Fixture _fixture;

    public ProductsQueueRepositoryTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        var options = _fixture.Freeze<ProductsQueueFileRepositoryOptions>(cmb => cmb.With(o => o.StorageFileName, "TestFile.jsn"));
        var optionsInf = _fixture.Freeze<IOptions<ProductsQueueFileRepositoryOptions>>().Value.Returns(options);
    }

    [Fact]
    public async Task ProductsQueueFileRepository_Test()
    {
        var repository = _fixture.Create<ProductsQueueRepository>();
        var first = GenerateProductQueue(5);
        var second = GenerateProductQueue(3);
        var third = GenerateProductQueue(1);
        var dictionary = new Dictionary<Guid, Queue<User>>();
        dictionary.Add(first.IdProduct, ListToQueue(first.UsersQueue));
        dictionary.Add(second.IdProduct, ListToQueue(second.UsersQueue));
        dictionary.Add(third.IdProduct, ListToQueue(third.UsersQueue));

        Func<Task> action = () => repository.SaveStorage(dictionary, CancellationToken.None);

        await action.Should().NotThrowAsync();

        var loadedDictionary = await repository.LoadStorage(CancellationToken.None);
        loadedDictionary.Should().NotBeNullOrEmpty();

        Assert.True(loadedDictionary.TryGetValue(first.IdProduct, out _));
        Assert.True(loadedDictionary.TryGetValue(second.IdProduct, out _));
        Assert.True(loadedDictionary.TryGetValue(third.IdProduct, out _));
    }

    private SetProductQueueRequest GenerateProductQueue(int queuItemsCount)
     => new SetProductQueueRequest
     {
         IdProduct = _fixture.Create<Guid>(),
         UsersQueue = _fixture.CreateMany<User>(queuItemsCount).ToList(),
     };

    private Queue<T> ListToQueue<T>(IEnumerable<T> items)
       => new(items);
}