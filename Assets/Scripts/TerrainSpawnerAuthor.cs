using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

[RequiresEntityConversion]
public class TerrainSpawnerAuthor : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public GameObject ChunkPrefab;
    // public GameObject GroundGreen;
    // public GameObject GroundBrown;
    // public GameObject Sand;
    // public GameObject Bush;
    // public GameObject GreyStone;

    public int ChunkCount = 4;
    public int VoxelCount = 16;

    public void Convert (Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

        var countSingle = dstManager.CreateEntity (typeof (CountComponent));
        var countQuery = dstManager.CreateEntityQuery (typeof (CountComponent));
        countQuery.SetSingleton (new CountComponent {
            VoxelCount = VoxelCount,
                ChunkCount = ChunkCount
        });
        var chunkEntity = conversionSystem.GetPrimaryEntity (ChunkPrefab);;
        dstManager.AddSharedComponentData (chunkEntity, new RenderMesh {
            mesh = ChunkPrefab.GetComponent<MeshFilter> ().sharedMesh,
                material = ChunkPrefab.GetComponent<Renderer> ().sharedMaterial
        });

        dstManager.AddComponentData (entity, new TerrainSpawner {
            ChunkPrefab = chunkEntity
            // Prefabs = new Prefabs {
            //     // NonePrefab = conversionSystem.GetPrimaryEntity (NonePrefab),
            //         // Sand = conversionSystem.GetPrimaryEntity (Sand),
            //         // GroundBrown = conversionSystem.GetPrimaryEntity (GroundBrown),
            //         // GroundGreen = conversionSystem.GetPrimaryEntity (GroundGreen),
            //         // Bush = conversionSystem.GetPrimaryEntity (Bush),
            //         // GreyStone = conversionSystem.GetPrimaryEntity (GreyStone)
            // },
        });
        dstManager.AddComponentData (entity, new TerrainSpawnerTag ());
    }

    public void DeclareReferencedPrefabs (List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add (ChunkPrefab);
        // referencedPrefabs.Add (Sand);
        // referencedPrefabs.Add (GroundGreen);
        // referencedPrefabs.Add (GroundBrown);
        // referencedPrefabs.Add (Bush);
        // referencedPrefabs.Add (GreyStone);
    }

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
}