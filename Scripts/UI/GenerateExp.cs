using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UXF;

public class GenerateExp : MonoBehaviour
{
    public void Generate(Session session)
    {
        // nombre d'essais : 
        Block bloc = session.CreateBlock(5);
        foreach (Trial trial in bloc.trials)
        {
            trial.settings.SetValue("n1", 2);
            trial.settings.SetValue("n2", 4); 
        }

        session.GetTrial(3).settings.SetValue("number", 1); 
    }
}
