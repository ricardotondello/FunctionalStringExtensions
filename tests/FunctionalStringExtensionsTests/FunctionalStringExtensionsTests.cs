using FluentAssertions;
using FunctionalStringExtensions;

namespace FunctionalStringExtensionsTests;

public class FunctionalStringExtensionsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldReturnDefaultValueWhenStringIsInvalid(string? value)
    {
        //Act
        var result = value.OrDefault("abc");

        //Assert
        result.Should().Be("abc");
    }

    [Theory]
    [InlineData("test value")]
    public async Task ShouldReturnValueAsyncWhenStringIsValid(string value)
    {
        //Act
        var result = await value.OrDefaultAsync(Task.FromResult("abc"));

        //Assert
        result.Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task ShouldReturnDefaultValueAsyncWhenStringIsInvalid(string? value)
    {
        //Act
        var result = await value.OrDefaultAsync(Task.FromResult("abc"));

        //Assert
        result.Should().Be("abc");
    }

    [Theory]
    [InlineData("test value")]
    public void ShouldReturnValueWhenStringIsValid(string value)
    {
        //Act
        var result = value.OrDefault("abc");

        //Assert
        result.Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldReturnFuncValueWhenStringIsInvalid(string? value)
    {
        //Act
        var result = value.WhenNullOrEmpty(() => "abc");

        //Assert
        result.Should().Be("abc");
    }

    [Theory]
    [InlineData("test value")]
    public void ShouldNotReturnFuncWhenStringIsValid(string value)
    {
        //Act
        var result = value.WhenNullOrEmpty(() => "abc");

        //Assert
        result.Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task ShouldReturnFuncValueAsyncWhenStringIsInvalid(string? value)
    {
        //Act
        var result = await value.WhenNullOrEmptyAsync(() => Task.FromResult("abc"));

        //Assert
        result.Should().Be("abc");
    }

    [Theory]
    [InlineData("test value")]
    public async Task ShouldNotReturnFuncAsyncWhenStringIsValid(string value)
    {
        //Act
        var result = await value.WhenNullOrEmptyAsync(() => Task.FromResult("abc"));

        //Assert
        result.Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldExecuteActionValueWhenStringIsInvalid(string? value)
    {
        //Arrange
        var initValue = 0;

        //Act
        value.OnNullOrEmpty(Act);

        //Assert
        initValue.Should().BeGreaterThan(0);
        return;

        void Act()
        {
            initValue++;
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task ShouldExecuteActionValueAsyncWhenStringIsInvalid(string? value)
    {
        //Arrange
        var initValue = 0;

        //Act
        await value.OnNullOrEmptyAsync(Task.Run(() => Act(ref initValue)));

        //Assert
        initValue.Should().BeGreaterThan(0);
        return;

        void Act(ref int change)
        {
            change++;
        }
    }
}