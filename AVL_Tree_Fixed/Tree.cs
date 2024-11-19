using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
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
            Tree leftTree;
            Tree rightTree;
            if (treeOne.head.height <= treeTwo.head.height)
            {
                leftTree = treeOne;
                rightTree = treeTwo;
            } 
            else
            {
                leftTree = treeTwo;
                rightTree = treeOne;
            }

            if (leftTree.head == null) { return rightTree; }
            if (rightTree.head == null) { return leftTree; }

            Node leftDock = leftTree.head;
            Node parent = leftDock;
            while (leftDock.right != null)
            {
                parent = leftDock;
                leftDock = leftDock.right;
            }
            if (parent != leftDock) { leftTree.Delete(leftDock.data); }

            Node rightDock = rightTree.head.left;
            parent = rightDock;
            while (rightDock.height != leftTree.head.height)
            {
                parent = rightDock;
                if (rightDock.left.height == rightDock.height - 1) { rightDock = rightDock.left; }
                else { rightDock = rightDock.right; }
            }
            //if (parent.left == rightDock) 
            //{ 
            //    Tree newTree = new Tree();
            //    newTree.head = leftDock;
            //    newTree.head.left = treeOne.head;
            //    newTree.head.right = rightDock;
            //    parent.left = newTree.head;
            //}
            //else 
            //{
            //    Tree newTree = new Tree();
            //    newTree.head = leftDock;
            //    newTree.head.left = treeOne.head;
            //    newTree.head.right = rightDock;
            //    parent.right = newTree.head;
            //}

            return rightTree;
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
