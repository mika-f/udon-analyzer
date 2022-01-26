# How to: Add a new Tests

Udon Analyzer supports to generate new analyzers from `testgen.exe` CLI.

Example:

```bash
# for CodeFix
$ testgen.exe \
  --src $REPOSITORY_ROOT/src \
  --id VRC0001 \
  --with-code-fix
```
