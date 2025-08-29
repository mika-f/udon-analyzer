## List of Runtime Analyzers in UdonAnalyzers

| ID      | Title                                                                                                                             | Severity | 
| ------- | --------------------------------------------------------------------------------------------------------------------------------- | -------- | 
| VRC0001 | [Try\-Catch\-Finally is not supported](./VRC0001.md)                                                                              | Error    | 
| VRC0002 | [Does not support throwing exceptions](./VRC0002.md)                                                                              | Error    | 
| VRC0003 | [Udon does not support instantiating non\-GameObject types](./VRC0003.md)                                                         | Error    | 
| VRC0004 | [The specified event is deprecated use the version with VRCPlayerApi](./VRC0004.md)                                               | Error    | 
| VRC0005 | [Udon does not currently support type checking with the \`is\` keyword](./VRC0005.md)                                             | Error    | 
| VRC0006 | [The \`as\` keyword is not yet supported by Udon](./VRC0006.md)                                                                   | Error    | 
| VRC0007 | [Method is not exposed to Udon](./VRC0007.md)                                                                                     | Error    | 
| VRC0008 | [Field is not exposed to Udon](./VRC0008.md)                                                                                      | Error    | 
| VRC0009 | [Type is not exposed to Udon](./VRC0009.md)                                                                                       | Error    | 
| VRC0010 | [Cannot sync variable because behaviour is set to NoVariableSync, change the behaviour sync mode to sync variables](./VRC0010.md) | Warning  | 
| VRC0011 | [Udon does not currently support syncing of the type](./VRC0011.md)                                                               | Warning  | 
| VRC0012 | [Udon does not support linear interpolation of the synced type](./VRC0012.md)                                                     | Warning  | 
| VRC0013 | [Udon does not support smooth interpolation of the synced type](./VRC0013.md)                                                     | Warning  | 
| VRC0014 | [Udon does not support variable tweening when the behaviour is in Manual Sync Mode](./VRC0014.md)                                 | Warning  | 
| VRC0015 | [Syncing of array type is only supported in manual sync mode](./VRC0015.md)                                                       | Warning  | 
| VRC0016 | [The method specified for SendCustomEvent must be public](./VRC0016.md)                                                           | Warning  | 
| VRC0017 | [The method specified for over the network cannot start with an underscore](./VRC0017.md)                                         | Warning  | 
| VRC0018 | [The method specified for SendCustomEvent is not declared in the behaviour](./VRC0018.md)                                         | Warning  | 
| VRC0019 | [NetworkCallableAttribute must be required for calling method via SendCustomNetworkEvent with parameters](./VRC0019.md)           | Error    | 
| VRC0020 | [The parameter type of method does not match the Nth argument of SendCustomNetworkEvent](./VRC0020.md)                            | Error    | 
| VRC0021 | [Unable to send network event to UdonBehaviour with SyncType 'None'](./VRC0021.md)                                                | Warning  | 


