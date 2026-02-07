using Sandbox;

public sealed class CameraMovement : Component
{
	[Property] private Movement movement;
	[Property] private CameraComponent cam;
	public Angles EyeAngle { get; set; }

	protected override void OnStart()
	{

		cam = GetComponent<CameraComponent>();
	}
	protected override void OnUpdate()
	{
		if (movement != null )
		{
			movement = Scene.Get<Movement>();
			if (movement == null )
			{
				return;
			}
		}
		WorldPosition = movement.head.WorldPosition;
		Angles look = EyeAngle;
		look += Input.AnalogLook;
		look.roll = 0;
		look.pitch = look.pitch.Clamp( -89f, 89f );
		EyeAngle = look;
		Rotation lookDir = EyeAngle.ToRotation();
		WorldRotation = lookDir;
		movement.head.WorldRotation = lookDir;
		movement.body.WorldRotation = Rotation.FromYaw( EyeAngle.yaw );
	}
}
