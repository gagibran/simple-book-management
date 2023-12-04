namespace BookRentalManager.Application.Dtos;

public sealed class GetAuthorFromBookDto(FullName fullName)
{
    public string FullName { get; } = fullName.ToString();
}
