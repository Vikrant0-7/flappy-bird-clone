using Godot;
using System;

public class bg : ParallaxBackground
{

    float pipeSpeed = 0;


    public void Init(float pipeSpeed){
        this.pipeSpeed = pipeSpeed * Global.blockSize;
        Global.BirdDead += StopMotion;
        Global.Paused += Pause;
    }

    public override void _Process(float delta){
        ScrollBaseOffset += Vector2.Left * pipeSpeed/4 * delta;
    }

    void Stop(bool stop = false){
        SetProcess(stop);
    }
    void StopMotion(bool a){
        Stop();
    }
    void Pause(bool isPaused){
        Stop(isPaused);
    }

        protected override void Dispose(bool disposing)
    {
        Global.BirdDead -= StopMotion;
        Global.Paused -= StopMotion;
    }

    //disconnects signals once this scene has exited tree
    public override void _ExitTree()
    {
        Global.BirdDead -= StopMotion;
        Global.Paused -= StopMotion;
        //timer.Disconnect("timeout",this,"_on_Timer_timeout");
        base._ExitTree();
    }
}
