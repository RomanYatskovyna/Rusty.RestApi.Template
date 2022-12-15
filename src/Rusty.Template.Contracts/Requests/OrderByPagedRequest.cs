using FluentValidation;
using Rusty.Template.Contracts.SubTypes;

// ReSharper disable All

namespace Rusty.Template.Contracts.Requests;

/// <summary>
///     The order by paged request
/// </summary>
public sealed record OrderByPagedRequest
{
    /// <summary>
    ///     Gets or sets the value of the page data
    /// </summary>
    public PageData? PageData { get; set; }

    /// <summary>
    ///     Gets or sets the value of the order by data
    /// </summary>
    public OrderByData? OrderByData { get; set; }
}

/// <summary>
///     The order by paged request validator class
/// </summary>
/// <seealso cref="AbstractValidator{OrderByPagedRequest}" />
public sealed class OrderByPagedRequestValidator : AbstractValidator<OrderByPagedRequest>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="OrderByPagedRequestValidator" /> class
    /// </summary>
    public OrderByPagedRequestValidator()
    {
        RuleFor(w => w.PageData).SetValidator(new PageDataValidator()!).When(item => item.PageData is not null);
        RuleFor(w => w.OrderByData).SetValidator(new OrderByDataValidator()!)
            .When(item => item.OrderByData is not null);
    }
}