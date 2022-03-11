# Dictionary for Udon Analyzer

## Text Dictionary

Udon Analyzer refers to the following files in the project by API dictionaries, if they exists:

- `PublicAPI.Shipped.txt`
- `PublicAPI.Shipped.*.txt`

### Format

`PublicAPI.Shipped.txt` will be in the following format:

```
T:N.AllowedType
```

This is the same format as BannedApiAnalyzers and can be used outside of VRChat Udon as long as the results are the same as in the table below.

| Symbol in Source                   | Entry in PublicAPI.Shipped.txt                     |
| ---------------------------------- | -------------------------------------------------- |
| `class AllowedType`                | `T:N.AllowedType`                                  |
| `class AllowedType<T>`             | `` T:N.AllowedType`1 ``                            |
| `AllowedType()`                    | `M:N.AllowedType#ctor`                             |
| `int AllowedMethod()`              | `M:N.AllowedType.AllowedMethod`                    |
| `void AllowedMethod(int i)`        | `M:N.AllowedType.AllowedMethod(System.Int32)`      |
| `void AllowedMethod<T>(T t)`       | ``` M:N.AllowedType.AllowedMethod`1(``0) ```       |
| `void AllowedMethod<T>(Func<T> f)` | ``` M:N.AllowedType.AllowedMethod`1(Func{``0}) ``` |
| `string AllowedField`              | `F:N.AllowedType.AllowedField`                     |
| `string AllowedProperty { get; } ` | `P:N.AllowedType.AllowedProperty`                  |
