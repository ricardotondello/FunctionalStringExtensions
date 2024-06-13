using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FunctionalStringExtensions;

public static class FunctionalStringExtensions
{
    private static readonly object Lock = new();
    private static readonly TimeSpan RegexTimeOut = TimeSpan.FromMilliseconds(100);
    private static readonly IDictionary<string, object> EmptyQueryDictionary = new Dictionary<string, object>(0);

    /// <summary>
    /// This extension method returns the provided default value if the input string is null or empty.
    /// </summary>
    /// <param name="value">Your string</param>
    /// <param name="default">Define the default string in case of null or empty</param>
    /// <returns>Either your string or chosen default value</returns>
    public static string OrDefault(this string? value, string @default) =>
        string.IsNullOrEmpty(value) ? @default : value;

    /// <summary>
    /// Similar to the `OrDefault` method, this asynchronous extension returns a default value obtained from a `Task string` if the input string is null or empty.
    /// </summary>
    /// <param name="value">Your string</param>
    /// <param name="default">Define the default string in case of null or empty</param>
    /// <returns>Either your string or chosen default value</returns>
    public static async Task<string> OrDefaultAsync(this string? value, Task<string> @default) =>
        string.IsNullOrEmpty(value) ? await @default : value;

    /// <summary>
    /// This extension method returns the result of the provided delegate function if the input string is null or empty.
    /// </summary>
    /// <param name="value">Your string</param>
    /// <param name="fnDefault">Func to be executed when value is Null or Empty</param>
    /// <returns>Either your string or the result of the Func</returns>
    public static string WhenNullOrEmpty(this string? value, Func<string> fnDefault) =>
        string.IsNullOrEmpty(value) ? fnDefault() : value;

    /// <summary>
    /// Similar to the `WhenNullOrEmpty` method, this asynchronous extension returns a value obtained from a `Task string` returned by the delegate function if the input string is null or empty.
    /// </summary>
    /// <param name="value">Your string</param>
    /// <param name="fnDefault">Func to be executed when value is Null or Empty</param>
    /// <returns>Either your string or the result of the async Func</returns>
    public static async Task<string> WhenNullOrEmptyAsync(this string? value, Func<Task<string>> fnDefault) =>
        string.IsNullOrEmpty(value) ? await fnDefault() : value;

    /// <summary>
    /// This extension method executes the provided action if the input string is null or empty.
    /// </summary>
    /// <param name="value">Your string</param>
    /// <param name="action">Action to be executed when value is Null or Empty</param>
    public static void OnNullOrEmpty(this string? value, Action action)
    {
        if (string.IsNullOrEmpty(value))
        {
            action();
        }
    }

    /// <summary>
    /// Similar to the `OnNullOrEmpty` method, this asynchronous extension executes a provided task if the input string is null or empty.
    /// </summary>
    /// <param name="value">Your string</param>
    /// <param name="action">Task to be executed when value is Null or Empty</param>
    public static async Task OnNullOrEmptyAsync(this string? value, Task action)
    {
        if (string.IsNullOrEmpty(value))
        {
            await action;
        }
    }

