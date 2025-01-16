using System.Text.RegularExpressions;
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
        Assert.Equal("abc", result);
    }

    [Theory]
    [InlineData("test value")]
    public async Task ShouldReturnValueAsyncWhenStringIsValid(string value)
    {
        //Act
        var result = await value.OrDefaultAsync(Task.FromResult("abc"));

        //Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task ShouldReturnDefaultValueAsyncWhenStringIsInvalid(string? value)
    {
        //Act
        var result = await value.OrDefaultAsync(Task.FromResult("abc"));

        //Assert
        Assert.Equal("abc", result);
    }

    [Theory]
    [InlineData("test value")]
    public void ShouldReturnValueWhenStringIsValid(string value)
    {
        //Act
        var result = value.OrDefault("abc");

        //Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldReturnFuncValueWhenStringIsInvalid(string? value)
    {
        //Act
        var result = value.WhenNullOrEmpty(() => "abc");

        //Assert
        Assert.Equal("abc", result);
    }

    [Theory]
    [InlineData("test value")]
    public void ShouldNotReturnFuncWhenStringIsValid(string value)
    {
        //Act
        var result = value.WhenNullOrEmpty(() => "abc");

        //Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task ShouldReturnFuncValueAsyncWhenStringIsInvalid(string? value)
    {
        //Act
        var result = await value.WhenNullOrEmptyAsync(() => Task.FromResult("abc"));

        //Assert
        Assert.Equal("abc", result);
    }

    [Theory]
    [InlineData("test value")]
    public async Task ShouldNotReturnFuncAsyncWhenStringIsValid(string value)
    {
        //Act
        var result = await value.WhenNullOrEmptyAsync(() => Task.FromResult("abc"));

        //Assert
        Assert.Equal(value, result);
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
        Assert.True(initValue > 0);
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
        Assert.True(initValue > 0);
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
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToSlugShouldThrowRegexMatchTimeoutExceptionWhenStringIsToLong()
    {
        //Arrange
        var text = string.Concat(Enumerable.Repeat("a", 10_000_000));

        // Act
        //Assert
        Assert.Throws<RegexMatchTimeoutException>(() => text.ToSlug());
    }

    [Fact]
    public void ToEnumShouldTransformToEnumStruct()
    {
        //Arrange
        const string stringEnum = "Value1";

        //Act
        var result = stringEnum.ToEnum<FakeEnum>();

        //Assert
        Assert.Equal(FakeEnum.Value1, result);
    }

    [Theory]
    [InlineData(FakeEnum.Value2)]
    public void ToEnumShouldReturnChosenDefaultValueWhenStringIsInvalid(FakeEnum fakeEnum)
    {
        //Arrange
        const string stringEnum = "Invalid";

        //Act
        var result = stringEnum.ToEnum<FakeEnum>(fakeEnum);

        //Assert
        Assert.Equal(fakeEnum, result);
    }

    [Fact]
    public void ToEnumShouldReturnDefaultValueWhenStringIsInvalid()
    {
        //Arrange
        const string stringEnum = "Invalid";

        //Act
        var result = stringEnum.ToEnum<FakeEnum>();

        //Assert
        Assert.Equal(FakeEnum.Value1, result);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc123def456ghi", "abcdefghi")]
    [InlineData("123456", "")]
    [InlineData("1a2b3c4d5e", "abcde")]
    [InlineData("12.8/0';@#!%^&*()a12,9", "a")]
    public void OnlyLettersShouldReturnOnlyLetters(string? value, string expected)
    {
        //Arrange & Act
        var result = value.OnlyLetters();

        //Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc123def456ghi", "123456")]
    [InlineData("abc", "")]
    [InlineData("123456", "123456")]
    [InlineData("1a2b3c4d5e", "12345")]
    [InlineData("12.8/0';@#!%^&*()a12,9", "1280129")]
    public void OnlyNumbersShouldReturnOnlyNumbers(string? value, string expected)
    {
        //Arrange & Act
        var result = value.OnlyNumbers();

        //Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc123def456ghi", "abc123def456ghi")]
    [InlineData("abc", "abc")]
    [InlineData("123456", "123456")]
    [InlineData("12.8/0';@#!%^&*()a12,9abc", "1280a129abc")]
    public void OnlyCharactersAndNumbersShouldReturnOnlyCharactersAndNumbers(string? value, string expected)
    {
        //Arrange & Act
        var result = value.OnlyCharactersAndNumbers();

        //Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc123def456ghi", "")]
    [InlineData("abc", "")]
    [InlineData("123456", "")]
    [InlineData("1a2b3c4d5e", "")]
    [InlineData("12.8/0';@#!%^&*()a12,9abc", "./';@#!%^&*(),")]
    public void OnlySpecialCharactersShouldReturnOnlySpecialCharacters(string? value, string expected)
    {
        //Arrange & Act
        var result = value.OnlySpecialCharacters();

        //Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not a valid url")]
    [InlineData("?")]
    [InlineData(" ")]
    [InlineData("variable1=true?")]
    public void ParseQueryString_ShouldReturnEmptyListWhenNotAValidInput(string? value)
    {
        var result = value.ParseQueryString();

        Assert.Empty(result);
    }

    [Theory]
    [InlineData("http://yoursite.com?variable1=false", "variable1", false)]
    [InlineData("http://yoursite.com?variable1=False", "variable1", false)]
    [InlineData("http://yoursite.com?variable1=1", "variable1", 1)]
    [InlineData("http://yoursite.com?variable1=19.86", "variable1", 19.86)]
    [InlineData("?variable1=true", "variable1", true)]
    [InlineData("?variable1=True", "variable1", true)]
    [InlineData("?variable1=1", "variable1", 1)]
    [InlineData("?variable1=0.77", "variable1", 0.77)]
    [InlineData("?variable1=test", "variable1", "test")]
    public void ParseQueryString_ConvertType_ShouldReturnListOfKeyValues(string? value, string expectedKey,
        object expectedValue)
    {
        var result = value.ParseQueryString(autoConvertType: true);

        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(expectedKey, result.First()
            .Key);
        Assert.Equal(expectedValue, result.First()
            .Value);
        Assert.IsType(expectedValue.GetType(), result.First()
            .Value);
    }

    [Theory]
    [InlineData("http://yoursite.com?variable1=false", "variable1", "false")]
    [InlineData("http://yoursite.com?variable1=False", "variable1", "False")]
    [InlineData("http://yoursite.com?variable1=1", "variable1", "1")]
    [InlineData("http://yoursite.com?variable1=19.86", "variable1", "19.86")]
    [InlineData("?variable1=true", "variable1", "true")]
    [InlineData("?variable1=True", "variable1", "True")]
    [InlineData("?variable1=1", "variable1", "1")]
    [InlineData("?variable1=0.77", "variable1", "0.77")]
    [InlineData("?variable1=test", "variable1", "test")]
    public void ParseQueryString_WithoutConversion_ShouldReturnListOfKeyValues(string? value, string expectedKey,
        string expectedValue)
    {
        var result = value.ParseQueryString(autoConvertType: false);

        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(expectedKey, result.First()
            .Key);
        Assert.Equal(expectedValue, result.First()
            .Value);
        Assert.IsType<string>(result.First()
            .Value);
    }

    [Theory]
    [InlineData("?variable1=15&variable2=is that a question?&variable3=true&variable4=33.88&variable5=",
        "variable1=15|variable2=is that a question?|variable3=true|variable4=33.88|variable5=")]
    public void ParseQueryString_ShouldReturnListOfKeyValues(string? value, string expectedPipedString)
    {
        var result = value.ParseQueryString(autoConvertType: false);

        var expected = expectedPipedString.Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Split('='))
            .ToDictionary(k => k[0], v => v[1]);

        Assert.NotEmpty(result);
        Assert.Equivalent(expected, result);
    }

    [Theory]
    [InlineData("?not a valid url", "not a valid url=")]
    [InlineData("?not a valid url&&&&&", "not a valid url=")]
    [InlineData("?not a valid url&&&&&=value", "not a valid url=|=value")]
    [InlineData(" ?not a valid url&&&&&=value ", "not a valid url=|=value ")]
    [InlineData("variable1=15&variable2=is that a question?&variable3=true&variable4=33.88&variable5=",
        "variable3=true|variable4=33.88|variable5=")]
    public void ParseQueryString_EdgeCasesShouldReturnListOfKeyValues(string? value, string expectedPipedString)
    {
        var result = value.ParseQueryString(autoConvertType: false);

        var expected = expectedPipedString.Split('|')
            .Select(s => s.Split('='))
            .ToDictionary(k => k.Length > 0
                ? k[0]
                : string.Empty, v => v.Length > 1
                ? v[1]
                : string.Empty);

        Assert.NotEmpty(result);
        Assert.Equivalent(expected, result);
    }
}

public enum FakeEnum
{
    Value1,
    Value2
}