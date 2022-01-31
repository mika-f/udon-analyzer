namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;

public interface ISubCommand<T> : ISubCommand where T : class, new() {
}