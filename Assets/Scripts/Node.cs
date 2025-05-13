using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    
    private List<Node> childrenNodeList;

    public List<Node> ChildrenNodeList {
        get => childrenNodeList;
    }
    public bool Visited { get; set; }
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
    
    public int TreeLayerIndex { get; set; }
    public Node Parent { get; set; }

    public Node(Node parentNode) {
        childrenNodeList = new List<Node>();
        this.Parent = parentNode;
        if (parentNode != null) {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node) {
        childrenNodeList.Add(node);
    }

    public void RemoveChild(Node node) {
        childrenNodeList.Remove(node);   
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
