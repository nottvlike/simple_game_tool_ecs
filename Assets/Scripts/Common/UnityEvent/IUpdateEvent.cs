public interface IUpdateEvent
{
	void Update();
}

public interface IFixedUpdateEvent
{
    void FixedUpdate();
}

public interface ILateUpdateEvent
{
    void LateUpdate();
}