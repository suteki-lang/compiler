namespace Suteki
{
    class NodePrimitive : Node
    {
        public Token         Token;
        public bool          IsConst;
        public PrimitiveKind PrimitiveKind;

        public override Token GetToken => Token;

        public override string GetString
        {
            get
            {
                string[] cTypes =
                {
                    "",
                    "void ", 
                    "bool ", 
                    "string ",

                    "unsigned char ",
                    "unsigned short ",
                    "unsigned int ", 
                    "unsigned long ",
                    "unsigned long ",

                    "char ",  
                    "short ", 
                    "int ",   
                    "long ",  
                    "long ",  

                    "float ",
                    "double ",
                };
                
                if (IsConst)
                    return $"const {cTypes[(int)PrimitiveKind]}";

                return cTypes[(int)PrimitiveKind];
            }
        }

        // Type checking
        public override Type TypeCheck(Input input)
        {
            if (IsConst)
            {
                return new TypeConst()
                {
                    Type = new TypePrimitive()
                    {
                        Kind = PrimitiveKind
                    }
                };
            }
            else
            {
                return new TypePrimitive() 
                { 
                    Kind = PrimitiveKind
                };
            }
        }
    }
}