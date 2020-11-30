using UnityEngine;
using System.Collections;

public class Tank : TankBase
{

    protected override void OnReset()
    {

    }

    protected override void OnThink(float dt) 
	{
        Vector3 dirToMine = GetDirToMine(nearMine);

        Vector3 dir = this.transform.forward;

        inputs[0] = dirToMine.x;
        inputs[1] = dirToMine.y;
        inputs[2] = dir.x;
        inputs[2] = dir.y;

        float[] outputs = brain.Synapsis(inputs);

        SetForces(outputs[0], outputs[1], dt);

        //
    }
    
    protected override void OnTakeMine(GameObject mine)
    {
        genome.fitness++;
    }
}
