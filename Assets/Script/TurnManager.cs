using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Turn    // ��� �׷��� ������  
    { 
        player,
        enemy
    }

    public Turn turn;
    public static TurnManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        turn = Turn.player;
    }

    public void NextTurn()
    {
        if (turn == Turn.player)
        {
            turn = Turn.enemy;
        }
        else if (turn == Turn.enemy)
        {
            turn = Turn.player;

            TargetManager.instance.PawnActionReset();
        }
    }
}