# Planned Analyzers

List of planned analyzers in the future release.

## Udon Runtime

- `ERR: UdonSharp does not currently support type checking with the \"is\" keyword since Udon does not yet expose the proper functionality for type checking.`
- `` ERR: The `is` keyword is not yet supported by UdonSharp since Udon does not expose what is necessary ``
- `` ERR: The `as` keyword is not yet supported by UdonSharp since Udon does not expose what is necessary ``
- `ERR: Method {signature} is not exposed in Udon`
- `ERR: Field accessor {signature} is not exposed in Udon`
- `ERR: Udon does not support return values of type {type} yet`
- `ERR: Udon does not support method parameters of type {type} yet`
- `ERR: Udon does not support variable of type {type} yet`
- `ERR: Cannot sync variable because behaviour is set to NoVariableSync, change the behaviour sync mode to sync variables`
- `ERR: Udon does not currently support syncing of the type {type}`
- `ERR: Udon does not support linear interpolation of the synced type {type}`
- `ERR: Udon does not support smooth interpolation of the synced type {type}`
- `ERR: Udon does not support variable tweening when the behaviour is in Manual sync mode`
- `ERR: Syncing of array type {type} is only supported in manual sync mode`
- `WRN: The method called by SendCustomEvent must be public`
- `WRN: The method specified for SendCustomEvent must be public`
- `WRN: The method called over the network cannot start with an underscore`
- `WRN: The method is not called over the network should be started with an underscore`
- `WRN: The specified event is not declared in the behaviour`

## UdonSharp Compiler

- `ERR: Invalid target property for {variable}`
- `ERR: Types must match between property and variable change field`
- `ERR: Cannot define method with the same name as built-in UdonSharpBehaviour methods`
- `ERR: UdonSharp only supports 1 type generic methods at the moment`
- `ERR: Nullable types are not currently supported by UdonSharp`
- `ERR: UdonSharp does not yet support user defined enums`
- `` ERR: UdonSharp does not currently support using `typeof` on user defined types ``
- `ERR: UdonSharp does not yet support static using directives`
- `ERR: UdonSharp does not yet support namespace alias directives`
- `` WRN: Use the `nameof` operator instead of directly specifying the method name in SendCustomEvent ``
- `WRN: Use the namespace to avoid class name conflicts`
