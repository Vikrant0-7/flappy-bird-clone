using Godot;
using System.Collections.Generic;

public class PipeGenerator : Node2D
{
    //VARIABLES SUPPLIED BY EXTERNAL SCRIPTS
    public Hardness settings;

    //CONSTANTS
    PackedScene pipeScene = GD.Load<PackedScene>("res://Pipe/Pipe.tscn");
    
    //INTERALLY CALCULATED VARIABLES
    Timer timer;
    int pipeIdx = 0;
    float time = 0;
    float prev_position = -1;

    bool isPaused = false;

    public List<Pipe> Pipes;

    #region Properties
    float padding{
        get{
            return settings.gap/2+.4f;
        }
    } //Minimum Gap between pipe and upper and lower end of screen
    float spawnPoint{
        get{
            return (Global.numberOfBlocks.x + 2) * Global.blockSize;
        }
    } //X axis value for spawn point of pipes
    #endregion

    public override void _PhysicsProcess(float delta)
    {
        time += delta * 10;
        if(time > 180){
            time -= 180;
        }
    }

    //To be called by external scripts to initialze pipe generator
    public void Ready()
    {
        //Maintain record of pipes spawned if it is main menu
        if(settings.isMainMenu)
            Pipes = new List<Pipe>();
        
        Global.BirdDead += _OnBirdDead; //Connects BirdDead signal to class's method
        Global.Paused += OnPause;

        timer = GetNode<Timer>("Timer"); //Gets timer node, which is child 
        timer.WaitTime = settings.spawnTime; //asigns time between two spawns
        
        if (!timer.IsConnected("timeout",this,"_on_Timer_timeout")) //connects timeout signal if already not connected
            timer.Connect("timeout",this,"_on_Timer_timeout");
        GetNode<bg>("%PBG").Init(settings.pipeSpeed);
        //sets speed of background scrolling
        SpawnPipe();
        timer.Start();
    }
    
    //Spawn Pipe scene
    void SpawnPipe(){
        if(isPaused)
            return;
        //creates instance of pipe scene
        Pipe p = pipeScene.Instance<Pipe>();
        Vector2 pos = Vector2.Zero;

        //Generation of position governed by different sets of rule
        //Default is RandomGenerator
        if(settings.generator == GeneratorType.Trinagular){
            pos = Generator.TriangularGenerator(padding,pipeIdx,spawnPoint,out pipeIdx);
        }
        else if (settings.generator == GeneratorType.ModeratedRandom){
            pos = Generator.ModeratedRandom(padding,spawnPoint,settings,prev_position);
            prev_position = pos.y/Global.blockSize;
        }
        else if (settings.generator == GeneratorType.Sine){
            pos = Generator.SineGenerator(padding,spawnPoint,time);
        }
        else /*if (settings.generator == GeneratorType.Random)*/{
            pos = Generator.RandomGenerator(padding,spawnPoint);
        }

        //Add spawned pipe to record list if currently running in main menu
        if(settings.isMainMenu)
            Pipes.Add(p);
        
        //Supplies position, speed and seperation to pipe
        p.Init(pos, settings.pipeSpeed, settings.gap,this);
        CallDeferred("add_child",p); //add instanced pipe as child

        pipeIdx++; //increase number of pipe spawned by 1. Manipulates pipe generators
    }

    //Generates new pipe after a specified amount of time
    void _on_Timer_timeout(){
        SpawnPipe();
        timer.Start();
    }

    //actions to be performed once bird is dead
    void _OnBirdDead(bool hs){
        timer.Stop(); //Stops Generation of new pipes
    }

    protected override void Dispose(bool disposing)
    {
        Global.BirdDead -= _OnBirdDead;
        Global.Paused -= OnPause;
    }

    //disconnects signals once this scene has exited tree
    public override void _ExitTree()
    {
        Global.BirdDead -= _OnBirdDead;
        Global.Paused -= OnPause;
        //timer.Disconnect("timeout",this,"_on_Timer_timeout");
        base._ExitTree();
    }

    void OnPause(bool isPaused){
        this.isPaused = isPaused;
        if (!isPaused){
            timer.Start();
        }
    }


}
