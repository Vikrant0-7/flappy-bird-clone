using Godot;
using System;

public class Pipe : KinematicBody2D
{
    //CONSTANTS 
    const float textureSize = 52; //Size of pipe texture in pixels. Use full for extending pipe till end of screen
    PackedScene[] pipes = {(PackedScene)GD.Load("res://Pipe/PipeBottom.scn"),(PackedScene)GD.Load("res://Pipe/PipeUpper.scn")};

    //VARIABLES SUPPLIED BY EXTERNAL SCRIPTS
    public Vector2 velocity;  //Velocity at which pipe moves. Also the velocity of bird once dead!
    
    Vector2 tempPosition; //Temperarily store position of pipe, untill it is added to tree
    float gap; //Seperation in two pipes in pixels

    PipeGenerator father;
    
    //INTERNALLY SUPPLIED VARIABLES
    Area2D pipe1, pipe2;

 

    public override void _Ready()
    {
        Global.BirdDead += _OnBirdDead; //Connects BirdDead signals to class's method

        GlobalPosition = tempPosition; //asigns position, once scene has entered in tree
        
        //Assigning Interal variables. Used for calculations required to extend Pipe to match height of screen
        pipe1 = GetNode<Area2D>("Up");
        pipe2 = GetNode<Area2D>("Down"); 
        Generate(); //Generates and Performs required calculations
        //

        GetNode<VisibilityNotifier2D>("VisibilityNotifier2D").Connect("screen_exited",this,"_OnScreenExited"); //connecting signal
        //in order to delete pipe once not visible
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = MoveAndSlide(velocity); //Moves pipe with required velocity
        
        //if currently in main menu
        //then remove itself from pipes list 
        //once this pipe has passed bird
        if(father.settings.isMainMenu){
            if(GlobalPosition.x < 250){
                Remove();
            }
        }
    }

    //Initializes Pipe. To be called from external scripts once scene is instanced
    public void Init(Vector2 pos, float speed, float dist, PipeGenerator father){

        this.father = father;
        //Resizes Area, that monitors if player has crossed pipe, to match pipe seperation
        (GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Shape as RectangleShape2D).Extents = new Vector2(2,dist * Global.blockSize); 
        tempPosition = pos; //Temeperarily stores position. Untill scene has added to tree
        gap = dist/2 * Global.blockSize; //converts pipe seperation from blocks to pixels;
        velocity = Vector2.Left * speed * Global.blockSize; //converts speed in blocks/sec to a vector with speed in pixel per second

    }


    public void Generate()
    {
        pipe1.Position = Vector2.Up * gap;
        pipe2.Position = Vector2.Down * gap;

        //Exdends lower pipe's texture to lower end of screen
        Vector2 addPos = pipe2.Position + Vector2.Down * textureSize;
        while(addPos.y <= Global.size.y + textureSize - GlobalPosition.y){
            Area2D tempPipe = pipes[1].Instance<Area2D>(); //creates instance of texture
            tempPipe.Scale = new Vector2(1,-1); //flips it in y direction
            tempPipe.Position = addPos; //asigns position
            addPos += textureSize * Vector2.Down; //adds on texture block to current position and uses it for next pipe
            CallDeferred("add_child",tempPipe);
        }

        //Extends upper pipe's texture to upper end of screen
        addPos = pipe1.Position + Vector2.Up * textureSize;
        while(addPos.y >= -textureSize - GlobalPosition.y){
            Area2D tempPipe = pipes[1].Instance<Area2D>();
            tempPipe.Position = addPos;
            addPos  += textureSize * Vector2.Up;
            CallDeferred("add_child",tempPipe);
        }
    }

    //Remove this pipe
    public void Remove(){
        if(father.settings.isMainMenu && father.Pipes.Contains(this)){
            father.Pipes.Remove(this);
        }
    }

    //delete scene once not visible on the screen
    public void _OnScreenExited(){
        Remove();
        QueueFree();
    }

    //Stop motion of pipe once bird is dead
    void _OnBirdDead(bool hs){
        velocity = Vector2.Zero;
    }

    //disconnect signals when deleting this scene from scene tree
    protected override void Dispose(bool disposing)
    {
        Global.BirdDead -= _OnBirdDead;
        base.Dispose(disposing);
    }
}
