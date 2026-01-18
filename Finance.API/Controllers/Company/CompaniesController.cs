using Finance.API.Models.Queries.Common;
using Finance.API.Models.Queries.Companies;
using Finance.Application.Features.Companies.Commands.CreateCompany;
using Finance.Application.Features.Companies.Commands.RestoreCompany;
using Finance.Application.Features.Companies.Commands.SoftDeleteCompany;
using Finance.Application.Features.Companies.Commands.UpdateCompany;
using Finance.Application.Features.Companies.Queries.GetAllCompanies;
using Finance.Application.Features.Companies.Queries.GetCompanyById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finance.API.Controllers.Company;

[Route("api/[controller]")]
[ApiController]
public sealed class CompaniesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateCompanyRequestDto createCompanyDto,
    CancellationToken cancellationToken)
    {
        var companyId = await mediator.Send(createCompanyDto, cancellationToken);

        return CreatedAtAction(
            nameof(Create),
            new { id = companyId },
            companyId);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] PagingParameters paging,
        [FromQuery] CompanyQueryParameters query,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAllCompaniesQuery(
                paging.PageNumber,
                paging.PageSize,
                query.IsActive,
                query.Code,
                query.Search,
                query.SortBy,
                query.SortDirection),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
    Guid id,
    CancellationToken cancellationToken)
    {
        var company = await mediator.Send(
            new GetCompanyByIdQuery(id),
            cancellationToken);

        return Ok(company);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
    Guid id,
    [FromBody] UpdateCompanyRequestDto request,
    CancellationToken ct)
    {
        var command = new UpdateCompanyCommand(
            id,
            request.Name,
            request.Code,
            request.BaseCurrencyCode);

        await mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> SoftDelete(
    Guid id,
    CancellationToken ct)
    {
        await mediator.Send(
            new SoftDeleteCompanyCommand(id),
            ct);

        return NoContent();
    }

    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(
    Guid id,
    CancellationToken ct)
    {
        await mediator.Send(
            new RestoreCompanyCommand(id),
            ct);

        return NoContent();
    }
}
