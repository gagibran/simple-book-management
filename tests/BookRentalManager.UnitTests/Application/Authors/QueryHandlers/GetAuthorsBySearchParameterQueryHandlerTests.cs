using BookRentalManager.Application.Authors.Queries;
using BookRentalManager.Application.Authors.QueryHandlers;

namespace BookRentalManager.UnitTests.Application.Authors.QueryHandlers;

public sealed class GetAuthorsBySearchParameterQueryHandlerTests
{
    private readonly Author _author;
    private readonly List<Author> _authors;
    private readonly Mock<IRepository<Author>> _authorRepositoryStub;
    private readonly Mock<IMapper<Author, GetAuthorDto>> _getAuthorDtoMapperStub;
    private readonly GetAuthorsBySearchParameterQueryHandler _getAuthorsBySearchParameterQueryHandler;
    private readonly GetAuthorDto _getAuthorDto;

    public GetAuthorsBySearchParameterQueryHandlerTests()
    {
        _author = TestFixtures.CreateDummyAuthor();
        _authors = new List<Author> { _author };
        _getAuthorDto = new(Guid.NewGuid(), _author.FullName, new List<GetBookFromAuthorDto>());
        _getAuthorDtoMapperStub = new();
        _authorRepositoryStub = new();
        _getAuthorsBySearchParameterQueryHandler = new(
            _authorRepositoryStub.Object,
            _getAuthorDtoMapperStub.Object);
        _getAuthorDtoMapperStub
            .Setup(getAuthorDtoMapper => getAuthorDtoMapper.Map(It.IsAny<Author>()))
            .Returns(_getAuthorDto);
    }

    [Fact]
    public async Task HandleAsync_WithoutAnyAuthorsWithSearchParameter_ReturnsEmptyList()
    {
        // Arrange:
        var getAuthorsBySearchParameterQuery = new GetAuthorsBySearchParameterQuery(
            TestFixtures.PageIndex,
            TestFixtures.TotalItemsPerPage,
            "Name");
        _authorRepositoryStub
            .Setup(authorRepository => authorRepository.GetAllBySpecificationAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Specification<Author>>(),
                default))
            .ReturnsAsync(new List<Author>());

        // Act:
        Result<IReadOnlyList<GetAuthorDto>> handlerResult = await _getAuthorsBySearchParameterQueryHandler.HandleAsync(
            getAuthorsBySearchParameterQuery,
            default);

        // Assert:
        Assert.Empty(handlerResult.Value);
    }

    [Fact]
    public async Task HandleAsync_WithAtLeastOneAuthorWithSearchParameter_ReturnsListWithMatchingAuthor()
    {
        // Arrange:
        var getAuthorsBySearchParameterQuery = new GetAuthorsBySearchParameterQuery(
            TestFixtures.PageIndex,
            TestFixtures.TotalItemsPerPage,
            _author.FullName.CompleteName);
        _authorRepositoryStub
            .Setup(authorRepository => authorRepository.GetAllBySpecificationAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Specification<Author>>(),
                default))
            .ReturnsAsync(_authors);

        // Act:
        Result<IReadOnlyList<GetAuthorDto>> handlerResult = await _getAuthorsBySearchParameterQueryHandler.HandleAsync(
            getAuthorsBySearchParameterQuery,
            default);

        // Assert:
        Assert.Equal(_getAuthorDto, handlerResult.Value.FirstOrDefault());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task HandleAsync_WithEmptySearchParameter_ReturnsListWithAllAuthors(string searchParam)
    {
        // Arrange:
        var getAuthorsBySearchParameterQuery = new GetAuthorsBySearchParameterQuery(
            TestFixtures.PageIndex,
            TestFixtures.TotalItemsPerPage,
            searchParam);
        _authors.Add(new Author(FullName.Create("Sarah", "Smith").Value));
        _authorRepositoryStub
            .Setup(authorRepository => authorRepository.GetAllBySpecificationAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Specification<Author>>(),
                default))
            .ReturnsAsync(_authors);

        // Act:
        Result<IReadOnlyList<GetAuthorDto>> handlerResult = await _getAuthorsBySearchParameterQueryHandler.HandleAsync(
            getAuthorsBySearchParameterQuery,
            default);

        // Assert:
        Assert.Equal(2, handlerResult.Value.Count);
    }
}
