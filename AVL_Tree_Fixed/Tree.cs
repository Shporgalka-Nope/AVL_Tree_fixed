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
            Stack<Node> allAffected = new Stack<Node>();
            FixBalance(Insert(data, head, allAffected));
        }
        private Stack<Node> Insert(int data, Node? current, Stack<Node> allAffected)
        {
            Node newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
                allAffected.Push(newNode);
                return allAffected;
            }

            allAffected.Push(current);
            if (data <= current.data) 
            {
                if (current.left != null) { allAffected = Insert(data, current.left, allAffected); }
                else 
                { 
                    current.left = newNode;
                    allAffected.Push(newNode);
                }
            }
            else 
            {
                if (current.right != null) { allAffected = Insert(data, current.right, allAffected); }
                else 
                { 
                    current.right = newNode;
                    allAffected.Push(newNode);
                }
            }
            return allAffected;
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

        private void FixBalance(Stack<Node> allAffected)
        {
            Node? node = null;
            Node? parent = null;
            int count = 0;
            while (allAffected.Count != 0)
            {
                node = allAffected.Pop();
                if (node != head) { allAffected.TryPeek(out parent); }
                node.height = count;
                int bF = node.GetBalance();
                if (bF <= -2) 
                { 
                    LeftRotation(node, parent);
                    continue;
                }
                else if (bF >= 2) 
                { 
                    RightRotation(node, parent);
                    continue;
                }
                count++;
            }
        }
        private void LeftRotation(Node root, Node? parent)
        {
            Stack<Node> allAffected = new Stack<Node>();
            Node? rRight = root.right;
            root.right = rRight.left;
            rRight.left = root;
            allAffected.Push(rRight);
            allAffected.Push(root);
            if (root == head) 
            { 
                head = rRight;
                FixBalance(allAffected);
                return;
            }
            if (parent.left == root) { parent.left = rRight; }
            else { parent.right = rRight; }
            FixBalance(allAffected);
        }
        private void RightRotation(Node root, Node parent)
        {
            Stack<Node> allAffected = new Stack<Node>();
            Node? rLeft = root.left;
            root.left = rLeft.right;
            rLeft.right = root;
            allAffected.Push(rLeft);
            if (root == head) 
            { 
                head = rLeft;
                FixBalance(allAffected);
                return;
            }
            if (parent.left == root) { parent.left = rLeft; }
            else { parent.right = rLeft; }
            FixBalance(allAffected);
        }
    }

    internal class Node
    {
        public int data;
        public Node? left;
        public Node? right;

        public int height = 0;
        public int balFac;

        public Node(int d)
        {
            data = d;
        }

        public int GetBalance()
        {
            int leftH = 0;
            int rghtH = 0;
            if (left != null ) { leftH = 1 + left.height; }
            if (right != null) { rghtH = 1 + right.height; }
            balFac = leftH - rghtH;
            return balFac;
        }
    }
}
