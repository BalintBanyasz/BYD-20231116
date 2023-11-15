namespace SampleLibrary;

public class Service
{
    private readonly IDependency _dependency;

    public Service(IDependency dependency)
    {
        _dependency = dependency;
    }

    public async Task FunctionAAsync(params string[] args)
    {
        foreach (var arg in args)
        {
            const string MagicString = "SPECIAL_VALUE";
            if (arg != MagicString)
            {
                await _dependency.FunctionAAsync(arg);
            }
        }
    }
}
