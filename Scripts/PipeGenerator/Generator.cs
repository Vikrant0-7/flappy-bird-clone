using Godot;
using System;

public static class Generator{
    //Generates upper and lower extremes as pipe position 
    public static Vector2 TriangularGenerator(float padding, int pipeIdx, float spawnPoint, out int idx){
        pipeIdx %= 2;
        float[] pipes = {padding,Global.numberOfBlocks.y-padding};
        idx = pipeIdx;
        return Vector2.Right * spawnPoint +  Vector2.Down * pipes[pipeIdx] * Global.blockSize;
    }

    //Generates Random Position for pipe
    public static Vector2 RandomGenerator(float padding, float spawnPoint){
        GD.Randomize();
        return Vector2.Right * spawnPoint +  Vector2.Down * (float)GD.RandRange(padding,Global.numberOfBlocks.y-padding) * Global.blockSize;;
    }

    //Generates pipes in form of sine wave
    public static Vector2 SineGenerator(float padding, float spawnPoint,float time){
        float y = Mathf.Sin(Mathf.Deg2Rad(time)) * (Global.numberOfBlocks.y - 2*padding) + padding;
        return Vector2.Right * spawnPoint +  Vector2.Down * y * Global.blockSize;
    }

    //Randomly generates pipes that are always in reach of bird
    public static Vector2 ModeratedRandom(float padding, float spawnPoint, Hardness hard, float prev_position){
        float maxDist = hard.birdSpeed * hard.spawnTime;
        maxDist -= hard.gap/4.0f;
        if(prev_position == -1){
            Vector2 temp = RandomGenerator(padding,spawnPoint);
            prev_position = temp.y / Global.blockSize;
            return temp;
        }
        GD.Randomize();
        float pos = prev_position + (float)GD.RandRange(-maxDist,maxDist);
        if(pos < padding){
            GD.Randomize();
            float diff = padding - pos;
            pos += (float)GD.RandRange(diff,maxDist);
        }
        else if(pos > Global.numberOfBlocks.y - padding){
            GD.Randomize();
            float diff = pos - (Global.numberOfBlocks.y - padding);
            pos -= (float)GD.RandRange(diff,maxDist);
        }
        /*pos = Mathf.Max(padding,pos);
        pos = Mathf.Min(pos,Global.numberOfBlocks.y-padding);*/
        return Vector2.Right * spawnPoint +  Vector2.Down * pos * Global.blockSize;
    }
}