using Godot;
using System;

public partial class Player : CharacterBody3D
{
    // How fast the player moves in meters per second.
    [Export] public int Speed { get; set; } = 14;
    // The downward acceleration when in the air, in meters per second squared.
    [Export] public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        // We create a local variable to store the input direction.
        var direction = Vector3.Zero;

        // We check for each move input and update the direction accordingly.
        // Notice how we are working with the vector's X and Z axes.
        // In 3D, the XZ plane is the ground plane.
        direction.X += Input.IsActionPressed("move_right") ? 1.0f : 0f;
        direction.X -= Input.IsActionPressed("move_left") ? 1.0f : 0f;
        direction.Z += Input.IsActionPressed("move_back") ? 1.0f : 0f;
        direction.Z -= Input.IsActionPressed("move_forward") ? 1.0f : 0f;

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").LookAt(Position + direction, Vector3.Up);
        }

        // Ground velocity
        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        // Vertical velocity
        if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        // Moving the character
        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
