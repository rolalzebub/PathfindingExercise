using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {

    Pathfinding pfMgr;
    int selectedNodes = 0;
    List<Node> selectedNodesList;
    Vector3 pathStart;

	// Use this for initialization
	void Start () {
        pfMgr = GetComponent<Pathfinding>();
        selectedNodesList = new List<Node>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mouse = new Vector3()
        {
            x = Mathf.RoundToInt( Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
            y = Mathf.RoundToInt( Camera.main.ScreenToWorldPoint(Input.mousePosition).y),
            z = 0f
        };

        if (pfMgr.nodes.ContainsKey(mouse))
        {
            Vector3 path = pfMgr.GetNode(transform.position, mouse);

            if (path != null)
            {
                pfMgr.ChangePathNodeColour(path);
            }
        }

        if (Input.GetMouseButtonDown(0) &&
            pfMgr.nodes.ContainsKey(mouse))
        {
            Node m;
            pfMgr.nodes.TryGetValue(mouse, out m);

            pathStart = m.transform.position;

        }

    }
}
