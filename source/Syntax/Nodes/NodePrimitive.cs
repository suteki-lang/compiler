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

        // Type checking
        public override ExpressionKind TypeCheck(Input input)
        {
            switch (PrimitiveKind)
            {
                case PrimitiveKind.Void:
                    return ExpressionKind.Void;

                case PrimitiveKind.Bool:
                    return ExpressionKind.Bool;

                case PrimitiveKind.String:
                    return ExpressionKind.String;

                case PrimitiveKind.Single:
                case PrimitiveKind.Double:
                    return ExpressionKind.Float;

                default:
                    return ExpressionKind.Integer;
            }
        }
    }
}