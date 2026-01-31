using Sandbox;

public interface FinishEvent : ISceneEvent<FinishEvent>
{
	public void IsFinished(bool ISFinished);
}
