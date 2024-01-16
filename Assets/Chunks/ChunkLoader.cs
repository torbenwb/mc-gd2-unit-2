using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    Dictionary<Vector3Int, Chunk> activeChunks = new Dictionary<Vector3Int, Chunk>();
    Queue<Chunk> inactiveChunks = new Queue<Chunk>();
    List<Vector3Int> chunksInRadius = new List<Vector3Int>();

    public int radius = 3;
    public GameObject chunkPrefab;

    public static ChunkLoader instance;

    public GameObject player;
    public float waitTime = 0.1f;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;

        TerrainGenerator.seed = Random.Range(25000, 35000);

        for(int x = -radius; x <= radius; x++){
            for(int y = -radius; y <= radius; y++){
                GameObject newChunk = Instantiate(chunkPrefab);
                Chunk chunk = newChunk.GetComponent<Chunk>();
                newChunk.SetActive(false);
                newChunk.transform.parent = transform;
                inactiveChunks.Enqueue(chunk);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence(){
        while(true){
            GetChunksCellsInRadius(Chunk.GetChunkCell(player.transform.position), radius);
            DestroyChunksOutsideRadius();
            LoadNextChunk();
            yield return new WaitForSeconds(waitTime);
        }
    }

    List<Vector3Int> GetChunksCellsInRadius(Vector3Int chunkCell, int radius){
        chunksInRadius = new List<Vector3Int>();

        for(int x = -radius; x <= radius; x++){
            for(int y = -radius; y <= radius; y++){
                Vector3Int offset = new Vector3Int(
                    x * Chunk.width,
                    0,
                    y * Chunk.width
                );
                chunksInRadius.Add(chunkCell + offset);
            }
        }
        return chunksInRadius;
    }

    void DestroyChunksOutsideRadius(){
        List<Vector3Int> destroyChunks = new List<Vector3Int>();

        foreach(KeyValuePair<Vector3Int, Chunk> pair in activeChunks){
            if (!chunksInRadius.Contains(pair.Key)) destroyChunks.Add(pair.Key);
        }

        for(int i = 0; i < destroyChunks.Count; i++){
            activeChunks[destroyChunks[i]].gameObject.SetActive(false);
            inactiveChunks.Enqueue(activeChunks[destroyChunks[i]]);
            activeChunks.Remove(destroyChunks[i]);
        }
    }

    void LoadNextChunk(){
        if (inactiveChunks.Count == 0) return;
        for(int i = 0; i < chunksInRadius.Count; i++){
            if (!activeChunks.ContainsKey(chunksInRadius[i])){
                Chunk chunkToLoad = inactiveChunks.Dequeue();
                chunkToLoad.transform.position = chunksInRadius[i];
                chunkToLoad.BuildMesh();
                chunkToLoad.gameObject.SetActive(true);
                activeChunks.Add(chunksInRadius[i], chunkToLoad);
                break;
            }
        }
    }

    public void ReloadActiveChunk(Vector3Int chunkCell){
        if (activeChunks.ContainsKey(chunkCell)){
            activeChunks[chunkCell].BuildMesh();
        }
    }
}
