using Components;
using Unity.Entities;
using UnityEngine;

public class CubePointerAuthor : MonoBehaviour, IConvertGameObjectToEntity {
    public void Convert (Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

        dstManager.AddComponentData (entity, new CubePointerTag ());
    }

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
}