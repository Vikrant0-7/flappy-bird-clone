using Godot;
using System;

public class Settings : Node
{
    int[] easy;
    int[] normal;
    int[] hard;

    public override void _Ready()
    {
        SaveData data = SaveSystem.Load();
        if(data.IsNull()){
            easy = new int[] {0,0,0};
            normal = new int[] {0,0,0};
            hard = new int[] {0,0,0};
        }
        else{
            easy = data.highscore_easy;
            normal = data.highscore_normal;
            hard = data.highscore_hard;
            GameMode.hardnessLevel = (GameMode.HardnessLevel)data.hardnessLevel;
            GameMode.worldGeneration = (GameMode.WorldGeneration)data.worldGenerator;
        }
    }

    public bool SetHighScore(int score){
        if(GameMode.hardnessLevel == GameMode.HardnessLevel.easy){
            int currentScore = easy[(int)GameMode.worldGeneration];
            easy[(int)GameMode.worldGeneration] = Mathf.Max(currentScore, score);
            return score > currentScore;
        }
        if(GameMode.hardnessLevel == GameMode.HardnessLevel.medium){
            int currentScore = normal[(int)GameMode.worldGeneration];
            normal[(int)GameMode.worldGeneration] = Mathf.Max(currentScore, score);
            return score > currentScore;
        }
        if(GameMode.hardnessLevel == GameMode.HardnessLevel.hard){
            int currentScore = hard[(int)GameMode.worldGeneration];
            hard[(int)GameMode.worldGeneration] = Mathf.Max(currentScore, score);
            return score > currentScore;
        }
        return false;
    }

    public void GetHighscores(out int[] easy, out int[] normal, out int[] hard) {
        easy = this.easy;
        normal = this.normal;
        hard = this.hard;
    }

    public override void _Notification(int what)
    {
        if(what == NotificationWmQuitRequest){
            SaveData saveData = new SaveData((int)GameMode.worldGeneration,(int)GameMode.hardnessLevel, easy, normal, hard);
            SaveSystem.Save(saveData);
            GetTree().Quit();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsClass("InputEventKey")){
            if(Input.IsActionJustPressed("fullscreen")){
                OS.WindowFullscreen = !OS.WindowFullscreen;
            }
        }
    }
}
