using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainiPosition;
    public LayerMask terrainMask;
    PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        pm=FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChcker();
    }
    void ChunkChcker(){
        if(pm.moveDir.x>0&&pm.moveDir.y==0){
            if(!Physics2D.OverlapCircle(player.transform.position+new Vector3(20,0,0),checkerRadius,terrainMask)){
                noTerrainiPosition=player.transform.position+new Vector3(20,0,0);
                SpawnChunk();
            }
        } else if(pm.moveDir.x<0&&pm.moveDir.y==0){
            if(!Physics2D.OverlapCircle(player.transform.position+new Vector3(-20,0,0),checkerRadius,terrainMask)){
                noTerrainiPosition=player.transform.position+new Vector3(-20,0,0);
                SpawnChunk();
            }
        }else if(pm.moveDir.x==0&&pm.moveDir.y>0){
            if(!Physics2D.OverlapCircle(player.transform.position+new Vector3(0,20,0),checkerRadius,terrainMask)){
                noTerrainiPosition=player.transform.position+new Vector3(0,20,0);
                SpawnChunk();
            }
        }else if(pm.moveDir.x==0&&pm.moveDir.y<0){
            if(!Physics2D.OverlapCircle(player.transform.position+new Vector3(0,-20,0),checkerRadius,terrainMask)){
                noTerrainiPosition=player.transform.position+new Vector3(0,-20,0);
                SpawnChunk();
            }
        }
    }
    void SpawnChunk(){
        int rand=Random.Range(0,terrainChunks.Count);
        Instantiate(terrainChunks[rand],noTerrainiPosition,Quaternion.identity);
    }
}
