using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Schema;

namespace AVL_Tree_Fixed
{
    internal class Tree
    {
        public Node head;

        public Tree()
        {

        }

        public void Insert(int data)
        {
            Insert(data, head);
        }
        private Node Insert(int data, Node? current)
        {
            if (head == null)
            {
                head = new Node(data);
                return head;
            }
            if (current == null) { return new Node(data); }

            if (data == current.data) {  return current; }
            if (data <= current.data) { current.left = Insert(data, current.left); }
            else { current.right = Insert(data, current.right); }

            current.height = 1 + Math.Max(GetHeight(current.left), GetHeight(current.right));
            
            int balance = GetBalance(current);
            if (balance > 1) {
                if (GetBalance(current.left) < 0)
                {
                    current.left = LeftRotation(current.left);
                }
                return RightRotation(current);
            }
            else if (balance < -1) 
            { 
                if (GetBalance(current.right) > 0)
                {
                    current.right = RightRotation(current.right);
                }
                return LeftRotation(current); }

            return current;
        }
        private static int GetHeight(Node node)
        {
            if (node == null) { return 0; }
            return node.height;
        }
        private static int GetBalance(Node node)
        {
            return GetHeight(node.left) - GetHeight(node.right);
        }

        public void Delete(int data)
        {
            Delete(data, head);
        }
        private Node Delete(int data, Node current)
        {
            if (data == current.data) 
            {
                if (current.left == null || current.right == null)
                {
                    Node temp;
                    if (current.left != null) { temp = current.left; }
                    else { temp = current.right; }

                    return temp;
                }
                else
                {
                    int tempData = FindMin(current.right).data;
                    //if (FindMin(current).data == current.left.data) { current.left = null; }
                    //if (FindMin(current).data == current.right.data) { current.right = null; }
                    current.data = tempData;
                    current.right = Delete(tempData, current.right);
                    return current;
                }

            }
            else
            {
                if (data < current.data) { current.left = Delete(data, current.left); }
                else { current.right = Delete(data, current.right); }
            }

            current.height = 1 + Math.Max(GetHeight(current.left), GetHeight(current.right));
            int balance = GetBalance(current);
            if (balance > 1) { return RightRotation(current); }
            else if (balance < -1) { return LeftRotation(current); }
            return current;
        }
        private static Node FindMin(Node root)
        {
            if (root.left == null) { return root; }
            else { return FindMin(root.left); }
        }

        private static Node DeleteMin(Node root)
        {
            if (root.left == null) { return root.right; }
            else { return DeleteMin(root.left); }
        }
        public static Node DeleteMax(Node root)
        {
            Node selected;
            if (root.right != null) { selected = DeleteMax(root.right); }
            else { return root; }
            root.right = null;
            return selected;
        }

        public void Print()
        {
            Console.WriteLine(Print(head));
        }
        private string Print(Node root)
        {
            string output = "";
            if (root.left != null) { output += Print(root.left); }
            if (root.right != null) { output += Print(root.right); }
            output += root.data;
            return output;
        }
        public static Stack<Node> GetAllNodes(Tree tree)
        {
            Stack<Node> nodes = new Stack<Node>();
            GetAllNodes(tree.head, nodes);
            return nodes;
        }
        private static Node GetAllNodes(Node current, Stack<Node> nodes)
        {
            if (current.left != null) { GetAllNodes(current.left, nodes); }
            if (current.right != null) { GetAllNodes(current.right, nodes); }
            nodes.Push(current);
            return current;
        } 

        private Node LeftRotation(Node root)
        {
            Node? rRight = root.right;
            root.right = rRight.left;
            rRight.left = root;
            if(root == head) { head = rRight; }

            root.height = 1 + Math.Max(GetHeight(root.left), GetHeight(root.right));
            rRight.height = 1 + Math.Max(GetHeight(rRight.left), GetHeight(rRight.right));

            return rRight;
        }
        private Node RightRotation(Node root)
        {
            Node? rLeft = root.left;
            root.left = rLeft.right;
            rLeft.right = root;
            if(head == root) {  head = rLeft; }

            root.height = 1 + Math.Max(GetHeight(root.left), GetHeight(root.right));
            rLeft.height = 1 + Math.Max(GetHeight(rLeft.left), GetHeight(rLeft.right));
            return rLeft;
        }

        public static Tree operator +(Tree original, int value)
        {
            Tree newTree = new Tree();
            Stack<Node> nodes = GetAllNodes(original);
            foreach(Node node in nodes)
            {
                newTree.Insert(node.data + value);
            }
            return newTree;
        }
        public static Tree operator +(Tree treeOne, Tree treeTwo)
        {
            Node min = FindMin(treeTwo.head);
            Node max = treeOne.head;
            while (max.right != null)
            {
                max = max.right;
            }
            if (max.data < min.data) { return TreePlus_Two(treeOne, treeTwo); }

            min = FindMin(treeOne.head);
            max = treeTwo.head;
            if (max.data < min.data) { return TreePlus_Two(treeTwo, treeOne); }
            else { return TreePlus_One(treeOne, treeTwo); }
        }

        private static Tree TreePlus_One(Tree treeOne, Tree treeTwo)
        {
            Tree newTree = new Tree();
            Stack<Node> nodes = StackPlus(GetAllNodes(treeOne), GetAllNodes(treeTwo));
            foreach (Node node in nodes)
            {
                newTree.Insert(node.data);
            }
            return newTree;
        }
        private static Tree TreePlus_Two(Tree treeOne, Tree treeTwo) 
        {
            Tree leftTree = treeOne;
            Tree rightTree = treeTwo;

            if (leftTree.head == null) { return rightTree; }
            if (rightTree.head == null) { return leftTree; }

            //Left tree max edge search
            Node leftDock = leftTree.head;
            while (leftDock.right != null)
            {
                leftDock = leftDock.right;
            }
            if (leftTree.head != leftDock) { leftTree.Delete(leftDock.data); }

            //Right tree same-height search on the left side
            int targetHeight = leftTree.head.height;
            
            return FindPlace(leftTree, rightTree, leftDock, targetHeight);
        }

        public static Tree FindPlace(Tree leftTree, Tree rightTree, Node x, int height)
        {
            rightTree.head = _FindPlace(leftTree, x, rightTree.head, height);
            return rightTree;
        }

        private static Node _FindPlace(Tree leftTree, Node x, Node root, int height)
        {
            if (root.height != height) { root.left = _FindPlace(leftTree, x, root.left, height); }
            else
            {
                x.left = leftTree.head;
                x.right = root;
                root = x;
            }
            return root;
        }

        private static Tree MakeNewTree(Node x, Tree t1, Node p)
        {
            Tree newTree = new Tree();
            newTree.head = x;
            newTree.head.left = t1.head;
            newTree.head.right = p;
            return newTree;
        }

        //Unholy creation
        public static Stack<Node> StackPlus(Stack<Node> firstStack, Stack<Node> secondStack) 
        { 
            foreach(Node node in firstStack)
            {
                secondStack.Push(node);
            }
            return secondStack;
        }
    }

    internal class Node
    {
        public int data;
        public Node? left;
        public Node? right;
        public int height = 1;

        public Node(int d)
        {
            data = d;
        }
    }
}
