using Godot;

public partial class SweepPin : RigidBody3D
{
	Area3D _getPin;
	public override void _Ready()
	{
		_getPin = GetNode<Area3D>("Area3D");
		_getPin.BodyEntered += OnBodyEntered;
		Global.SweepPinRigid = this;
		
		
	}
    private void OnBodyEntered(Node body)
    {
		if (body.IsInGroup("PinGroup"))
		{
        	body.RemoveFromGroup("PinGroup");
		}
    }
}
