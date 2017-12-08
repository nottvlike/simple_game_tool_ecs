public interface IPoolObject
{
    string PoolName { get; }
    bool IsInUse { get; set; }
}
