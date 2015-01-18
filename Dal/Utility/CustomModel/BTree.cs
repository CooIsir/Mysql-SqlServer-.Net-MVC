using System;
using System.Collections.Generic;
using System.Text;

namespace com.Utility
{
    public class BTreeNode<T> where T : IComparable<T>
    {
        private T _nodeNodeData;
        private BTreeNode<T> _LeftNode;
        private BTreeNode<T> _RightNode;

        public  T NodeData 
        {
            get{return _nodeNodeData;}
            set{_nodeNodeData = value;}
        }
        public BTreeNode<T> LeftNode
        {
            get{return _LeftNode ;}
            set{_LeftNode = value;}
        }
        public BTreeNode<T> RightNode
        {
            get { return _RightNode; }
            set { _RightNode = value; }
        }
        public BTreeNode(T item)
        {
            _nodeNodeData = item;
            _LeftNode = _RightNode = null;
        } 
    }

    public class BTree<T> where T : IComparable<T>
    {
        public BTreeNode<T> root; 
        public BTree()
        {
            root = null;
        }
        public void Destroy(BTreeNode<T> node)
        {
            if (root != null)
            {
                Destroy(root.LeftNode);
                Destroy(root.RightNode);
                root = null;
            }
        }
        public void CreateBTree(List<T> list)
        { 
            foreach (T temp in list)
            { 
                Insert(temp);
            }
        }
        public void Inorder(BTreeNode<T> root)
        {
            if (root == null)
                return;
            Inorder(root.LeftNode );
            //root.PrintNode();
            Inorder(root.RightNode);
        }
        public void Firstorder(BTreeNode<T> root)
        {
            if (root == null)
                return;
            //root.PrintNode();
            Firstorder(root.LeftNode);
            Firstorder(root.RightNode);
        }
        public void Backorder(BTreeNode<T> root)
        {
            if (root == null)
                return;
            Backorder(root.LeftNode);
            Backorder(root.RightNode);
            //root.PrintNode();
        }
        public BTreeNode<T> search(T item)
        {
            BTreeNode<T> temp = null;
            if (root.NodeData.Equals(item))
                temp = root;
            else
                temp = search(root, item);
            return temp;
        }
        public BTreeNode<T> search(BTreeNode<T> root, T item)
        {
            BTreeNode<T> tmp = null;
            if (root.NodeData.Equals(item))
            {
                tmp = root;
                return tmp;
            }
            int result = Comparer<T>.Default.Compare(root.NodeData, item);
            if (result > 0)
            {
                if (root.LeftNode != null)
                    tmp = search(root.LeftNode, item);
            }
            else
            {
                if (root.RightNode != null)
                    tmp = search(root.RightNode, item);
            }
            return tmp;
        }
        public void Insert(T item)
        {
            if (root == null)
            {
                root = new BTreeNode<T>(item);
            }
            else
                Insert(root, item);
        }
        public void Insert(BTreeNode<T> node, T item)
        {
            int result = Comparer<T>.Default.Compare(node.NodeData, item);
            if (result >= 0)
            {
                if (node.LeftNode == null)
                {
                    BTreeNode<T> temp = new BTreeNode<T>(item);
                    node.LeftNode = temp;
                }
                else
                    Insert(node.LeftNode, item);
            }
            else
            {
                if (node.RightNode == null)
                {
                    BTreeNode<T> temp = new BTreeNode<T>(item);
                    node.RightNode = temp;
                }
                else
                    Insert(node.RightNode, item);
            }
        }
        public BTreeNode<T> MinRight(BTreeNode<T> node)
        {
            if (node.LeftNode == null)
                return node;
            else
                return MinRight(node.LeftNode);
        }

        public BTreeNode<T> searchParent(BTreeNode<T> head, BTreeNode<T> p)
        {
            if (head.LeftNode == p || head.RightNode == p || head == null)
                return head;
            int result = Comparer<T>.Default.Compare(p.NodeData, head.NodeData);
            if (result <= 0)
                return searchParent(head.LeftNode, p);
            else
                return searchParent(head.RightNode, p);
        }

        public void delete(T item)
        {
            BTreeNode<T> p = null;

            p = search(root, item);

            if (p == null) // p is not the item of the BTree
            {
                // don't find the item 
                return;
            }
            if (p == root) // p is the root node
            {
                if (root.LeftNode != null || root.RightNode != null)
                {
                    // BTree root node can't be delete yet 
                    return;
                }
                else
                {
                    root = null;
                    // BTree root node has been deleted 
                    return;
                }
            }

            else // p is not the root node
            {
                BTreeNode<T> parent = null;
                parent = searchParent(root, p);
                if (p.LeftNode == null && p.RightNode == null) // p is leaf node
                {
                    if (parent.LeftNode == p)
                        parent.LeftNode = null;
                    else
                        parent.RightNode = null;
                }
                else //p is not leaf node
                {
                    if (p.RightNode == null) // p has no right child node
                    {
                        if (parent.LeftNode == p)
                            parent.LeftNode = p.LeftNode;
                        else
                            parent.RightNode = p.LeftNode;
                    }
                    if (p.LeftNode == null) // p has no left child node
                    {
                        if (p.LeftNode == p)
                            parent.LeftNode = p.RightNode;
                        else
                            parent.RightNode = p.RightNode;
                    }
                    else if (p.LeftNode != null && p.RightNode != null) // p has both left node and right node
                    {
                        BTreeNode<T> rightMinSon, rightMinParent;
                        rightMinSon = MinRight(p.RightNode);
                        rightMinParent = searchParent(p.RightNode, rightMinSon);

                        rightMinParent.LeftNode = rightMinSon.RightNode;

                        if (p.RightNode == rightMinSon)
                        {
                            p.RightNode = rightMinSon.RightNode;
                        }
                        p.NodeData = rightMinSon.NodeData;
                    }
                }
            }
        } // end of delete
    } // end of BTree class
}