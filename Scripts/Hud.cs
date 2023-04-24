using Godot;
using System;

public class Hud : CanvasLayer
{
    String[] hardness = {"Easy","Normal","Hard"};
    String[] generation = {"Triangular","Sine","Classic"};

    bool paused = false;

    public override void _Input(InputEvent @event)
    {
        
        if(@event.IsClass("InputEventKey")){
            if(Input.IsKeyPressed((int)KeyList.Escape)){
                paused = !paused;
                if(!paused)
                    _on_Pause();
                else
                    _on_Resume();
                
            }
            
        }
    }

    public override void _Ready()
    {
        GetNode<Control>("%GameHUD").Show();
        GetNode<Control>("DieMenu").Hide();
        GetNode<Control>("%PauseMenu").Hide();
        Global.ScoreUpdate += ScoreUpdate;
        Global.Paused += _OnPause;
        Global.BirdDead += BirdDied;
        GetNode<Label>("%ScoreNumber").Text = "0";
        GetNode<Label>("%Gamemode").Text = hardness[(int)GameMode.hardnessLevel] + ": " + generation[(int)GameMode.worldGeneration];
    }

    void ScoreUpdate(int score){
        GetNode<Label>("%ScoreNumber").Text = score.ToString();
    }

    public override void _ExitTree()
    {
        Global.ScoreUpdate -= ScoreUpdate;
        base._ExitTree();
    }

    void _on_Pause(){
        Global.EmitSignal(Signals.Paused,true);
    }

    void _on_Resume(){
        Global.EmitSignal(Signals.Paused,false);
    }

    void _on_Restart_pressed(){
        GetTree().ReloadCurrentScene();

    }

    void _on_Main_Menu(){
        GetTree().Paused = false;
        GetTree().ChangeScene("res://MainMenu/MainMenu.tscn");
    }

    void _OnPause(bool isPaused){
        if(isPaused){
            GetTree().Paused = true;
            GetNode<Control>("%GameHUD").Hide();
            GetNode<Control>("DieMenu").Hide();
            GetNode<Control>("%PauseMenu").Show();
        }
        else{
            GetTree().Paused = false;
            GetNode<Control>("%GameHUD").Show();
            GetNode<Control>("%PauseMenu").Hide();
            GetNode<Control>("DieMenu").Hide();
        }
    }

    void BirdDied(bool hs){
        GetNode<Label>("DieMenu/ScoreNumberDie").Text = Global.Score.ToString();
        GetNode<Control>("DieMenu").Show();
        GetNode<Label>("%HS").Visible = hs;
        GetNode<Control>("%PauseMenu").Hide();
        GetNode<Control>("%GameHUD").Hide();
    }

    protected override void Dispose(bool disposing)
    {
        Global.ScoreUpdate -= ScoreUpdate;
        Global.Paused -= _OnPause;
        Global.BirdDead -= BirdDied;
        base.Dispose(disposing);
    }

}
