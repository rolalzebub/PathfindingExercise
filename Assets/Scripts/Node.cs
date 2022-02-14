using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    List<Vector3> edges;
    public bool isWalkable = true;
    Pathfinding pfMgr;
    Vector3 top_nbr, bot_nbr, left_nbr, right_nbr;

    private void Start()
    {
        edges = new List<Vector3>();
        pfMgr = FindObjectOfType<Pathfinding>();

        if (isWalkable)
        {
            top_nbr = new Vector3()
            {
                x = transform.position.x,
                y = transform.position.y + 1
            };

            bot_nbr = new Vector3()
            {
                x = transform.position.x,
                y = transform.position.y - 1
            };

            left_nbr = new Vector3()
            {
                x = transform.position.x - 1,
                y = transform.position.y
            };

            right_nbr = new Vector3()
            {
                x = transform.position.x + 1,
                y = transform.position.y
            };

            if (pfMgr.nodes.ContainsKey(top_nbr))
            {
                Node n;
                pfMgr.nodes.TryGetValue(top_nbr, out n);
                if (n.isWalkable)
                    AddEdge(top_nbr);
            }

            if (pfMgr.nodes.ContainsKey(bot_nbr))
            {
                Node n;
                pfMgr.nodes.TryGetValue(bot_nbr, out n);
                if (n.isWalkable)
                    AddEdge(bot_nbr);
            }

            if (pfMgr.nodes.ContainsKey(left_nbr))
            {
                Node n;
                pfMgr.nodes.TryGetValue(left_nbr, out n);
                if (n.isWalkable)
                    AddEdge(left_nbr);
            }

            if (pfMgr.nodes.ContainsKey(right_nbr))
            {
                Node n;
                pfMgr.nodes.TryGetValue(right_nbr, out n);
                if (n.isWalkable)
                    AddEdge(right_nbr);
            }
        }

        else
        {
            GetComponent<MeshRenderer>().material.color = Color.black;
        }
    }

    public void AddEdge(Vector3 destination)
    {
        Vector3 difference = destination - transform.position;

        if(difference.magnitude > 1)
        {
            return;
        }

        edges.Add(destination);

        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        lr.positionCount = 2 * edges.Count;
        int i = 0;

        foreach(Vector3 position in edges)
        {
            lr.SetPosition(i, transform.position);
            lr.SetPosition(i + 1, position);

            i += 2;
        }

        Debug.Log("Added edge");
    }

    public List<Vector3> GetConnections()
    {
        return edges;
    }
}
