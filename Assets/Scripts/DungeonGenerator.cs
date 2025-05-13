using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
    private int dungeonWidth;
    private int dungeonLength;

    
    private List<RoomNode> allSpaceNodes = new List<RoomNode>();

    public DungeonGenerator(int dungeonWidth, int dungeonLength) {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }

    public List<Node> CalculateRooms(int maxRooms, int roomWidthMin, int roomLengthMin) {
        BSP bsp = new BSP(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxRooms, roomWidthMin, roomLengthMin);
        foreach(var node in allSpaceNodes)
        {
            Debug.Log(node.Width + " " + node.Length);
        }
        return new List<Node>(allSpaceNodes);
    }
    
    
    // Start is called before the first frame update
    void Start() {
    }

    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
