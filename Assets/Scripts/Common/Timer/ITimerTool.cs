public interface ITimerObject
{
    void Tick();
}

public interface ITimerTool
{
    TimerEvent AddOnce(int delay, ITimerObject timer);
    TimerEvent AddMulti(int delay, int count, int interval, ITimerObject timer);
    TimerEvent AddEndLess(int delay, int interval, ITimerObject timer);

    void Destroy();
}
