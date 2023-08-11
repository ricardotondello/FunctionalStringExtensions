# StringExtensions

`StringExtensions` is a C# class library that provides a set of extension methods for working with strings. 
These extensions are designed to make common string manipulation tasks more convenient and expressive. 
Whether you're dealing with default values, asynchronous operations, or executing actions based on string conditions,
these extensions aim to simplify your code and enhance its readability.

## Installation

To easily integrate the StringExtensions library into your project, you can use NuGet Package Manager. 
NuGet is a package manager for .NET that simplifies the process of adding, removing, 
and updating libraries in your applications.

After that import the `StringExtensions` namespace in your code files where you want to use the provided extension methods:

   ```csharp
   using StringExtensions;
   ```

## Available Extension Methods

### `string? OrDefault(string? value, string @default)`

This extension method returns the provided default value if the input string is null or empty.

**Parameters:**
- `value` (string?): The input string.
- `@default` (string): The default value to return if the input string is null or empty.

**Usage:**
```csharp
string result = input.OrDefault("default value");
```

### `async Task<string> OrDefaultAsync(string? value, Task<string> @default)`

Similar to the `OrDefault` method, this asynchronous extension returns a default value obtained from a `Task<string>` if the input string is null or empty.

**Parameters:**
- `value` (string?): The input string.
- `@default` (Task<string>): A task that provides the default value to return if the input string is null or empty.

**Usage:**
```csharp
string result = await input.OrDefaultAsync(Task.FromResult("default value"));
```

### `string? WhenNullOrEmpty(string? value, Func<string> fnDefault)`

This extension method returns the result of the provided delegate function if the input string is null or empty.

**Parameters:**
- `value` (string?): The input string.
- `fnDefault` (Func<string>): A delegate function that returns the default value if the input string is null or empty.

**Usage:**
```csharp
string result = input.WhenNullOrEmpty(() => "default value");
```

### `async Task<string> WhenNullOrEmptyAsync(string? value, Func<Task<string>> fnDefault)`

Similar to the `WhenNullOrEmpty` method, this asynchronous extension returns a value obtained from a `Task<string>` returned by the delegate function if the input string is null or empty.

**Parameters:**
- `value` (string?): The input string.
- `fnDefault` (Func<Task<string>>): An asynchronous delegate function that returns a task providing the default value if the input string is null or empty.

**Usage:**
```csharp
string result = await input.WhenNullOrEmptyAsync(async () => await GetDefaultValueAsync());
```

### `void OnNullOrEmpty(string? value, Action action)`

This extension method executes the provided action if the input string is null or empty.

**Parameters:**
- `value` (string?): The input string.
- `action` (Action): The action to execute if the input string is null or empty.

**Usage:**
```csharp
input.OnNullOrEmpty(() => Console.WriteLine("Input is null or empty."));
```

### `async Task OnNullOrEmptyAsync(string? value, Task action)`

Similar to the `OnNullOrEmpty` method, this asynchronous extension executes a provided task if the input string is null or empty.

**Parameters:**
- `value` (string?): The input string.
- `action` (Task): The task to execute if the input string is null or empty.

**Usage:**
```csharp
await input.OnNullOrEmptyAsync(async () => await PerformAsyncAction());
```

## Contributing

Contributions are welcome! If you find a bug or have a feature request, please open an issue on GitHub.
If you would like to contribute code, please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.
See [LICENSE](https://github.com/ricardotondello/StringExtensions/blob/main/LICENSE) for more information.

## Support

<a href="https://www.buymeacoffee.com/ricardotondello" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>