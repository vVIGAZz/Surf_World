using Sandbox;

public sealed class Start : Component, Component.ITriggerListener
{
	public void OnTriggerEnter(Collider other )
	{
		if (other.GetComponent<Player>() != null )
		{
			EventManager.StopRun( other.GetComponent<Player>() );
		}
		
	}
	public void OnTriggerExit(Collider other )
	{
		if ( other.GetComponent<Player>() != null )
		{
			EventManager.StartRun( other.GetComponent<Player>() );
		}
	}
}
