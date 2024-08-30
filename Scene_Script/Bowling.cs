using Godot;
using System;

public partial class Bowling : Node3D
{	
	private RigidBody3D _BallRigid;
	private RigidBody3D _PinRigid1;
	private HScrollBar _PowerBar;
	private Label _LabelPower;
	private double _PowerValue;
	private Button _LeftButton;
	private Button _RightButton;
	private AnimationPlayer _firstAnim;
	
	public override void _Ready()
	{
		_BallRigid = GetNode<RigidBody3D>("BallRigid");
		_PinRigid1 = GetNode<RigidBody3D>("PinRigid1");
		for(int i=-2;i<=2;i++)
		{
			for(int j=-15;j<=-13;j++)
			{
			AddChild(_PinRigid1.Duplicate());
			Vector3 vector3 = new Vector3(i, 2, j);
			_PinRigid1.Position = vector3;
			}
		}
		_PowerBar = GetNode<HScrollBar>("VBoxContainer/PowerBar");
		_LabelPower = GetNode<Label>("VBoxContainer/PowerLabel");
		_LeftButton = GetNode<Button>("LeftButton");
		_LeftButton.Pressed += LeftImulse;
		_RightButton = GetNode<Button>("RightButton");
		_RightButton.Pressed += RightImulse;
		_firstAnim = GetNode<AnimationPlayer>("AnimationPlayer");
		_firstAnim.Play("first");

	}
	public override void _Process(double delta)
	{
		_PowerValue = _PowerBar.Value; 
		_LabelPower.SetText("Power Value : " + _PowerValue.ToString("F2"));
	}
	public void _OnBallRigidInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shape_idx)
	{
		if (inputEvent is InputEventMouseButton btn && btn.ButtonIndex == MouseButton.Left && inputEvent.IsPressed())
		{
			_BallRigid.ApplyImpulse(new Vector3(0, 0, (float)(_PowerValue*-1)));
        }
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Keycode == Key.Right)
			{
				GetViewport().WarpMouse(new Vector2(1075,700));
			}
			if (eventKey.Pressed && eventKey.Keycode == Key.Left)
			{
				GetViewport().WarpMouse(new Vector2(20,700));
			}
			if (eventKey.Pressed && eventKey.Keycode == Key.Space)
			{
				GetTree().ReloadCurrentScene();
			}
		}
	}
	public void LeftImulse()
	{
		_BallRigid.ApplyImpulse(new Vector3((float)-0.1, 0,0));
	}
	public void RightImulse()
	{
		_BallRigid.ApplyImpulse(new Vector3((float)0.1, 0,0));
	}
	public void _onArea3dBodyEntered(Node3D body)
	{
		if (body.IsInGroup("BallRigid"))
		{
			_BallRigid.Position = new Vector3(0,1,0);
			_BallRigid.ApplyImpulse(new Vector3(0, 0, 1));
		}
	}
}
