using Godot;

public partial class BowlingSec : Node3D
{	
	//PackedScene _ballScene = GD.Load<PackedScene>("res://Scene_Script/BallRigid.tscn");
	RigidBody3D _ballRigid;
	Area3D _checkY;
	//MeshInstance3D _arrowMesh;
	MeshInstance3D _starMesh;
	Camera3D _camera3D;
	//Button _shootButton;
	//Area3D _fakeBall;
	RandomNumberGenerator _rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		_ballRigid = GetNode<RigidBody3D>("BallRigidA");
		_camera3D = GetNode<Camera3D>("Camera3D");
		Global.Camera3D = _camera3D;
		_checkY = GetNode<Area3D>("CheckY");
		_checkY.BodyEntered += OnCheckYBodyEntered;
		_starMesh = GetNode<MeshInstance3D>("Moon");
		
	}
	public override void _Process(double delta)
	{
		float rotate=0f;
		_starMesh.RotateX(rotate+=0.01f);
		_starMesh.RotateY(rotate+=0.01f);

	}

	public void NextRoll()
	{
		if (BallRigid._ballMesh.GetActiveMaterial(0) is StandardMaterial3D ballMat)
		{
			ballMat.AlbedoColor = new Color(_rng.RandfRange(0.2f,1), _rng.RandfRange(0.2f,1), _rng.RandfRange(0.2f,1));
		}
	}

	public void OnCheckYBodyEntered(Node3D body)
	{
		if(body.IsInGroup("BallGroup"))
		{
			NextRoll();   
			_ballRigid.Position = new Vector3(0,1,0);
			_ballRigid.Sleeping = true;
			Global.IfBallGrop();

		}
		if(body.IsInGroup("PinGroup"))
		{
			Global.ScoreCurrent+=1;
		}
		if(body.IsInGroup("SweepPin"))
		{
			Global.SweepPinRigid.QueueFree();
		}
	}
	// public void _OnFakeBallInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shape_idx)
	// {
	// 	if (inputEvent is InputEventMouseButton btn && btn.ButtonIndex == MouseButton.Left && inputEvent.IsPressed())
	// 	{
			
	// 		Global.JustAMomentLb.Visible = true;
	// 		Tween _tween = GetTree().CreateTween();
	// 		_tween.TweenProperty(Global.JustAMomentLb,"scale",new Vector2(3.0f,3.0f),0.5f).SetTrans(Tween.TransitionType.Back);
	// 		_tween.TweenProperty(Global.JustAMomentLb,"scale",Vector2.One,0.5f).SetTrans(Tween.TransitionType.Back);
			
	// 		_ballRigid = _ballScene.Instantiate<RigidBody3D>();
	// 		_ballRigid.Position = new Vector3(Global.VectorX,1,0);
	// 		_ballRigid.AddToGroup("BallGroup");
	// 		AddChild(_ballRigid);
	// 		_ballRigid.ApplyImpulse(new Vector3(0, 0, Global.PowerValue*-1));
    //     }
	// }
}
