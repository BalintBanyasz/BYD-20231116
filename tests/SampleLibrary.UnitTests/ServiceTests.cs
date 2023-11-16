using FluentAssertions;
using NSubstitute;

namespace SampleLibrary.UnitTests;

public class ServiceTests
{
    [Fact]
    public async Task FunctionAAsync_CallsDependencyWithCorrectArgument()
    {
        // Arrange
        var mockDependency = Substitute.For<IDependency>();
        mockDependency.FunctionAAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult("x"));

        var sut = new Service(mockDependency);

        // Act
        await sut.FunctionAAsync("Test");

        // Assert
        await mockDependency.Received(1).FunctionAAsync("Test", default);
    }

    [Fact]
    public async Task FunctionAAsync_CallsDependencyForAllArguments()
    {
        // Arrange
        var mockDependency = Substitute.For<IDependency>();
        mockDependency.FunctionAAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult("x"));

        mockDependency.FunctionAAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult("y"));

        var sut = new Service(mockDependency);

        // Act
        await sut.FunctionAAsync("Test", "AnotherValue", "SPECIAL_VALUE");

        // Assert
        await mockDependency.Received(1).FunctionAAsync("Test", default);
        await mockDependency.Received(1).FunctionAAsync("AnotherValue", default);
    }

    [Fact]
    public async Task FunctionAAsync_DoesNotCallDependencyForMagicStringArgument()
    {
        // Arrange
        var mockDependency = Substitute.For<IDependency>();

        var sut = new Service(mockDependency);

        // Act
        await sut.FunctionAAsync("SPECIAL_VALUE");

        // Assert
        await mockDependency.DidNotReceive().FunctionAAsync("SPECIAL_VALUE", default);
    }

    [Fact]
    public async Task FunctionAAsync_PropagatesInvalidOperationException()
    {
        // Arrange
        var mockDependency = Substitute.For<IDependency>();
        mockDependency.When(x => x.FunctionAAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new InvalidOperationException());

        var sut = new Service(mockDependency);

        // Act
        var act = async () => await sut.FunctionAAsync("InvalidArgument");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
