using Sandbox;

public interface SetTime : ISceneEvent<SetTime>
{
	public void SetTime( double time );
}