    /// <summary>
    /// Turns your string in a slug
    /// </summary>
    /// <param name="value">Your string</param>
    /// <exception cref="RegexMatchTimeoutException"></exception>
    /// <returns>The slugified of your provided string</returns>
    public static string ToSlug(this string? value)
    {
        lock (Lock)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var bytes = Encoding.GetEncoding("UTF-8").GetBytes(value);

            var firstReplace = Regex.Replace(Encoding.ASCII.GetString(bytes), @"\s{2,}|[^\w]", " ",
                RegexOptions.ECMAScript, RegexTimeOut).Trim();

            return Regex.Replace(firstReplace, @"\s+", "_", RegexOptions.None, RegexTimeOut).ToLowerInvariant();
        }
    }

    /// <summary>
    /// Parse your string to a Enum value
    /// </summary>
    /// <param name="value">String to be parsed</param>
    /// <param name="default">Chosen default value when string doesnt match</param>
    /// <typeparam name="T">Type of your Enum</typeparam>
    /// <returns>The Enum value parsed or default when not matched</returns>
    public static T ToEnum<T>(this string? value, T @default = default) where T : struct =>
        Enum.TryParse<T>(value, out var result) ? result : @default;

    /// <summary>
    /// Search for letters (A to Z) in the string
    /// </summary>
    /// <param name="value">Input <see cref="string"/> value</param>
    /// <returns>The letters present in the string provided</returns>
    public static string OnlyLetters(this string? value)
    {
        return value.FilterCharacters(char.IsLetter);
    }

    /// <summary>
    /// Search for numbers (0 to 9) in the string
    /// </summary>
    /// <param name="value">Input <see cref="string"/> value</param>
    /// <returns>The numbers present in the string provided</returns>
    public static string OnlyNumbers(this string? value)
    {
        return value.FilterCharacters(char.IsDigit);
    }

    /// <summary>
    /// Search for characters and numbers (A to Z or 0 to 9) in the string
    /// </summary>
    /// <param name="value">Input <see cref="string"/> value</param>
    /// <returns>The characters and numbers present in the string provided</returns>
    public static string OnlyCharactersAndNumbers(this string? value)
    {
        return value.FilterCharacters(char.IsLetterOrDigit);
    }
    
    /// <summary>
    /// Search for especial characters (not A to Z and not 0 to 9) in the string
    /// </summary>
    /// <param name="value">Input <see cref="string"/> value</param>
    /// <returns>The especial characters present in the string provided</returns>
    public static string OnlySpecialCharacters(this string? value)
    {
        return value.FilterCharacters(c => !char.IsLetterOrDigit(c));
    }

    /// <summary>
    /// Parses a query string into a dictionary of key-value pairs.
    /// </summary>
    /// <param name="value">The input query string.</param>
    /// <param name="autoConvertType">Indicates whether to automatically convert the query values to their appropriate types (e.g., int, bool, double). Default is false.</param>
    /// <returns>
    /// A dictionary containing the parsed query parameters as key-value pairs. The keys are strings, and the values are objects.
    /// If the input string is null or empty, an empty dictionary is returned.
    /// </returns>
    public static IDictionary<string, object> ParseQueryString(this string? value, bool autoConvertType = false)
    {
        var index = value?.IndexOf('?') ?? -1; 
        if (string.IsNullOrEmpty(value) || index < 0)
        {
            return EmptyQueryDictionary;
        }

        var query = value[0] == '?' 
            ? value.TrimStart('?') 
            : value.Substring(index + 1, value.Length - index - 1);

        if (string.IsNullOrEmpty(query))
        {
            return EmptyQueryDictionary;
        }
        
        return query
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split('='))
            .ToDictionary(
                part => UrlDecode(part[0]), 
                part =>
                {
                    var partValue = MaybeGetValue(part);
                    var decodedValue = UrlDecode(partValue);
                    return autoConvertType ? ConvertToType(decodedValue) : decodedValue;
                });

        string MaybeGetValue(string[] partValue)
        {
            return partValue.Length < 2 ? string.Empty : partValue[1];
        }
        
        object ConvertToType(string valueToConvert)
        {
            if (int.TryParse(valueToConvert, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
            {
                return intValue;
            }

            if (bool.TryParse(valueToConvert, out var boolValue))
            {
                return boolValue;
            }

            if (double.TryParse(valueToConvert, NumberStyles.Float | NumberStyles.AllowThousands, 
                    CultureInfo.InvariantCulture, out var doubleValue))
            {
                return doubleValue;
            }
        
            return valueToConvert;
        }

        string UrlDecode(string? input)
        {
            return string.IsNullOrEmpty(input) 
                ? string.Empty 
                : HttpUtility.UrlDecode(input);
        }
    }
    
    private static string FilterCharacters(this string? value, Func<char, bool> predicate)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var chars = value.ToCharArray();
        var sb = new StringBuilder(chars.Length);

        foreach (var c in chars.Where(predicate))
        {
            sb.Append(c);
        }

        return sb.ToString();
    }
    
}