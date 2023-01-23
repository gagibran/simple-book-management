namespace BookRentalManager.Application.Mappers;

internal sealed class GetCustomerDtoMapper : IMapper<Customer, GetCustomerDto>
{
    private readonly IMapper<IReadOnlyList<Book>, IReadOnlyList<GetCustomerBookDto>> _getCustomerBookDtosMapper;

    internal GetCustomerDtoMapper(IMapper<IReadOnlyList<Book>, IReadOnlyList<GetCustomerBookDto>> getCustomerBookDtosMapper)
    {
        _getCustomerBookDtosMapper = getCustomerBookDtosMapper;
    }

    public GetCustomerDto Map(Customer customer)
    {
        return new GetCustomerDto(
            customer.Id,
            customer.FullName,
            customer.Email,
            customer.PhoneNumber,
            _getCustomerBookDtosMapper.Map(customer.Books),
            customer.CustomerStatus,
            customer.CustomerPoints);
    }
}
