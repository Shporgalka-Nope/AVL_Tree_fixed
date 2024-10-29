namespace AVL_Tree_Fixed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(25);

            tree.Print();
        }
    }
}
