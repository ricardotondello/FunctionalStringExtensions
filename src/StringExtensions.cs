using System;
using System.Threading.Tasks;

namespace StringExtensions;

public static class StringExtensions
{
    public static string OrDefault(this string? value, string @default) =>
        string.IsNullOrEmpty(value) ? @default : value;
    
    public static async Task<string> OrDefaultAsync(this string? value, Task<string> @default) =>
        string.IsNullOrEmpty(value) ? await @default : value;

    public static string WhenNullOrEmpty(this string? value, Func<string> fnDefault) =>
        string.IsNullOrEmpty(value) ? fnDefault() : value;
    
    public static async Task<string> WhenNullOrEmptyAsync(this string? value, Func<Task<string>> fnDefault) =>
        string.IsNullOrEmpty(value) ? await fnDefault() : value;

    public static void OnNullOrEmpty(this string? value, Action action)
    {
        if (string.IsNullOrEmpty(value))
        {
            action();
        }
    }
    
    public static async Task OnNullOrEmptyAsync(this string? value, Task action)
    {
        if (string.IsNullOrEmpty(value))
        {
            await action;
        }
    }
}