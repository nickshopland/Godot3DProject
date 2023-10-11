using Godot;
using System;

public partial class Player : CharacterBody3D
{
    // Emitted when the player was hit by a mob.
    [Signal] public delegate void HitEventHandler();
    
    [Export] public int Speed { get; set; } = 14;
    [Export] public int FallAcceleration { get; set; } = 75;
    [Export] public int JumpImpulse { get; set; } = 20;
    [Export] public int BounceImpulse { get; set; } = 16;

    private Vector3 _targetVelocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;

        direction.X += Input.IsActionPressed("move_right") ? 1.0f : 0f;
        direction.X -= Input.IsActionPressed("move_left") ? 1.0f : 0f;
        direction.Z += Input.IsActionPressed("move_back") ? 1.0f : 0f;
        direction.Z -= Input.IsActionPressed("move_forward") ? 1.0f : 0f;

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").LookAt(Position + direction, Vector3.Up);
        }

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _targetVelocity.Y = JumpImpulse;
        }

        for (int index = 0; index < GetSlideCollisionCount(); index++)
        {
            KinematicCollision3D collision = GetSlideCollision(index);

            if (collision.GetCollider() is Mob mob)
            {
                if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
                {
                    mob.Squash();
                    _targetVelocity.Y = BounceImpulse;
                }
            }
        }

        // Moving the character
        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    private void Die()
    {
        EmitSignal(SignalName.Hit);
        QueueFree();
    }

    private void OnMobDetectorBodyEntered(Node3D body)
    {
        GD.Print("DEAD");
        Die();
    }
}
