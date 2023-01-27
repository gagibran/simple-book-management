using BookRentalManager.Application.Customers.Commands;
using BookRentalManager.Application.Customers.Queries;

namespace BookRentalManager.Api.Controllers;

[Route("api/[controller]")]
public sealed class CustomerController : BaseController
{
    public CustomerController(IDispatcher dispatcher, ILogger<CustomerController> customerControllerLogger)
        : base(dispatcher, customerControllerLogger)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetCustomerDto>>> GetCustomersAsync(
        CancellationToken cancellationToken,
        int pageIndex = 1,
        int totalItemsPerPage = 50,
        [FromQuery(Name = "search")] string searchParameter = "")
    {
        var getCustomersBySearchParameterQuery = new GetCustomersBySearchParameterQuery(
            pageIndex,
            totalItemsPerPage,
            searchParameter);
        Result<IReadOnlyList<GetCustomerDto>> getAllCustomersResult = await _dispatcher.DispatchAsync<IReadOnlyList<GetCustomerDto>>(
            getCustomersBySearchParameterQuery,
            cancellationToken);
        return Ok(getAllCustomersResult.Value);
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetCustomerByIdAsync))]
    public async Task<ActionResult<GetCustomerDto>> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Result<GetCustomerDto> getCustomerByIdResult = await _dispatcher.DispatchAsync<GetCustomerDto>(
            new GetCustomerByIdQuery(id),
            cancellationToken);
        if (!getCustomerByIdResult.IsSuccess)
        {
            _baseControllerLogger.LogError(getCustomerByIdResult.ErrorMessage);
            return NotFound(getCustomerByIdResult.ErrorMessage);
        }
        return Ok(getCustomerByIdResult.Value);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomerAsync(CreateCustomerDto createCustomerDto, CancellationToken cancellationToken)
    {
        Result<CustomerCreatedDto> createCustomerResult = await _dispatcher.DispatchAsync<CustomerCreatedDto>(
            new CreateCustomerCommand(createCustomerDto),
            cancellationToken);
        if (!createCustomerResult.IsSuccess)
        {
            _baseControllerLogger.LogError(createCustomerResult.ErrorMessage);
            return BadRequest(createCustomerResult.ErrorMessage);
        }

        return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = createCustomerResult.Value!.Id }, createCustomerResult.Value);
    }
}
