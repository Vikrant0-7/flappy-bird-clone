using Godot;
using System;

public class MainMenu : Control
{
    PipeGenerator pipeGenerator; //Pipe generator for background play
    BirdAuto bird; //Automated bird for background play

    String[] hardness = { "Easy", "Normal", "Hard" };
    String[] generation = { "Triangular", "Sine", "Classic" };

    //return position of next pipe 
    //i.e. next pipe that is to be crossed by bird
    public Vector2 next
    {
        get
        {
            return pipeGenerator.Pipes[0].GlobalPosition;
        }
    }


    public override void _Ready()
    {
        GetNode<VBoxContainer>("ModesMenu").Hide();
        GetNode<VBoxContainer>("MainMenu").Show();
        GetNode<VBoxContainer>("Highscore").Hide();

        Global.Score = 0; //resets score to 0
        Global.size = GetViewport().Size; //recalculate size of world

        //fetches pipeGenerator and bird from tree
        pipeGenerator = GetNodeOrNull<PipeGenerator>("%PipeGenerator");
        bird = GetNodeOrNull<BirdAuto>("%BirdAuto");

        //Give error if any one of them is not found
        if (pipeGenerator == null || bird == null)
            GD.PrintErr("Scene Must Contian Bird Node and PipeGenerator Node");

        //getting hardness for pipes to spawn
        Hardness hard = new Hardness(GD.Load<Hardness>("res://Settings/easy.tres"));
        hard.isMainMenu = true; //telling pipes and pipe generator that they are currently in mainmenu
        //                      //pipe generator will record pipes, that it will spawn, and store them in list
        //                      //pipes will remove themselves from list of spawn pipes

        //initializing pipeGenerator and bird
        pipeGenerator.settings = hard;
        pipeGenerator.Ready();
        bird.Init(this, hard);
        GetNode<Label>("%Gamemode").Text = hardness[(int)GameMode.hardnessLevel] + ": " + generation[(int)GameMode.worldGeneration];

    }

    //Show Main Menu
    //Hide Modes Menu
    void _on_Back_Modes()
    {
        GetNode<VBoxContainer>("ModesMenu").Hide();
        GetNode<VBoxContainer>("MainMenu").Show();
        GetNode<VBoxContainer>("Highscore").Hide();
        GetNode<Label>("%Gamemode").Text = hardness[(int)GameMode.hardnessLevel] + ": " + generation[(int)GameMode.worldGeneration];

    }

    //Show Main Menu
    //Hide Highscore Menu
    void _on_Back_Highscore()
    {
        GetNode<VBoxContainer>("ModesMenu").Hide();
        GetNode<VBoxContainer>("MainMenu").Show();
        GetNode<VBoxContainer>("Highscore").Hide();
    }

    //Start the game
    void _onStart()
    {
        GetTree().ChangeScene("res://test.tscn");
    }

    //Hide Main Menu
    //Show Modes menu
    void _onModes()
    {
        GetNode<VBoxContainer>("ModesMenu").Show();
        GetNode<VBoxContainer>("MainMenu").Hide();
        GetNode<VBoxContainer>("Highscore").Hide();
    }

    //Hide Main Menu
    //Show Highscore's menu
    void _onHighscore()
    {
        GetNode<VBoxContainer>("MainMenu").Hide();
        GetNode<VBoxContainer>("Highscore").Show();
        GetNode<VBoxContainer>("ModesMenu").Hide();
    }

    //Quit application
    void _onQuit()
    {
        GetTree().Root.PropagateNotification(NotificationWmQuitRequest);
    }
}
