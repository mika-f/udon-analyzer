## Targeting

UdonAnalyzer will automatically detect the current version from `version.txt` present in the VRChat SDK and UdonSharp.  
Therefore, if the `version.txt` has not been updated, you may not be able to use the features of the new version.

### Default Targeting

If no special settings are made, the version will be detected automatically as described above.

### Explicit Targeting

If you want to fix the version intentionally for some reason, you can do so by specifying it in the editorconfig.  
Example:

```editorconfig
[*.cs]

udon_analyzer.vrchat_sdk = 2021.11.24.16.19
udon_analyzer.udon_sharp = 0.20.0
```
