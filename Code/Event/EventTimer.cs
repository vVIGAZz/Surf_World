using Sandbox;
public interface EventTimer : ISceneEvent<EventTimer>
{
	public void StartTimer();
	public void StopTimer();
}
