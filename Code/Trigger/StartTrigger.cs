using Sandbox;

public sealed class StartTrigger : Component, Component.ITriggerListener
{
	public void OnTriggerEnter(Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			EventTimer.Post( x => x.StopTimer() );
		}
	}
	public void OnTriggerExit(Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			EventTimer.Post( x => x.StartTimer() );
		}
	}
}
