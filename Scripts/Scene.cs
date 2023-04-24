using Godot;
using System;

public class Scene : Node2D
{
    PipeGenerator pipeGenerator;
    Bird bird;

    [Export]
    Hardness settings;

    bool isStarted = false;

    String sPath = "res://Settings/";
    String[] hNess = {"easy","normal","hard"};
    String[] gMode = {"_triangular.tres","_sine.tres",".tres"};

    public override void _Ready()
    {
        if(settings == null){
            settings = GD.Load<Hardness>(sPath+hNess[(int)GameMode.hardnessLevel]+gMode[(int)GameMode.worldGeneration]);
        }
        Global.Score = 0;
        Global.size = GetViewport().Size;
        pipeGenerator = GetNodeOrNull<PipeGenerator>("PipeGenerator");
        bird = GetNodeOrNull<Bird>("Bird");

        if(pipeGenerator == null || bird == null)
            GD.PrintErr("Scene Must Contian Bird Node and PipeGenerator Node");
        
        pipeGenerator.settings = settings;
        bird.Speed = settings.birdSpeed;
        bird.PipeSpeed = settings.pipeSpeed;
        bird.SetPhysicsProcess(false);
    }

    public override void _Input(InputEvent @event)
    {
        if(!isStarted &&(@event.IsClass("InputEventKey") || @event.IsClass("InputEventMouseButton")))
        {
            Global.size = GetViewport().Size;
            bird.SetPhysicsProcess(true);
            pipeGenerator.Ready();
            isStarted = true;
        }
    }

}
