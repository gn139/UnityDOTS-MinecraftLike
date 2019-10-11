using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class TerrainSpawnerAuthor : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public GameObject NonePrefab;
    public GameObject GroundGreen;
    public GameObject GroundBrown;
    public GameObject Sand;
    public GameObject Bush;
    public GameObject GreyStone;

    public void Convert (Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData (entity, new TerrainSpawner {
            Prefabs = new Prefabs {
                    NonePrefab = conversionSystem.GetPrimaryEntity (NonePrefab),
                        Sand = conversionSystem.GetPrimaryEntity (Sand),
                        GroundBrown = conversionSystem.GetPrimaryEntity (GroundBrown),
                        GroundGreen = conversionSystem.GetPrimaryEntity (GroundGreen),
                        Bush = conversionSystem.GetPrimaryEntity (Bush),
                        GreyStone = conversionSystem.GetPrimaryEntity (GreyStone)
                },
                Count = 4
        });
        dstManager.AddComponentData (entity, new TerrainSpawnerTag ());
    }

    public void DeclareReferencedPrefabs (List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add (NonePrefab);
        referencedPrefabs.Add (Sand);
        referencedPrefabs.Add (GroundGreen);
        referencedPrefabs.Add (GroundBrown);
        referencedPrefabs.Add (Bush);
        referencedPrefabs.Add (GreyStone);
    }

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
}