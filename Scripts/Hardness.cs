using Godot;
using System;

public class Hardness : Resource
{
    [Export]
    public float pipeSpeed;
    [Export]
    public float gap;
    [Export]
    public float spawnTime;
    [Export]
    public float birdSpeed;

    [Export]
    public GeneratorType generator;

    public bool isMainMenu = false;

    public override string ToString()
    {
        String ret = "Pipe Speed: " + pipeSpeed;
        ret += "\nGap Between Pipes: " + gap;
        ret += "\nSpawn Time: " + spawnTime;
        ret += "\nBirdSpeed: " + birdSpeed;
        ret += "\nGenerator: ";
        if(generator == GeneratorType.Trinagular)
            ret += "Triangular";
        if(generator == GeneratorType.Sine)
            ret += "Sine";
        if(generator == GeneratorType.ModeratedRandom)
            ret += "SModeratedRandom";
        if(generator == GeneratorType.Random)
            ret += "Random";

        return ret;
    }

    public Hardness(){

    }

    public Hardness(Hardness h){
        pipeSpeed = h.pipeSpeed;
        gap = h.gap;
        spawnTime = h.spawnTime;
        birdSpeed = h.birdSpeed;
        generator = h.generator;
        isMainMenu = h.isMainMenu;
    }

    public Hardness(float pipeSpeed, float gap, float spawnTime, float birdSpeed, GeneratorType generator, bool isMainMenu){
        this.pipeSpeed = pipeSpeed;
        this.gap = gap;
        this.spawnTime = spawnTime;
        this.birdSpeed = birdSpeed;
        this.generator = generator;
        this.isMainMenu = isMainMenu;
    }
}

public enum GeneratorType{
        Trinagular,
        Sine,
        ModeratedRandom,
        Random
}

