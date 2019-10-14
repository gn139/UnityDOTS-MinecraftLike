# UnityDOTS-MincraftLike

## Unity ECS

In order to use Unity ECS we need to do some preparation first.

[How to use Unity ECS](https://github.com/Unity-Technologies/EntityComponentSystemSamples#installation-guide-for-blank-ecs-project)

[this way to documentation.](https://docs.unity3d.com/Packages/com.unity.entities@0.0/manual/index.html)

## Unity Physics

To use Unity Physics also need Installation from Package Manager. 

[The official sample](https://github.com/Unity-Technologies/EntityComponentSystemSamples/tree/master/UnityPhysicsSamples) 

[sample documentation](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/UnityPhysicsSamples/Documentation/samples.md).

[Unity Physics Documentation](https://docs.unity3d.com/Packages/com.unity.physics@0.0/manual/index.html)

## Unity InputSystem

[The new input system for Unity](https://github.com/Unity-Technologies/InputSystem)

Because CharacterController from the official physics sample use this, so I tried to use it.

## Voxel And Chunk

Usually, when generating the world of blocks similar to Minecraft, we use the Mesh of the Chunk GameObject.Since Unity ECS is used in this project, I have been trying to achieve this step through multi-threading, but I have not found a good way so far.After reading [Minecraft - the Sandbox] (https://github.com/Michona/Minecraft-Sandbox) and [MinecraftECS] (https://github.com/UnityTechnologies/MinecraftECS), I found that Cube can be used directly in the Unity ECS to generate the world without affecting performance.So every Voxel in the project is an Entity that can be rendered, and every Chunk is just an Entity that contains logical data.

通常情况下，在生成类似Minecraft的方块世界时，大家会采用自定义Chunk GameObject的Mesh来实现。在这个项目中因为使用了Unity ECS，我一直想通过多线程的方式来实现这一步，但目前为止没有找到好的方法。在阅读了[Minecraft-Sandbox](https://github.com/Michona/Minecraft-Sandbox) 和 [MinecraftECS](https://github.com/UnityTechnologies/MinecraftECS)后发现，Unity ECS中可以直接使用Cube来生成世界并且不会影响性能。所以该项目中的每一个Voxel都是可以被渲染的Entity，而每一个Chunk则只是一个包含逻辑数据的Entity。

## To Do List

- [ ] Use JobSystem to update Mesh
- [ ] Generate more complex world
- [ ] Create biology in the world
- [ ] Navigation with Unity ECS

## Others

I copied the Boids implementation from the official example for performance testing. My hardwares are as follows:

- CPU: Intel i5-7500
- RAM: 8 GB
- GPU: NVIDIA GeForce GTX 1050

Can stabilize at 80~60 frames.