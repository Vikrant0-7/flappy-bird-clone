using Godot;
using System;

public class Bird : KinematicBody2D
{
    //VARIABLES SUPPLIED BY EXTERNAL SCRIPTS
    public float Speed;
    public float PipeSpeed;

    //INTERALLY CALCULATED AND USED VARIABLES
    Vector2 velocity;
    bool isDead = false;
    RigidBody2D body2D;

    public override void _Ready()
    {
        body2D = GetNode<RigidBody2D>("RigidBody2D");
        GetNode<CollisionPolygon2D>("%CollisionPolygon2D").Disabled = true;
    }

    public override void _PhysicsProcess(float delta)
    {
        if(!isDead)
            velocity = ((Input.IsActionPressed("Jump")) ? Vector2.Up : Vector2.Down) * Speed * Global.blockSize;
        else
            SetPhysicsProcess(false);
        velocity = MoveAndSlide(velocity,Vector2.Up);
        
    }

    //Performing steps once player has collided with pipe
    void _OnAreaEntered(Area2D area){
        if(area.IsInGroup("Pipes") || area.IsInGroup("WorldBoundary")){            
            isDead = true;
            velocity = Vector2.Right * PipeSpeed * Global.blockSize;
            GetNode<CollisionPolygon2D>("%CollisionPolygon2D").SetDeferred("disabled",false);
            body2D.SetDeferred("mode",RigidBody2D.ModeEnum.Rigid);
            body2D.SetDeferred("sleeping",false);
            body2D.SetDeferred("linear_velocity",velocity);
            bool hs = GetNode<Settings>("/root/Settings").SetHighScore(Global.Score);
            Global.EmitSignal(Signals.BirdDead,hs);
            
        }
        else if(area.IsInGroup("Score")){
            Global.Score++;
        }
    }
}
