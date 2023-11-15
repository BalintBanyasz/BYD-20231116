namespace SampleLibrary;

public interface IDependency
{
    Task<string> FunctionAAsync(string arg, CancellationToken cancellationToken = default);
}
