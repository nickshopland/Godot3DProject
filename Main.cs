using Godot;
using System;

public partial class Main : Node
{
    [Export] public PackedScene MobScene { get; set; }

    private Player _player;
    private PathFollow3D _mobSpawnLocation;
    private Timer _mobTimer;
    private Control _ui;

    public override void _Ready()
    {
        base._Ready();

        _player = GetNode<Player>("Player");
        _mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
        _mobTimer = GetNode<Timer>("MobTimer");
        _ui = GetNode<Control>("UserInterface/Retry");
        _ui.Hide();
    }

    private void OnMobTimerTimeout()
    {
        _mobSpawnLocation.ProgressRatio = GD.Randf();

        Mob mob = MobScene.Instantiate<Mob>();
        mob.Initialize(_mobSpawnLocation.Position, _player.Position);
        AddChild(mob);

        mob.Squashed += GetNode<ScoreLabel>("UserInterface/ScoreLabel").OnMobSquashed;
    }
    
    private void OnPlayerHit()
    {
        _mobTimer.Stop();
        _ui.Show();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Retry").Visible)
        {
            // This restarts the current scene.
            GetTree().ReloadCurrentScene();
        }
    }
}
