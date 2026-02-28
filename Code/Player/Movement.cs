using Sandbox;
using Sandbox.Citizen;

public sealed class Movement : Component
{
	[Property] private GameObject spawn;
	//SFX
	[Property] private SoundBoxComponent sfx;
	//Component
	public CharacterController controller;
	[Property] public GameObject head;
	[Property] public GameObject body;
	//Gravity
	private Vector3 gravity = new Vector3( 0, 0, 800 );
	private float jumpForce = 268f;
	//Movement property
	public Vector3 WishVelocity;
	[Sync] public Angles EyeAngle { get; set;}
	//Speed
	private float speed = 420f;
	protected override void OnStart()
	{
		controller = GetComponent<CharacterController>();
		controller.Acceleration = 60f;
		controller.GroundAngle = 45;
		controller.StepHeight = 0;
	}
	protected override void OnUpdate()
	{
		Angles look = EyeAngle;
		look += Input.AnalogLook;
		look.roll = 0;
		look.pitch = look.pitch.Clamp( -89f, 89f );
		EyeAngle = look;
		Rotation lookDir = EyeAngle.ToRotation();
		head.WorldRotation = lookDir;
		WorldRotation = Rotation.FromYaw( EyeAngle.yaw );
		body.WorldRotation = Rotation.FromYaw( EyeAngle.yaw );
		CheckGround();

	}
	protected override void OnFixedUpdate()
	{
		BuildWishVelocity();
		if ( controller.IsOnGround && Input.Pressed( "jump" ) )
		{
			controller.Punch( Vector3.Up * jumpForce );
		}
		if ( controller.IsOnGround )
		{
			sfx.Enabled = true;
			controller.Velocity = controller.Velocity.WithZ( 0 );
			controller.Accelerate( WishVelocity );
			controller.ApplyFriction( 4f );
		}
		else
		{
			sfx.Enabled = false;
			controller.Velocity -= gravity * Time.Delta;
			controller.Accelerate( WishVelocity.ClampLength( 150 ) );
			//controller.ApplyFriction( 0f );
		}
		controller.Move();
	}
	private void BuildWishVelocity()
	{
		var camPos = head.WorldRotation.Angles();
		WishVelocity = 0;
		Rotation lookDir = camPos.ToRotation();
		WishVelocity = Input.AnalogMove * lookDir;
		WishVelocity = WishVelocity.WithZ( 0 );
		if ( !WishVelocity.IsNearZeroLength ) WishVelocity = WishVelocity.Normal;
		WishVelocity *= speed;
	}
	private void CheckGround()
	{
		SceneTraceResult tr = Scene.Trace.Sphere(32f, WorldPosition, WorldPosition + Vector3.Down * 2 ).WithTag("terrain").Run();
		if ( tr.Hit )
		{
			WorldPosition = spawn.WorldPosition;
			EventTimer.Post( x => x.StopTimer() );
		}
	}
}
