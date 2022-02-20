namespace Suteki;

class NodePrimitive : Node
{
    public Token         Token;
    public PrimitiveKind PrimitiveKind;

    public override Token    GetToken => Token;
    public override NodeKind Kind     => NodeKind.Primitive;

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

            return cTypes[(int)PrimitiveKind];
        }
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        return new TypePrimitive() 
        { 
            Kind = PrimitiveKind
        };
    }
}