## How to: Add a new Analyzer

Udon Analyzer supports to generate new analyzers from `codegen.exe` CLI.

Example:

```bash
# for Runtime
$ codegen.exe analyzer runtime
  --src $REPOSITORY_ROOT/src \
  --id 1 \
  --title MyAnalyzer \
  --category Compiler \
  --severity Warning \
  --runtime-min-version 1.0.0 \
  --description "This is a sample analyzer."

# for Compiler
$ codegen.exe analyzer compiler
  --src $REPOSITORY_ROOT/src \
  --id 1 \
  --title MyAnalyzer \
  --category Compiler \
  --severity Warning \
  --runtime-min-version 1.0.0 \
  --compiler-min-version 1.0.0 \
  --description "This is a sample analyzer."


# ...or JSON
$ codegen.exe analyzer compiler --src $REPOSITORY_ROOT/src --json analyzer.json

# for CodeFix
$ codegen.exe codefix
  --src $REPOSITORY_ROOT/src \
  --id 1 \
  --title MyCodeFix \
  --runtime-min-version 1.0.0 \
  --compiler-min-version 1.0.0
```
