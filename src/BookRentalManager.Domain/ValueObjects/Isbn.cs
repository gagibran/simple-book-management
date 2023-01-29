namespace BookRentalManager.Domain.ValueObjects;

public sealed class Isbn : ValueObject
{
    public string IsbnValue { get; }

    private Isbn()
    {
        IsbnValue = string.Empty;
    }

    private Isbn(string isbnValue)
    {
        IsbnValue = isbnValue;
    }

    public static Result<Isbn> Create(string isbnValue)
    {
        string formattedIsbn = Regex.Replace(isbnValue, @"\s+|-+", "").ToUpper();
        if (formattedIsbn.Length != 10 && formattedIsbn.Length != 13)
        {
            return Result.Fail<Isbn>(nameof(Create), "Invalid ISBN format.");
        }
        return Result.Success<Isbn>(new Isbn(isbnValue.Trim()));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return IsbnValue;
    }
}
