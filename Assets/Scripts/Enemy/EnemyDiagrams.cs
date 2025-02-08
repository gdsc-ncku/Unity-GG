using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyDiagrams : MonoBehaviour
{
    static public Dictionary<LayerMask, Dictionary<LayerMask, int>> enemyDiagram = new();
    public const int stay = 0, runAway = -1, attack = 1;

    private void Start()
    {
        LayerTagPack.initDiagrams(enemyDiagram);
        init();
    }
    
    //關係圖: 1代表遇到會攻擊, 0代表待機, -1代表逃跑 
    private void init()
    {
        enemyDiagram[LayerMask.GetMask(LayerTagPack.MilitaryEnemy)][LayerMask.GetMask(LayerTagPack.Player)] = attack;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.MilitaryEnemy)][LayerMask.GetMask(LayerTagPack.MilitaryEnemy)] = stay;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.MilitaryEnemy)][LayerMask.GetMask(LayerTagPack.GPowerEnemy)] = attack;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.MilitaryEnemy)][LayerMask.GetMask(LayerTagPack.BaseEnemy)] = runAway;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.MilitaryEnemy)][LayerMask.GetMask(LayerTagPack.UndergroundDwellers)] = stay;

        enemyDiagram[LayerMask.GetMask(LayerTagPack.GPowerEnemy)][LayerMask.GetMask(LayerTagPack.Player)] = attack;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.GPowerEnemy)][LayerMask.GetMask(LayerTagPack.MilitaryEnemy)] = runAway;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.GPowerEnemy)][LayerMask.GetMask(LayerTagPack.GPowerEnemy)] = stay;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.GPowerEnemy)][LayerMask.GetMask(LayerTagPack.BaseEnemy)] = runAway;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.GPowerEnemy)][LayerMask.GetMask(LayerTagPack.UndergroundDwellers)] = stay;

        enemyDiagram[LayerMask.GetMask(LayerTagPack.BaseEnemy)][LayerMask.GetMask(LayerTagPack.Player)] = attack;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.BaseEnemy)][LayerMask.GetMask(LayerTagPack.MilitaryEnemy)] = attack;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.BaseEnemy)][LayerMask.GetMask(LayerTagPack.GPowerEnemy)] = attack;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.BaseEnemy)][LayerMask.GetMask(LayerTagPack.BaseEnemy)] = stay;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.BaseEnemy)][LayerMask.GetMask(LayerTagPack.UndergroundDwellers)] = attack;

        enemyDiagram[LayerMask.GetMask(LayerTagPack.UndergroundDwellers)][LayerMask.GetMask(LayerTagPack.Player)] = runAway;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.UndergroundDwellers)][LayerMask.GetMask(LayerTagPack.MilitaryEnemy)] = runAway;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.UndergroundDwellers)][LayerMask.GetMask(LayerTagPack.GPowerEnemy)] = runAway;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.UndergroundDwellers)][LayerMask.GetMask(LayerTagPack.BaseEnemy)] = runAway;
        enemyDiagram[LayerMask.GetMask(LayerTagPack.UndergroundDwellers)][LayerMask.GetMask(LayerTagPack.UndergroundDwellers)] = runAway;
    }
}
