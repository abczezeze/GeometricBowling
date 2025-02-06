using Godot;

public partial class MainMenu : Node
{
	private Label _bowlingScene;
	private Button _sceneA;
	private Button _sceneSec;
	private Button _sceneThird;
	private Button _sceneFourth;
	public override void _Ready()
	{
		_bowlingScene = GetNode<Label>("SceneVBox/BowlingScene");
		_sceneA = GetNode<Button>("SceneVBox/Bowling");
		_sceneA.Pressed += OnSceneBowling;
		_sceneSec = GetNode<Button>("SceneVBox/BowlingSec");
		_sceneSec.Pressed += OnSceneSec;
		_sceneThird = GetNode<Button>("SceneVBox/BowlingThird");
		_sceneThird.Pressed += OnSceneThird;
		_sceneFourth = GetNode<Button>("SceneVBox/Exit");
		_sceneFourth.Pressed += OnSceneFourth;
	}

	public override void _Process(double delta)
	{
		_bowlingScene.Text = Tr("BOWLINGSCENE");
		_sceneA.Text = Tr("FIRST");
		_sceneSec.Text = Tr("SEC");
		_sceneThird.Text = Tr("THIRD");
		_sceneFourth.Text = Tr("EXIT");
	}
	public void OnSceneBowling()
	{
		GetTree().ChangeSceneToFile("res://Scene_Script/Bowling.tscn");
		Global.SceneEnter = true;
	}
	public void OnSceneSec()
	{
		GetTree().ChangeSceneToFile("res://Scene_Script/BowlingSec.tscn");
		Global.SceneEnter = true;
	}
	public void OnSceneThird()
	{
		GetTree().ChangeSceneToFile("res://Scene_Script/BowlingThird.tscn");
		Global.SceneEnter = true;
	}
	public void OnSceneFourth()
	{
		GetTree().Quit(); 
	}	
}
