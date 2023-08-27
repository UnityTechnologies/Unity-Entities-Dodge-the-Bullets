using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// OnUpdate를 BulletVelocitySystem의 OnUpdate 이후에 실행
[UpdateAfter(typeof(BulletVelocitySystem))]
public partial struct BulletDestroySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // 총알이 없으면 실행할 필요가 없다.
        state.RequireForUpdate<Bullet>();
        
        // 엔티티를 파괴하는 명령을 삽입하기 위해
        // 엔티티 커맨드 버퍼(ECB - Entity CommandBuffer)가 필요하다.
        // 엔티티 커맨드 버퍼는 스레드 세이프한 명령어들을 쌓아놓는 큐
        
        // 그 중에서도 프레임의 시뮬레이션 시작 부분에 커맨드를 삽입할 수 있는 BeginSimulationEntityCommandBufferSystem 사용
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        // 커맨드 버퍼 시스템 싱글톤을 가져온다
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        
        // Unmanaged에 해당하는 월드를 가져오와 (Unmanaged 타입 - GC 대상이 아니고 클래스 인스턴스가 아닌 타입들)
        // 엔티티 커맨드 버퍼를 생성한다.
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        // 총알이 화면 밖으로 나가면 파괴한다
        // WithAll로 총알을 찾고, WithEntityAccess로 엔티티를 가져온다.
        foreach (var (transform, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>>()
                     .WithAll<Bullet>()
                     .WithEntityAccess())
        {
            if (math.abs(transform.ValueRO.Position.x) > 40 || math.abs(transform.ValueRO.Position.z) > 40)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
}