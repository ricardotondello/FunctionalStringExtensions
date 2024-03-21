# 🧶 FunctionalStringExtensions

`FunctionalStringExtensions` is a C# class library that provides a set of extension methods for working with strings. 
These extensions are designed to make common string manipulation tasks more convenient and expressive. 
Whether you're dealing with default values, asynchronous operations, or executing actions based on string conditions,
these extensions aim to simplify your code and enhance its readability.

## Installation 🚀

To easily integrate the FunctionalStringExtensions library into your project, you can use NuGet Package Manager. 
NuGet is a package manager for .NET that simplifies the process of adding, removing, 
and updating libraries in your applications.

After that import the `FunctionalStringExtensions` namespace in your code files where you want to use the provided extension methods:

```csharp
using FunctionalStringExtensions;
```

## Available Extension Methods 🛠️

### `OrDefault`

This extension method returns the provided default value if the input string is null or empty.

**Usage:**
```csharp
string result = input.OrDefault("default value");
```

### `OrDefaultAsync`

Similar to the `OrDefault` method, this asynchronous extension returns a default value obtained from a `Task<string>` if the input string is null or empty.

**Usage:**
```csharp
string result = await input.OrDefaultAsync(Task.FromResult("default value"));
```

### `WhenNullOrEmpty`

This extension method returns the result of the provided delegate function if the input string is null or empty.

**Usage:**
```csharp
string result = input.WhenNullOrEmpty(() => "default value");
```

### `WhenNullOrEmptyAsync`

Similar to the `WhenNullOrEmpty` method, this asynchronous extension returns a value obtained from a `Task<string>` returned by the delegate function if the input string is null or empty.

**Usage:**
```csharp
string result = await input.WhenNullOrEmptyAsync(async () => await GetDefaultValueAsync());
```

### `OnNullOrEmpty`

This extension method executes the provided action if the input string is null or empty.

**Usage:**
```csharp
input.OnNullOrEmpty(() => Console.WriteLine("Input is null or empty."));
```

### `OnNullOrEmptyAsync`

Similar to the `OnNullOrEmpty` method, this asynchronous extension executes a provided task if the input string is null or empty.

**Usage:**
```csharp
await input.OnNullOrEmptyAsync(async () => await PerformAsyncAction());
```

### `ToSlug`

Turns your string in a slug

**Usage**
```csharp
var slug = "ICH MUß EINIGE CRÈME BRÛLÉE HABEN".ToSlug();
```

### `ToEnum`

Parse your string to a Enum value

**Usage**
```csharp

public enum FakeEnum
{
    Value1,
    Value2
}

var result = "Value1".ToEnum<FakeEnum>(); // FakeEnum.Value1
```

### `OnlyLetters`

Search for letters (A to Z) in the string

**Usage**
```csharp
var result = "abc123def456ghi".OnlyLetters(); //"abcdefghi"
```

### `OnlyNumbers`

Search for numbers (0 to 9) in the string

**Usage**
```csharp
var result = "abc123def456ghi".OnlyNumbers(); //"123456"
```

### `OnlyCharactersAndNumbers`

Search for characters and numbers (A to Z or 0 to 9) in the string

**Usage**
```csharp
var result = "12.8/0';@#!%^&*()a12,9abc".OnlyCharactersAndNumbers(); //"1280a129abc"
```

### `OnlySpecialCharacters`

Search for especial characters (not A to Z and not 0 to 9) in the string

**Usage**
```csharp
var result = "12.8/0';@#!%^&*()a12,9abc".OnlySpecialCharacters(); //"./';@#!%^&*(),"
```