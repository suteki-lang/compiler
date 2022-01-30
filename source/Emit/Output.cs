namespace Suteki
{
    class Output
    {
        public string ExternalFunctionDeclarations;
        public string FunctionDeclarations;
        public string FunctionDefinitions;

        public string Includes;

        public int Tabs;

        // Default Constructor
        public Output()
        {
            ExternalFunctionDeclarations = "";
            FunctionDeclarations         = "";
            FunctionDefinitions          = "";

            Includes = "";

            Tabs = 0;
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
}