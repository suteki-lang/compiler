namespace Suteki.Compiler;

public class Output
{
    public string ExternalFunctionDeclarations;
    public string FunctionDefinitions;

    public int Tabs;

    // Default Constructor
    public Output()
    {
        ExternalFunctionDeclarations = "";
        FunctionDefinitions          = "";
        Tabs                         = 0;
    }

    // Get tabs
    public string GetTabs()
    {
        string result = "";

        for (int i = 0; i < Tabs; ++i)
            result += '\t';

        return result;
    }
}