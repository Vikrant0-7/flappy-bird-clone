using Godot;
using System;

//This is an automated player script]
//bird will fly endlessly without ever colliding once
public class BirdAuto : KinematicBody2D
{

    //EXTERNALLY SUPPLIED VARIABLES
    MainMenu father; //parent of bird
    //               used to fetch next incoming pipe
    float speed; //speed at which bird moves in vertical direction

    //INTERNALLY CALCULATED VARIABLES
    Vector2 velocity;
    

    public override void _Ready()
    {
        SetPhysicsProcess(false); //pauses execution of physics loop untill father and speed are assigned
        base._Ready();
    }

    //assings father and speed variables
    public void Init(MainMenu f, Hardness hard){
        father = f;
        speed = hard.birdSpeed * Global.blockSize;
        SetPhysicsProcess(true); //starts physics loop
    }

    //tell if current vertical position is in one block range of supplied vector
    bool isInRange(Vector2 what){
        return (Mathf.Abs(GlobalPosition.y - what.y + Global.blockSize) < Global.blockSize);
    }

    //tells if supplied vector is at least one block above current vertical position
    bool isUpper(Vector2 what){
        return GlobalPosition.y > what.y && !isInRange(what);
    }

    

    public override void _PhysicsProcess(float delta)
    {
        velocity = MoveAndSlide(velocity, Vector2.Up); //moves bird
    }

    //updates velocity after a fixed amount of time
    //to give more human like feelings
    void _onTimeout(){
        //if center of next pipe is at least one block above
        //move upwards
        if(isUpper(father.next)){
            velocity = Vector2.Up * speed;
        }
        //else move down
        else{
            velocity = Vector2.Down * speed;
        }
    }
}
