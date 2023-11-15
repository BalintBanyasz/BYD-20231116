using FluentAssertions;
using Moq;

namespace SampleLibrary.UnitTests;

public class ServiceTests
{
    [Fact]
    public async Task FunctionAAsync_CallsDependencyWithCorrectArgument()
    {
        // Arrange
        var mockDependency = new Mock<IDependency>();
        mockDependency.Setup(x => x.FunctionAAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("x");

        var sut = new Service(mockDependency.Object);

        // Act
        await sut.FunctionAAsync("Test");

        // Assert
        mockDependency.Verify(x => x.FunctionAAsync("Test", default), Times.Once());
    }

    [Fact]
    public async Task FunctionAAsync_CallsDependencyForAllArguments()
    {
        // Arrange
        var mockDependency = new Mock<IDependency>();
        mockDependency.SetupSequence(x => x.FunctionAAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("x")
            .ReturnsAsync("y");

        var sut = new Service(mockDependency.Object);

        // Act
        await sut.FunctionAAsync("Test", "AnotherValue", "SPECIAL_VALUE");

        // Assert
        mockDependency.Verify(x => x.FunctionAAsync("Test", default), Times.Once());
        mockDependency.Verify(x => x.FunctionAAsync("AnotherValue", default), Times.Once());
    }

    [Fact]
    public async Task FunctionAAsync_DoesNotCallDependencyForMagicStringArgument()
    {
        // Arrange
        var mockDependency = new Mock<IDependency>();

        var sut = new Service(mockDependency.Object);

        // Act
        await sut.FunctionAAsync("SPECIAL_VALUE");

        // Assert
        mockDependency.Verify(x => x.FunctionAAsync("SPECIAL_VALUE", default), Times.Never());
    }

    [Fact]
    public async Task FunctionAAsync_PropagatesInvalidOperationException()
    {
        // Arrange
        var mockDependency = new Mock<IDependency>();
        mockDependency.Setup(x => x.FunctionAAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var sut = new Service(mockDependency.Object);

        // Act
        var act = async () => await sut.FunctionAAsync("InvalidArgument");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
