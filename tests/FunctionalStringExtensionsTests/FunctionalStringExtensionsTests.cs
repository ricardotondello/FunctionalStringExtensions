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

    [Theory]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData("ICH MUß EINIGE CRÈME BRÛLÉE HABEN", "ich_mu_einige_cr_me_br_l_e_haben")]
    [InlineData("I'm a cute string/\"\"\\/", "i_m_a_cute_string")]
    public void ShouldMakeStringAsSlugs(string? text, string expected)
    {
        //Arrange Act
        var result = text.ToSlug();
        
        //Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToEnumShouldTransformToEnumStruct()
    {
        //Arrange
        var stringEnum = "Value1";
        
        //Act
        var result = stringEnum.ToEnum<FakeEnum>();
        
        //Assert
        result.Should().Be(FakeEnum.Value1);
    }
    
    [Theory]
    [InlineData(FakeEnum.Value2)]
    public void ToEnumShouldReturnChosenDefaultValueWhenStringIsInvalid(FakeEnum fakeEnum)
    {
        //Arrange
        var stringEnum = "Invalid";
        
        //Act
        var result = stringEnum.ToEnum<FakeEnum>(fakeEnum);
        
        //Assert
        result.Should().Be(fakeEnum);
    }
    
    [Fact]
    public void ToEnumShouldReturnDefaultValueWhenStringIsInvalid()
    {
        //Arrange
        var stringEnum = "Invalid";
        
        //Act
        var result = stringEnum.ToEnum<FakeEnum>();
        
        //Assert
        result.Should().Be(FakeEnum.Value1);
    }
}

public enum FakeEnum
{
    Value1,
    Value2
}