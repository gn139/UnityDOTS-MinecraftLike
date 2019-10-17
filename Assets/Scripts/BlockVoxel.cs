using Unity.Entities;

namespace Voxel {
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