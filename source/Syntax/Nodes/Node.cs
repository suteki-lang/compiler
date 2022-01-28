namespace Suteki
{
    class Node
    {
        // Emit C++ code
        public virtual void Emit(Input input)
        {

        }

        public virtual string   GetString => "";
        public virtual Token    GetToken  => null;
    }
}