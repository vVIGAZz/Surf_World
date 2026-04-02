using Sandbox;
using System.Xml.Linq;

public sealed class Finish : Component, Component.ITriggerListener
{
	[Property] public MapID MapName { get; set; }
	public void OnTriggerEnter(Collider other )
	{
		if (other.GetComponent<Player>() != null )
		{
			EventManager.FinishRun( other.GetComponent<Player>() );
		}
		if ( other.GetComponent<Movement>() != null )
		{
			EventManager.Respawn( other.GetComponent<Movement>());
		}
	}	
}
