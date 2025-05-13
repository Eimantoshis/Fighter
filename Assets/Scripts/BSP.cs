using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSP : MonoBehaviour {
    private RoomNode rootNode;

    public BSP(int dungeonWidth, int dungeonLength) {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
    }
    
    public RoomNode RootNode {
        get => rootNode;
    }

    public List<RoomNode> PrepareNodesCollection(int maxRooms, int roomWidthMin, int roomLengthMin) {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(rootNode);
        listToReturn.Add(rootNode);
        int iterations = 0;
        while (iterations < maxRooms && graph.Count > 0) {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if (currentNode.Width >= roomWidthMin * 2 && currentNode.Length >= roomLengthMin * 2) {
                SplitTheSpace(currentNode, roomWidthMin, roomLengthMin, graph, listToReturn);
            }
        }
        return listToReturn;
    }
    
    private void SplitTheSpace(RoomNode currentNode, int roomWidthMin, int roomLengthMin, Queue<RoomNode> graph, List<RoomNode> listToReturn) {


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
