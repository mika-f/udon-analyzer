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

# ...or JSON (recommended)
$ codegen.exe analyzer compiler --json analyzer.json


# for CodeFix
$ codegen.exe codefix
  --src $REPOSITORY_ROOT/src \
  --id 1 \
  --title MyCodeFix \
  --runtime-min-version 1.0.0 \
  --compiler-min-version 1.0.0
```

If you want to generate analyzer from JSON, you can use `--json` option with the following JSON format:

```json
{
  "source": "../src",
  "id": 37,
  "name": "DoesNotCurrentlySupportUsingTypeofOnUserDefinedTypesAnalyzer",
  "title": "UdonSharp does not currently support using 'typeof' on user defined types",
  "category": "Compiler",
  "severity": "Error",
  "description": "UdonSharp does not currently support using 'typeof' on user defined types",
  "messageFormat": null
}
```
