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
    public int chunkSize = 16;
    public Grid tileMapPrefab;
    public PlayerController playerController;
    private Dictionary<Vector2Int, Grid> loadedChunks = new Dictionary<Vector2Int, Grid>();

    void Start()
    {
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
                if (x == chunkSize - 1 || y == chunkSize - 1 || x == 0 || y == 0)
                {
                    int tileNum = Random.Range(0, walls.Length);
                    newTilemap.SetTile(new Vector3Int(x, y), walls[tileNum]);
                }
                else
                {
                    int tileNum = Random.Range(0, tiles.Length);

                    newTilemap.SetTile(new Vector3Int(x, y), tiles[tileNum]);
                }
            }
        }
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
