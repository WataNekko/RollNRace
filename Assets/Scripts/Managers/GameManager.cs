using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Dice dice;

    private Coroutine gameCoroutine;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        if (gameCoroutine != null)
        {
            StopCoroutine(gameCoroutine);
        }
        gameCoroutine = StartCoroutine(GameCoroutine());
    }

    private void GenerateRandomMap()
    {
        var rocks = RockPath.Instance.GetComponentsInChildren<Rock>();
        foreach (var rock in rocks)
        {
            // assign rock type based on probability
            rock.Type = UnityEngine.Random.value switch
            {
                < .15f => RockType.Bonus,
                < .30f => RockType.Fail,
                _ => RockType.Normal
            };
        }
    }

    private IEnumerator GameCoroutine()
    {
        // Init
        GenerateRandomMap();

        var players = Players.Instance.GetComponentsInChildren<Player>();
        var finishedPlayers = new List<Player>();

        // main game loop
        while (finishedPlayers.Count < players.Length)
        {
            foreach (var player in players.Where(p => !p.IsFinished))
            {
                yield return PlayTurn(player);
                if (player.IsFinished) finishedPlayers.Add(player);
            }
        }

        Debug.Log("Game ended:");
        Debug.Log("Scoreboard:" + Environment.NewLine +
            string.Join(Environment.NewLine,
                finishedPlayers.Select((p, i) => $"{i + 1}. {p.name}")));
    }

    private bool rollDiceSignal = false;
    public void RollDice() => rollDiceSignal = true;

    private IEnumerator PlayTurn(Player player)
    {
        HUD.Instance.TurnText.text = player.name + "'s turn";

        Debug.Log("Waiting for Roll Dice Signal");
        yield return new WaitUntil(() => rollDiceSignal);
        rollDiceSignal = false;

        Debug.Log("Rolling");
        int rolled = 0;
        yield return Dice.Instance.Roll((n) => rolled = n);
        Debug.Log("Got a " + rolled);

        yield return player.MoveSteps(rolled);

        player.TurnCount++;
    }
}
