using Godot;
using System;

public class ModesMenu : VBoxContainer
{
    Button[] hardness;
    Button[] world;

    //Overrides for currenty selected world generator and hardness
    StyleBoxFlat flat; //normal
    StyleBoxFlat hover; //when hovering

    String[] Hness = {"Easy","Normal","Hard"};
    String[] generation = {"Triangular","Sine","Classic"};

    public override void _Ready()
    {
        //initializing and assigning colors to overrides
        flat = new StyleBoxFlat();
        hover = new StyleBoxFlat();
        flat.BgColor = new Color("24ff00d1");
        hover.BgColor = new Color("12ff00d1");

        hardness = new Button[3];
        world = new Button[3];
        int idx = 0;

        //Gets buttons related to hardness
        foreach(Button b in GetNode<HBoxContainer>("HardnessSelector").GetChildren()){
            hardness[idx] = b;
            idx++;
        }
        idx = 0;
        //Fetches buttons from tree related to World Generation 
        foreach(Button b in GetNode<VBoxContainer>("WorldGeneration").GetChildren()){
            world[idx] = b;
            idx++;
        }
        UpdateButtons();
    }

    //Changes gamemode to easy
    void _onEasy(){
        GameMode.hardnessLevel = GameMode.HardnessLevel.easy;
        UpdateButtons();
    }

    //Changes gamemode to normal
    void _onNormal(){
        GameMode.hardnessLevel = GameMode.HardnessLevel.medium;
        UpdateButtons();
    }

    //changes gamemode to hard
    void _onHard(){
        GameMode.hardnessLevel = GameMode.HardnessLevel.hard;
        UpdateButtons();
    }

    //changes world generation to triangular wave
    void _onT(){
        GameMode.worldGeneration = GameMode.WorldGeneration.triangular;
        UpdateButtons();
    }

    //changes world generation to sine wave
    void _onS(){
        GameMode.worldGeneration = GameMode.WorldGeneration.sine;
        UpdateButtons();
    }

    //changes world generation to classic 
    void _onC(){
        GameMode.worldGeneration = GameMode.WorldGeneration.classic;
        UpdateButtons();
    }

    //updates overrides for currently selected game modes
    void UpdateButtons(){
        //removes any override present
        for(int i = 0; i < 3; i++){
            hardness[i].RemoveStyleboxOverride("normal"); 
            world[i].RemoveStyleboxOverride("normal");

            hardness[i].RemoveStyleboxOverride("hover");
            world[i].RemoveStyleboxOverride("hover");
        }

        //changes overrides for currently selected game modes
        hardness[(int)GameMode.hardnessLevel].AddStyleboxOverride("normal",flat);
        world[(int)GameMode.worldGeneration].AddStyleboxOverride("normal",flat);

        hardness[(int)GameMode.hardnessLevel].AddStyleboxOverride("hover",hover);
        world[(int)GameMode.worldGeneration].AddStyleboxOverride("hover",hover);
        GetNode<Label>("%Gamemode").Text = Hness[(int)GameMode.hardnessLevel] + ": " + generation[(int)GameMode.worldGeneration];
    }


}
