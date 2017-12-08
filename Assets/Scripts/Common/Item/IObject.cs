public interface IObject
{
    int Id { get; }
    string Name { get; }
    int Group { get; }
    int SubGroup { get; }

    bool IsInit();
	void Init();
	void Destroy();
}