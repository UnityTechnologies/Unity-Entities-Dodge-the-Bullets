using Unity.Entities;
using Unity.Mathematics;

// 움직이는 물체의 속도 데이터를 구현
public struct Movement : IComponentData
{
    public float3 Velocity;
}