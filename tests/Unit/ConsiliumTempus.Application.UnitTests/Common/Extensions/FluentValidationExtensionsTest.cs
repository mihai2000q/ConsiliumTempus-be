using System.Linq.Expressions;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace ConsiliumTempus.Application.UnitTests.Common.Extensions;

public class FluentValidationExtensionsTest
{
    #region Setup

    private sealed record Item(
        string Name,
        int Count,
        decimal Amount,
        bool Flag,
        DateTime Date,
        ProjectLifecycle Lifecycle,
        ProjectStatusType Status,
        Guid Identifier);

    private sealed class ItemFilterProperty(string identifier, Expression<Func<Item, object>> selector)
        : FilterProperty<Item>(identifier, selector);

    private sealed record ItemOrderProperty(string Key, Expression<Func<Item, object?>> Selector)
        : OrderProperty<Item>(Key, Selector);

    private sealed record Request(List<string> Search, List<string>? OrderBy = null);

    private sealed class RequestValidator : AbstractValidator<Request>
    {
        private static readonly List<OrderProperty<Item>> OrderProperties =
        [
            new ItemOrderProperty(nameof(Item.Name), i => i.Name),
            new ItemOrderProperty(nameof(Item.Count), i => i.Count)
        ];

        private static readonly List<FilterProperty<Item>> FilterProperties =
        [
            new ItemFilterProperty(nameof(Item.Name), i => i.Name),
            new ItemFilterProperty(nameof(Item.Count), i => i.Count),
            new ItemFilterProperty(nameof(Item.Amount), i => i.Amount),
            new ItemFilterProperty(nameof(Item.Flag), i => i.Flag),
            new ItemFilterProperty(nameof(Item.Date), i => i.Date),
            new ItemFilterProperty(nameof(Item.Lifecycle), i => i.Lifecycle),
            new ItemFilterProperty(nameof(Item.Status), i => i.Status),
            new ItemFilterProperty(nameof(Item.Identifier), i => i.Identifier)
        ];

        public RequestValidator()
        {
            RuleFor(r => r.Search).HasSearchFormat(FilterProperties);
            RuleFor(r => r.OrderBy!).HasOrderByFormat(OrderProperties).When(r => r.OrderBy is not null);
        }
    }

    private readonly RequestValidator _uut = new();

    private sealed record Holder<T>(T Value);

    private static Task<ValidationResult> Validate<T>(
        T value,
        Func<IRuleBuilder<Holder<T>, T>, IRuleBuilderOptions<Holder<T>, T>> rule)
    {
        var validator = new InlineValidator<Holder<T>>();
        rule(validator.RuleFor(h => h.Value));
        return validator.ValidateAsync(new Holder<T>(value));
    }

    #endregion

