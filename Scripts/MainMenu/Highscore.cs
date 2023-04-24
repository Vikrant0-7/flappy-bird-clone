using Godot;
using System;

public class Highscore : VBoxContainer
{
    int[] easy;
    int[] normal;
    int[] hard;

    public void SetHighScore(int[] easy, int[] normal, int[] hard){
        this.easy = easy;
        this.normal = normal;
        this.hard = hard;
    }

    public override void _Ready()
    {
        GetNode<Settings>("/root/Settings").GetHighscores(out easy, out normal, out hard);
        
        GetNode<Label>("%ET").Text = easy[0].ToString();
        GetNode<Label>("%ES").Text = easy[1].ToString();
        GetNode<Label>("%EC").Text = easy[2].ToString();

        GetNode<Label>("%NT").Text = normal[0].ToString();
        GetNode<Label>("%NS").Text = normal[1].ToString();
        GetNode<Label>("%NC").Text = normal[2].ToString();

        GetNode<Label>("%HT").Text = hard[0].ToString();
        GetNode<Label>("%HS").Text = hard[1].ToString();
        GetNode<Label>("%HC").Text = hard[2].ToString();
    }
}
