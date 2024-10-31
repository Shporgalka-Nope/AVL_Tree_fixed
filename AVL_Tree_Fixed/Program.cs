namespace AVL_Tree_Fixed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            tree.Insert(5);
            tree.Insert(4);
            tree.Insert(6);

            Tree newTree = new Tree();
            newTree.Insert(1);
            newTree.Insert(2);
            newTree.Insert(3);

            Tree plusTree = tree + newTree;

            tree.Print();
            plusTree.Print();
        }
    }
}
