using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EntityTracker : MonoBehaviour, IReceiveEntity {
    private Entity EntityToTrack = Entity.Null;
    public void SetReceivedEntity (Entity entity) {
        EntityToTrack = entity;
    }

    // Update is called once per frame
    void LateUpdate () {
        if (EntityToTrack != Entity.Null) {
            try {
                var em = World.Active.EntityManager;
                var position = em.GetComponentData<Translation> (EntityToTrack).Value;
                var rotation = em.GetComponentData<Rotation> (EntityToTrack).Value;
                if (!float.IsNaN (position.x) && !float.IsNaN (position.y) && !float.IsNaN (position.z)) {

                    transform.position = position;
                }

                transform.rotation = rotation;
            } catch {
                // Dirty way to check for an Entity that no longer exists.
                EntityToTrack = Entity.Null;
            }
        }
    }
}