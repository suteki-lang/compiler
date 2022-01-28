using System;

namespace Suteki
{
    class Program
    {
        static void Main(string[] arguments)
        {
            Input input = new Input();

            var block = new NodeBlock();
            block.Statements.Add(new NodeReturn()
            {
                Expression = new NodeInteger() 
                { 
                    Value = new Token() 
                    { 
                        Content = "0"
                    }
                }
            });

            var function = new NodeFunction()
            {
                Property = PropertyKind.Extern,
                Type = new NodePrimitive() { PrimitiveKind = PrimitiveKind.Int },
                Name = new Token() { Content = "main" },
                Block = block
            };

            function.Parameters.Add(new NodeParameter()
            {
                Type = new NodePrimitive() { PrimitiveKind = PrimitiveKind.Int },
                Name = new Token() { Content = "argc" }
            });

            input.Nodes.Add(function);
            input.Path = "test/testing.su";

            Emitter.Start(input);
            Config.Inputs.Add(input);
            Linker.Start();
        }
    }
}