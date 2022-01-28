namespace Suteki
{
    class NodePrimitive : Node
    {
        public Token         Token;
        public PrimitiveKind PrimitiveKind;

        public override Token GetToken => Token;

        public override string GetString
        {
            get
            {
                string[] cTypes =
                {
                    "void ", 
                    "bool ", 
                    "const char *",

                    "unsigned char ",
                    "unsigned short ",
                    "unsigned int ", 
                    "unsigned long ",

                    "char ",  
                    "short ", 
                    "int ",   
                    "long ",  

                    "float ",
                    "double ",
                };
                
                return cTypes[(int)PrimitiveKind];
            }
        }
    }
}