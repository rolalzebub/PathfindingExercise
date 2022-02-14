using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {


    //Creating the grid of nodes
    public int rows;
    public int columns;
    public GameObject nodePrefab;

    public Dictionary<Vector3, Node> nodes;
    Dictionary<Vector3, Vector3> nodeParents;

    void CreateGrid()
    {
        nodes = new Dictionary<Vector3, Node>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 newNode = new Vector3()
                {
                    x = transform.position.x + j,
                    y = transform.position.y + i
                };

                GameObject go = Instantiate(nodePrefab, newNode, Quaternion.identity);
                Node nodeObject = go.GetComponent<Node>();
                nodeObject.isWalkable = Random.Range(0, 2) > 0 ? true : false;

                nodes.Add(newNode, nodeObject);
            }
        }
    }

    void AddEdge(Vector3 start, Vector3 end)
    {
        if(!nodes.ContainsKey(start) ||
            !nodes.ContainsKey(end))
        {
            return;
        }

        Node n;
        nodes.TryGetValue(start, out n);
        n.AddEdge(end);
    }


    void Start () {
        CreateGrid();
        nodeParents = new Dictionary<Vector3, Vector3>();
    }

    public Vector3 GetNode(Vector3 startPosition, Vector3 endPosition)
    {
        List<Vector3> path = new List<Vector3>();

        if (!nodes.ContainsKey(startPosition))
        {
            return startPosition;
        }

        Queue<Vector3> queue = new Queue<Vector3>();
        HashSet<Vector3> exploredNodes = new HashSet<Vector3>();
        nodeParents = new Dictionary<Vector3, Vector3>();
        queue.Enqueue(startPosition);

        while (queue.Count != 0)
        {
            Vector3 currentNode = queue.Dequeue();

            if (currentNode == endPosition)
            {
                path.Add(currentNode);
                return currentNode;
            }

            Node n;
            nodes.TryGetValue(currentNode, out n);
            IList<Vector3> nodeConnections = n.GetConnections();

            foreach (Vector3 node in nodeConnections)
            {
                if (!exploredNodes.Contains(node))
                {
                    //Mark the node as explored
                    exploredNodes.Add(node);

                    //Store a reference to the previous node
                    nodeParents.Add(node, currentNode);

                    //Add this to the queue of nodes to examine
                    queue.Enqueue(node);
                }
            }
        }

        return startPosition;
    }
    

    public void ChangePathNodeColour(Vector3 goal)
    {
        RefreshGrid();
        List<Vector3> pathNodes = new List<Vector3>();
        Vector3 current = goal;

        while(current != transform.position)
        {
            pathNodes.Add(current);
            current = nodeParents[current];
        }
        pathNodes.Add(transform.position);

        foreach(Vector3 position in pathNodes)
        {
            Node n;
            nodes.TryGetValue(position, out n);
            n.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    void RefreshGrid()
    {
       foreach(Vector3 position in nodes.Keys)
       {
            Node n; 
            nodes.TryGetValue(position, out n);

            if(n.isWalkable)
                n.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

            else n.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
        }
    }
}
