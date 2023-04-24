using Godot;
using System;

public static class Global
{
    public static readonly int blockSize = 52;
    public static Vector2 size = new Vector2(1200,600);
    public static float pipeSeperation = 2;

    public static Vector2 numberOfBlocks{
        get{
            return size/blockSize;
        }
    }

    public static float bounds{
        get{
            return size.y * blockSize;
        }
    }

    private static int score;

    public static int Score{
        set{
            score = value;
            EmitSignal(Signals.ScoreUpdate);
        }
        get{
            return score;
        }
    }

    public delegate void PauseEvent(bool isPaused);
    public static PauseEvent Paused;

    public delegate void BirdDeadEvent(bool isHighscore);
    public static BirdDeadEvent BirdDead;

    public delegate void ScoreUpdateEvent(int _score);
    public static ScoreUpdateEvent ScoreUpdate;


    public static void EmitSignal(String signal,params object[] args){
        if(signal == Signals.BirdDead){
            BirdDead?.Invoke((bool)args[0]);
        }
        else if(signal == Signals.ScoreUpdate){
            ScoreUpdate?.Invoke(Score);
        }
        else if(signal == Signals.Paused){
            Paused?.Invoke((bool)args[0]);
            
        }
    }
}

public static class Signals{
    public static readonly String BirdDead = "BirdDead";
    public static readonly String ScoreUpdate = "ScoreUpdate";
    public static readonly String Paused = "Paused";
}

public static class GameMode{
    public enum HardnessLevel{
        easy = 0,
        medium = 1,
        hard = 2
    }
    public enum WorldGeneration{
        triangular = 0,
        sine = 1,
        classic = 2
    }
    public static HardnessLevel hardnessLevel = HardnessLevel.medium;
    public static WorldGeneration worldGeneration = WorldGeneration.classic;

}