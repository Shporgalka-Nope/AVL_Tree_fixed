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

            if (data <= current.data) { current.left = Insert(data, current.left); }
            else { current.right = Insert(data, current.right); }

            current.height = 1 + Math.Max(GetHeight(current.left), GetHeight(current.right));
            
            int balance = GetBalance(current);
            if (balance > 1) { return RightRotation(current); }
            else if (balance < -1) { return LeftRotation(current); }

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

                    if (temp == null) { return null; }
                    else { return temp; }
                }
                else
                {
                    int tempData = FindMin(current).data;
                    DeleteMin(current);
                    current.data = tempData;
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
            
            root.height = GetHeight(root);
            rLeft.height = GetHeight(rLeft);

            return rLeft;
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
