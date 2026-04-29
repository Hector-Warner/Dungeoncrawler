using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public List<Vector3Int> paths = new List<Vector3Int>();

    void Start()
    {
        paths.Add(new Vector3Int(50, 0, 5));
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
                    int tileNum = Random.Range(0, pathTiles.Length);
                    newTilemap.SetTile(new Vector3Int(x, y), pathTiles[tileNum]);
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
            float multiplicative = (float)(paths[i].y - tileCoord.y) / (paths[i].x - tileCoord.x);
            if (Mathf.FloorToInt(multiplicative * tileCoord.x + paths[i].z) == tileCoord.y)
            {
                int floored = Mathf.FloorToInt(multiplicative * tileCoord.x + paths[i].z);
                Debug.Log(floored);
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
