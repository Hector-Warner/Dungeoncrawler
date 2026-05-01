using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class TileGenerationScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Tile[] tiles;
    public Tile[] walls;
    public Tile[] pathTiles;
    public int chunkSize = 16;
    public Grid tileMapPrefab;
    public PlayerController playerController;
    private Dictionary<Vector2Int, Grid> loadedChunks = new Dictionary<Vector2Int, Grid>();
    public List<int[]> paths = new List<int[]>();
    public int pathBend = 0;

    void Start()
    {
        // X1 Y1 X2 Y2 YIntercept
        for (int i = 0; i < 12; i++)
        {
            createPath(Random.Range(-500,500), Random.Range(-500, 500), Random.Range(-500, 500), Random.Range(-500, 500));
        }
        
    }

    void createPath(int x1, int y1, int x2, int y2)
    {
        int yInter = (int) calculateYIntercept(x1, y1, x2, y2);
        int[] path = { x1, y1, x2, y2, yInter };
        paths.Add(path);
    }

    int calculateYIntercept(int x1, int y1, int x2, int y2) 
    {
        int yIntercept = Mathf.FloorToInt(y1-(y2-y1)/(x2-x1)*x1);
        return yIntercept;
    }

    public void createChunk(Vector2Int chunkStart)
    {
        Grid newChunk = Instantiate(tileMapPrefab);
        Tilemap newTilemap = newChunk.GetComponentInChildren<Tilemap>();
        loadedChunks.Add(chunkStart, newChunk);
        newChunk.transform.position = new Vector3Int(chunkStart.x*chunkSize, chunkStart.y*chunkSize);
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                if ((x == chunkSize - 1 || y == chunkSize - 1 || x == 0 || y == 0) && false)
                {
                    int tileNum = Random.Range(0, walls.Length);
                    newTilemap.SetTile(new Vector3Int(x, y), walls[tileNum]);
                }
                else if (checkTile(new Vector2Int(chunkStart.x * chunkSize + x, chunkStart.y * chunkSize + y)) == 1)
                {
                    int randomNum = Random.Range(0, 3);
                    if (randomNum == 0 || randomNum == 1) 
                    {
                        int tileNum = Random.Range(0, pathTiles.Length);
                        newTilemap.SetTile(new Vector3Int(x, y), pathTiles[tileNum]);
                    } else
                    {
                        int tileNum = Random.Range(0, tiles.Length);

                        newTilemap.SetTile(new Vector3Int(x, y), tiles[tileNum]);
                    }
                    
                    Debug.Log("Path: " + chunkStart.x * chunkSize + x + " " + chunkStart.y * chunkSize + y);
                } else
                {
                    int tileNum = Random.Range(0, tiles.Length);

                    newTilemap.SetTile(new Vector3Int(x, y), tiles[tileNum]);
                }
            }
        }
    }

    public int checkTile(Vector2Int tileCoord)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            float multiplicativeY = (float)(paths[i][1] - paths[i][3]) / (paths[i][0] - paths[i][2]);
            float multiplicativeX = (float)(paths[i][0] - paths[i][2]) / (paths[i][1] - paths[i][3]);
            if ((Mathf.Abs(Mathf.FloorToInt(multiplicativeY * tileCoord.x + paths[i][4]) - tileCoord.y) < 4 || Mathf.Abs(Mathf.FloorToInt(multiplicativeX * (tileCoord.y - paths[i][4])) - tileCoord.x) < 4) 
                && tileCoord.x >= Mathf.Min(paths[i][0], paths[i][2])
                && tileCoord.x <= Mathf.Max(paths[i][0], paths[i][2])
                )
            {
                return 1;
            }
        }
        return 0;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2Int currentChunk = new Vector2Int(Mathf.FloorToInt(playerController.transform.position.x / chunkSize), Mathf.FloorToInt(playerController.transform.position.y / chunkSize));
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int targetCoord = new Vector2Int(currentChunk.x + x, currentChunk.y + y);
                if (!loadedChunks.ContainsKey(targetCoord))
                {
                    createChunk(targetCoord);
                }
            }
        }
    }
}
