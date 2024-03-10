using ConsiliumTempus.Application.Common.Behaviors;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Project;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ConsiliumTempus.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTest
{
    #region Setup

    private readonly IValidator<CreateProjectCommand> _validator;
    private readonly RequestHandlerDelegate<ErrorOr<CreateProjectResult>> _nextBehavior;
    private ValidationBehavior<CreateProjectCommand, ErrorOr<CreateProjectResult>> _uut;

    public ValidationBehaviorTest()
    {
        _validator = Substitute.For<IValidator<CreateProjectCommand>>();
        _nextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<CreateProjectResult>>>();
        _uut = new(_validator);
    }

    #endregion

    [Fact]
    public async Task WhenCommandIsValid_ShouldValidateAndInvokeNextBehavior()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateCreateProjectCommand();

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult());

        var result = new CreateProjectResult();
        _nextBehavior
            .Invoke()
            .Returns(result);

        // Act
        var outcome = await _uut.Handle(command, _nextBehavior, default);

        // Assert
        await _validator
            .Received(1)
            .ValidateAsync(Arg.Any<CreateProjectCommand>());
        await _nextBehavior
            .Received(1)
            .Invoke();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(result);
    }

    [Fact]
    public async Task WhenCommandIsInvalid_ShouldReturnListOfErrors()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateCreateProjectCommand();

        var errors = new List<ValidationFailure> { new("Name", "Name is required") };
        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult(errors));

        var result = new CreateProjectResult();
        _nextBehavior
            .Invoke()
            .Returns(result);

        // Act
        var outcome = await _uut.Handle(command, _nextBehavior, default);

        // Assert
        await _validator
            .Received(1)
            .ValidateAsync(Arg.Any<CreateProjectCommand>());
        _nextBehavior.DidNotReceive();

        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(errors.Count);
        outcome.Errors.Should().AllSatisfy(e => e.Type.Should().Be(ErrorType.Validation));
        outcome.Errors.Zip(errors)
            .ToList()
            .ForEach(x =>
            {
                x.First.Code.Should().Be(x.Second.PropertyName);
                x.First.Description.Should().Be(x.Second.ErrorMessage);
            });
    }

    [Fact]
    public async Task WhenValidatorIsNull_ShouldInvokeNextBehavior()
    {
        // Arrange
        _uut = new ValidationBehavior<CreateProjectCommand, ErrorOr<CreateProjectResult>>();

        var command = ProjectCommandFactory.CreateCreateProjectCommand();

        var result = new CreateProjectResult();
        _nextBehavior
            .Invoke()
            .Returns(result);

        // Act
        var outcome = await _uut.Handle(command, _nextBehavior, default);

        // Assert
        _validator.DidNotReceive();
        await _nextBehavior
            .Received(1)
            .Invoke();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(result);
    }
}