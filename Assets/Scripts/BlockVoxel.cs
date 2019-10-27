using Unity.Entities;

namespace Voxel {
    [InternalBufferCapacity (0)]
    public struct BlockVoxel : IBufferElementData {
        public VoxelState Type;

        public BlockVoxel (VoxelState type) {
            Type = type;
        }
    }

    public enum VoxelState {
        Empty,
        None,
        Already
    }

    public enum Face {
        Up,
        Down,
        Left,
        Right,
        Front,
        Back,
    }
}