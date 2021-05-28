using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MDB.Locations
{
    public class NodeDisplayer : MonoBehaviour
    {
        [SerializeField] private GameObject nodeColumnPrefab;
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GameObject nodeConnectorPrefab;
        [SerializeField] private GameObject enemyPrefab;

        private void Start()
        {
            //Debugging purposes, manually making a map
            // ReSharper disable  Unity.IncorrectMonoBehaviourInstantiation
            Node node1 = Instantiate(nodePrefab).GetComponent<Node>();

            Node node2 = Instantiate(nodePrefab).GetComponent<Node>();
            Node node3 = Instantiate(nodePrefab).GetComponent<Node>();

            node1.SetValues(0, new List<Node>{node2, node3}, enemyPrefab, null);

            Node node4 = Instantiate(nodePrefab).GetComponent<Node>();
            Node node5 = Instantiate(nodePrefab).GetComponent<Node>();

            node2.SetValues(1, new List<Node>{node4, node5}, enemyPrefab, node1);
//
            node3.SetValues(1, new List<Node>(), enemyPrefab, node1);
//
            Node node6 = Instantiate(nodePrefab).GetComponent<Node>();

            node4.SetValues(2, new List<Node> {node6}, enemyPrefab, node2);

            node5.SetValues(2, new List<Node>(), enemyPrefab, node2);

            node6.SetValues(3, new List<Node>(), enemyPrefab, node4);

            GenerateView(node1);
            //End of debugging garbage
        }

        /// <summary>
        /// Take the starting node and generate a view off of that
        /// </summary>
        /// <param name="rootNode">The first node</param>
        public void GenerateView(Node rootNode)
        {
            int deepestNode = FindDeepestNode(rootNode); //Find how deep the map goes

            var columns = new GameObject[deepestNode + 1];

            //Create columns of nodes equal to deepestNode + 1
            for (int i = 0; i <= deepestNode; i++)
            {
                columns[i] = Instantiate(nodeColumnPrefab, transform);
            }

            //Place the nodes in their columns
            CreateNodes(columns, rootNode);

            //Force the layout to be rebuilt before end of frame so we can create the connectors
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            foreach (var column in columns)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(column.GetComponent<RectTransform>());
            }

            CreateConnectors(rootNode);
        }

        /// <summary>
        /// Goes through all nodes and finds the deepest one
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        private int FindDeepestNode(Node parentNode)
        {
            //Is the node the last in its branch?
            if (parentNode.ChildNodes.Count == 0)
            {
                return parentNode.DistFromRoot;
            }
            //Otherwise do recursive function time
            else
            {
                int deepestNode = 0;
                foreach (var node in parentNode.ChildNodes)
                {
                    int thisNodeDepth = FindDeepestNode(node);
                    if (thisNodeDepth > deepestNode)
                    {
                        deepestNode = thisNodeDepth;
                    }
                }

                return deepestNode;
            }
        }

        /// <summary>
        /// Places nodes in their corresponding columns based on dist from root
        /// </summary>
        /// <param name="columns">Array of columns</param>
        /// <param name="parentNode"></param>
        private void CreateNodes(GameObject[] columns, Node parentNode)
        {
            parentNode.transform.SetParent(columns[parentNode.DistFromRoot].transform);
            if (parentNode.ChildNodes.Count != 0)
            {
                foreach (var node in parentNode.ChildNodes)
                {
                    CreateNodes(columns, node);
                }
            }
        }

        /// <summary>
        /// Draw lines between nodes
        /// </summary>
        /// <param name="parentNode">Node to which draw line from</param>
        private void CreateConnectors(Node parentNode)
        {
            if (parentNode.ChildNodes.Count != 0)
            {
                foreach (var node in parentNode.ChildNodes)
                {
                    var connector = Instantiate(nodeConnectorPrefab, transform);
                    RectTransform parentTransform = parentNode.gameObject.GetComponent<RectTransform>();
                    RectTransform nodeTransform = node.gameObject.GetComponent<RectTransform>();
                    connector.GetComponent<NodeConnector>().CreateConnection(parentTransform, nodeTransform);
                    CreateConnectors(node);
                }
            }
        }


    }
}