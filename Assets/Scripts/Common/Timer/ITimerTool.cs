public delegate void TimerCallback();

public interface ITimerObject
{
    void Tick();
}

public interface ITimerTool
{
    TimerEvent AddOnce(int delay, TimerCallback timer);
    TimerEvent AddMulti(int delay, int count, int interval, TimerCallback timer);
    TimerEvent AddEndLess(int delay, int interval, ITimerObject timer);

    void Update();

    void Destroy();
}
