namespace Suteki
{
    class Output
    {
        public string Header   = "";
        public string Source   = "";
        public string Includes = "";
        public int    Tabs     =  0;

        // Begin scope
        public void BeginScope()
        {
            ++Tabs;
        }

        // End scope
        public void EndScope()
        {
            --Tabs;
        }

        // Write tabs
        public void WriteTabs()
        {
            for (int i = 0; i < (Tabs); ++i)
                Source += '\t';
        }
    }
}