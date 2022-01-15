# How to: Test Analyzer with xUnit

If you want to test this analyzer with xUnit, please follow the steps below:

## Requirements

1. Install [Unity Hub](https://unity3d.com/get-unity/download)
2. Install Unity Editor withing Unity Hub
   - To find out which version of Unity Editor should install, please refer to [the VRChat documentation](https://docs.vrchat.com/docs/current-unity-version)
3. Create a new project in Unity
4. Add the VRChat SDK (World) to the project
5. Add the [UdonSharp](https://github.com/MerlinVR/UdonSharp) to the project

## Testing

1. Configure environment variables
2. Run the test (`dotnet test`)

## Environment Variables

You should set the following environment variables:

| Environment Variable Key       | Example Value                                                   | Description                   |
| ------------------------------ | --------------------------------------------------------------- | ----------------------------- |
| `UDON_ANALYZER_TARGET_PROJECT` | `"C:\repos\github.com\natsuneko-laboratory\udon-analyzer-test"` | The path to the Unity project |