    [Fact]
    public async Task IsPassword_WhenPasswordHasEverything_ShouldBeValid()
    {
        // Arrange - nothing

        // Act
        var outcome = await Validate("Abcdefg12", rb => rb.IsPassword());

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("Abcdef12", 1)] // not longer than 8 characters
    [InlineData("abcdefgh1", 1)] // no uppercase letter
    [InlineData("ABCDEFGH1", 1)] // no lowercase letter
    [InlineData("Abcdefghi", 1)] // no number
    [InlineData("", 4)]
    public async Task IsPassword_WhenPasswordBreaksTheRules_ShouldReturnAnErrorForEachOne(
        string password,
        int expectedErrors)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate(password, rb => rb.IsPassword());

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().HaveCount(expectedErrors);
    }

    [Theory]
    [InlineData("john@mail.com")]
    [InlineData("john.doe_1-2@some.domain.org")]
    public async Task IsEmail_WhenEmailIsValid_ShouldBeValid(string email)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate(email, rb => rb.IsEmail());

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("john.doe")]
    [InlineData("@mail.com")]
    [InlineData("john@mail")]
    [InlineData("john@mail.COM")]
    public async Task IsEmail_WhenEmailIsInvalid_ShouldBeInvalid(string email)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate(email, rb => rb.IsEmail());

        // Assert
        outcome.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(-5000, true)]
    [InlineData(-1, true)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    public async Task IsPastDate_ShouldOnlyAcceptDatesBeforeToday(int daysFromToday, bool expectedIsValid)
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(daysFromToday);

        // Act
        var outcome = await Validate<DateOnly?>(date, rb => rb.IsPastDate());

        // Assert
        outcome.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public async Task IsPastDate_WhenThereIsNoDate_ShouldBeValid()
    {
        // Arrange - nothing

        // Act
        var outcome = await Validate<DateOnly?>(null, rb => rb.IsPastDate());

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("admin")]
    [InlineData("MEMBER")]
    [InlineData("vIEW")]
    public async Task IsWorkspaceRole_WhenRoleExists_ShouldBeValidRegardlessOfTheCasing(string role)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate(role, rb => rb.IsWorkspaceRole());

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("owner")]
    [InlineData("super admin")]
    public async Task IsWorkspaceRole_WhenRoleDoesNotExist_ShouldBeInvalid(string role)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate(role, rb => rb.IsWorkspaceRole());

        // Assert
        outcome.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData(null)]
    public async Task IsCurrencyCode_WhenCodeIsValidOrMissing_ShouldBeValid(string? currencyCode)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate(currencyCode, rb => rb.IsCurrencyCode());

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("usd")]
    [InlineData("US")]
    [InlineData("USDD")]
    [InlineData("US1")]
    public async Task IsCurrencyCode_WhenCodeIsInvalid_ShouldBeInvalid(string currencyCode)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate<string?>(currencyCode, rb => rb.IsCurrencyCode());

        // Assert
        outcome.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("#FF2233")]
    [InlineData("#abc")]
    [InlineData("#A1b2C3")]
    public async Task IsColor_WhenColorIsValid_ShouldBeValid(string color)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate(color, rb => rb.IsColor());

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("red")]
    [InlineData("FF2233")]
    [InlineData("#GG2233")]
    [InlineData("#12345")]
    [InlineData("#1234567")]
    public async Task IsColor_WhenColorIsInvalid_ShouldBeInvalid(string color)
    {
        // Arrange - parameterized

        // Act
        var outcome = await Validate(color, rb => rb.IsColor());

        // Assert
        outcome.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("name eq John")]
    [InlineData("name ct oh")]
    [InlineData("count gt 5")]
    [InlineData("amount lte 12")]
    [InlineData("flag eq true")]
    [InlineData("flag neq false")]
    [InlineData("date gte 2024-01-31")]
    [InlineData("lifecycle eq Active")]
    [InlineData("lifecycle neq upcoming")]
    [InlineData("status eq OnTrack")]
    [InlineData("status neq atrisk")]
    public async Task HasSearchFormat_WhenFilterIsValid_ShouldBeValid(string filter)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(new Request([filter]));

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("count gt abc")]
    [InlineData("amount gt abc")]
    [InlineData("flag eq maybe")]
    [InlineData("date gt notadate")]
    [InlineData("lifecycle eq Nothing")]
    [InlineData("status eq Nothing")]
    public async Task HasSearchFormat_WhenValueCannotBeParsedToThePropertyType_ShouldBeInvalid(string filter)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(new Request([filter]));

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().ContainSingle();
    }

    [Theory]
    [InlineData("name gt something")]
    [InlineData("flag lt true")]
    [InlineData("lifecycle gt Active")]
    [InlineData("identifier eq 5b7e2c4a-9c0e-4a77-8d6c-3f1d1d0a1f10")]
    public async Task HasSearchFormat_WhenOperatorIsNotSupportedByThePropertyType_ShouldBeInvalid(string filter)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(new Request([filter]));

        // Assert
        outcome.IsValid.Should().BeFalse();
        outcome.Errors.Should().ContainSingle();
    }

    [Theory]
    [InlineData("name.asc")]
    [InlineData("count.desc")]
    public async Task HasOrderByFormat_WhenOrderIsValid_ShouldBeValid(string order)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(new Request([], [order]));

        // Assert
        outcome.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("name")]
    [InlineData("name.descending")]
    [InlineData("Name.asc")]
    [InlineData("amount.asc")]
    [InlineData("name.asc", "name.desc")]
    public async Task HasOrderByFormat_WhenOrderIsInvalid_ShouldBeInvalid(params string[] orders)
    {
        // Arrange - parameterized

        // Act
        var outcome = await _uut.ValidateAsync(new Request([], orders.ToList()));

        // Assert
        outcome.IsValid.Should().BeFalse();
    }
}
