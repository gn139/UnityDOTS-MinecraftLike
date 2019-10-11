using Unity.Entities;
using Unity.Mathematics;

namespace Components {
    public struct VoxelTag : IComponentData { }
    public struct VoxelParentIndex : IComponentData {
        public int Value;
    }
    public struct VoxelPosition : IComponentData {
        public float3 Value;
    }
    //========================================================================//
    public struct VoxelChunkTag : IComponentData { }

    public struct VoxelChunkNeedHide : IComponentData { }

    public struct VoxelChunkHidden : IComponentData { }

    public struct WithOutVoxel : IComponentData { }

    public struct VoxelCount : IComponentData {
        public int Value;
    }

    public struct VoxelChunkRelativePosition : IComponentData {
        public int X;
        public int Y;
        public int Z;
    }

    //========================================================================//
    public struct TerrainSpawnerTag : IComponentData { }
    public struct TerrainSpawner : IComponentData {
        public Prefabs Prefabs;
        public int Count;
    }

    public struct Prefabs : IComponentData {
        public Entity NonePrefab;
        public Entity GroundGreen;
        public Entity GroundBrown;
        public Entity Sand;
        public Entity Bush;
        public Entity GreyStone;

    }

    //========================================================================//
    public struct PlayerTag : IComponentData { }

    public struct PlayerNotInGround : IComponentData {
        public Entity Player;
    }

    //========================================================================//
    public struct CameraControlData : IComponentData {
        public float3 PivotOffset;
        public float3 CameraOffset;
        public float AngleHorizontal;
        public float AngleVertical;
        public float3 RelCameraPos;
        public float RelCameraPosLength;
        public float3 SmoothPivotOffset; // Offset to repoint the camera.
        public float3 SmoothCameraOffset; // Offset to relocate the camera related to the player position.
        public float3 TargetPivotOffset; // Offset to repoint the camera.
        public float3 TargetCameraOffset;
        public float TargetMaxVerticalAngle;
        public float TargetFOV;
        public float Smooth; // Speed of camera responsiveness.
        public float HorizontalAimingSpeed; // Horizontal turn speed.
        public float VerticalAimingSpeed; // Vertical turn speed.
        public float MaxVerticalAngle; // Camera max clamp angle. 
        public float MinVerticalAngle; // Camera min clamp angle.
        public float ShapeHeight;
        public bool IsTrigger;
    }

    //========================================================================//

    public struct WireframeTag : IComponentData { }

    //========================================================================//

    public struct WireframeSpawner : IComponentData {
        public Entity Prefab;
    }

    public struct CubePointerTag : IComponentData { }
}