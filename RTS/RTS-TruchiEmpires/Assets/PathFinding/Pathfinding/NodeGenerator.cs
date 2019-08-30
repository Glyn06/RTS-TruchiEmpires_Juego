using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    [SerializeField] GameObject nodeObject;
    [SerializeField] Transform nodeParent;
    [SerializeField] Transform ground;
    public GameObject cubito; 

    List<List<Node>> nodes;
    int TerrainSize;

    void Awake()
    {
        TerrainSize = (int)ground.localScale.x /** 10 - 1*/;

        nodes = new List<List<Node>>();
        for (int i = 0; i < TerrainSize; i++)
            nodes.Add(new List<Node>());

        for (int i = 0; i < TerrainSize; i++)
            for (int j = 0; j < TerrainSize; j++)
                nodes[i].Add(null);
    }

    void Start()
    {
        Debug.Log("ENTRE AL START VIEJA");
        GenerateNodes();
    }
    public void GenerateNodes()
    {
        Vector3 actualPos = Vector3.zero;
        if (TerrainSize % 2 == 0)
        {
            actualPos = new Vector3(-(TerrainSize / 2) + 0.5f, 0.5f, -(TerrainSize / 2) + 0.5f);
        }
        else if (TerrainSize % 2 != 0)
        {
            actualPos = new Vector3(-(TerrainSize / 2), 0.5f, -(TerrainSize / 2));
        }
        for (int i = 0; i < TerrainSize; i++)
        {
            for (int j = 0; j < TerrainSize; j++)
            {
                RaycastHit hit;
                Instantiate(cubito, actualPos, Quaternion.identity);
                if (Physics.Raycast(actualPos, Vector3.down, out hit, actualPos.y))
                {
                    if (hit.collider.tag != "Obstaculo")
                    {
                        Debug.Log("ENTRE");
                        Node node = Instantiate(nodeObject, actualPos + new Vector3(0,hit.transform.position.y+0.5f,0), Quaternion.identity, nodeParent).GetComponent<Node>();
                        nodes[i][j] = node;

                        if (hit.collider.tag == "Mineral" || hit.collider.tag == "Centro Urbano")
                            node.IsObstacle = true;
                    }
                }
                actualPos.x += 1.0f;
            }
            actualPos.x = -(TerrainSize / 2);
            actualPos.z += 1.0f;
            //transform.position = actualPos;
        }

        for (int i = 0; i < TerrainSize; i++)
        {
            for (int j = 0; j < TerrainSize; j++)
            {
                if (nodes[i][j])
                {
                    // Directos
                    if (i + 1 < TerrainSize && nodes[i + 1][j]) nodes[i][j].AddAdyNode(nodes[i + 1][j], NodeAdyType.Straight, AdyDirection.Up);
                    if (j + 1 < TerrainSize && nodes[i][j + 1]) nodes[i][j].AddAdyNode(nodes[i][j + 1], NodeAdyType.Straight, AdyDirection.Right);
                    if (i - 1 >= 0 && nodes[i - 1][j]) nodes[i][j].AddAdyNode(nodes[i - 1][j], NodeAdyType.Straight, AdyDirection.Down);
                    if (j - 1 >= 0 && nodes[i][j - 1]) nodes[i][j].AddAdyNode(nodes[i][j - 1], NodeAdyType.Straight, AdyDirection.Left);

                    // Diagonales
                    if (i + 1 < TerrainSize && j + 1 < TerrainSize && nodes[i + 1][j + 1]) nodes[i][j].AddAdyNode(nodes[i + 1][j + 1], NodeAdyType.Diagonal, AdyDirection.UpRight);
                    if (i + 1 < TerrainSize && j - 1 >= 0 && nodes[i + 1][j - 1]) nodes[i][j].AddAdyNode(nodes[i + 1][j - 1], NodeAdyType.Diagonal, AdyDirection.UpLeft);
                    if (i - 1 >= 0 && j + 1 < TerrainSize && nodes[i - 1][j + 1]) nodes[i][j].AddAdyNode(nodes[i - 1][j + 1], NodeAdyType.Diagonal, AdyDirection.DownRight);
                    if (i - 1 >= 0 && j - 1 >= 0 && nodes[i - 1][j - 1]) nodes[i][j].AddAdyNode(nodes[i - 1][j - 1], NodeAdyType.Diagonal, AdyDirection.DownLeft);
                }
            }
        }
    }
}
