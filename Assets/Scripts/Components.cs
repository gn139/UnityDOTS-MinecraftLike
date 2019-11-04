using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Voxel;

namespace Components {

    //========================================================================//
    public struct VoxelChunkTag : IComponentData { }

    public struct VoxelChunkNewborn : IComponentData {}

    public struct VoxelChunkChanged : IComponentData { }

    public struct VoxelCount : IComponentData {
        public int Value;
    }

    public struct VoxelChunkRelativePosition : IComponentData {
        public int X;
        public int Y;
        public int Z;
    }

    /// <summary>
    ///     The buffer of mesh vertices.
    /// </summary>
    [InternalBufferCapacity (4)]
    public struct Vertex : IBufferElementData {
        /// <summary>
        ///     The Vertex.
        /// </summary>
        public float3 Value;
    }

    /// <summary>
    ///     The buffer of mesh uvs.
    /// </summary>
    [InternalBufferCapacity (0)]
    public struct Uv : IBufferElementData {
        /// <summary>
        ///     The uv.
        /// </summary>
        public float3 Value;
    }

    /// <summary>
    ///     The buffer of mesh normals.
    /// </summary>
    [InternalBufferCapacity (4)]
    public struct Normal : IBufferElementData {
        /// <summary>
        ///     The normal.
        /// </summary>
        public float3 Value;
    }

    /// <summary>
    ///     The buffer of mesh triangles.
    /// </summary>
    [InternalBufferCapacity (6)]
    public struct Triangle : IBufferElementData {
        /// <summary>
        ///     The triangle.
        /// </summary>
        public int Value;
    }

    //========================================================================//
    public struct CountComponent : IComponentData {
        public int ChunkCount;
        public int VoxelCount;
    }

    //========================================================================//
    public struct TerrainSpawnerTag : IComponentData { }

    public struct TerrainTag : IComponentData { }

    [WriteGroup (typeof (LocalToWorld))]
    public struct TerrainSpawner : IComponentData {
        public Entity ChunkPrefab;
        // public int Count;
    }

    // public struct Prefabs : IComponentData {
    //     public Entity TerrainPrefab;
    //     public Entity GroundGreen;
    //     public Entity GroundBrown;
    //     public Entity Sand;
    //     public Entity Bush;
    //     public Entity GreyStone;

    // }

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