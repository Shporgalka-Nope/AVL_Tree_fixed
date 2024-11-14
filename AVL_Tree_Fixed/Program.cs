namespace AVL_Tree_Fixed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            tree.Insert(6);
            tree.Insert(4);
            tree.Insert(8);
            tree.Insert(3);
            tree.Insert(7);
            tree.Insert(5);
            tree.Insert(9);

            Tree newTree = new Tree();
            newTree.Insert(10);
            newTree.Insert(6);
            newTree.Insert(11);
            newTree.Insert(5);
            newTree.Insert(7);

            Tree plusTree = tree + newTree;
            plusTree.Print();
        }
    }
}
