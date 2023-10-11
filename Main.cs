using Godot;
using System;

public partial class Main : Node
{
    [Export] public PackedScene MobScene { get; set; }

    private Player _player;
    private PathFollow3D _mobSpawnLocation;
    
    public override void _Ready()
    {
        _player = GetNode<Player>("Player");
        _mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
    }

    private void OnMobTimerTimeout()
    {
        _mobSpawnLocation.ProgressRatio = GD.Randf();
        
        Mob mob = MobScene.Instantiate<Mob>();
        mob.Initialize(_mobSpawnLocation.Position, _player.Position);
        AddChild(mob);
    }
    
    private void OnPlayerHit()
    {
        GetNode<Timer>("MobTimer").Stop();
    }
}
