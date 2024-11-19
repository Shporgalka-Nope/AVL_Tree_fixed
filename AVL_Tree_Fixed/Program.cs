namespace AVL_Tree_Fixed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);

            Tree newTree = new Tree();
            newTree.Insert(4);
            newTree.Insert(5);
            newTree.Insert(6);

            Tree plusTree = tree + newTree;
            plusTree.Print();
        }
    }
}
