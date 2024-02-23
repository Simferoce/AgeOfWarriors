using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Lane lane;
    [SerializeField] private CharacterDefinition characterDefinition;

    private void Start()
    {
        lane.Initialize();
        characterDefinition.Instantiate(lane, 0, 1);
    }
}
