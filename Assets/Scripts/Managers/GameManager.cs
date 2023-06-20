using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player CurrentPlayer { get; private set; } = null;

    [SerializeField]
    private Dice dice;

    private Player[] players;
    private List<Player> finishedPlayers;
    private int currentPlayerIndex;

    public void InitGame()
    {
        GenerateRandomMap();

        players = Players.Instance.GetComponentsInChildren<Player>();
        finishedPlayers = new List<Player>();
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
                < .70f => RockType.Fail,
                _ => RockType.Normal
            };
        }
    }

    private void Update()
    {

    }

    private void Start()
    {
        InitGame();
        StartCoroutine(GameCoroutine());
    }

    private IEnumerator GameCoroutine()
    {
        // main game loop
        while (finishedPlayers.Count < players.Length)
        {
            foreach (var player in players.Where(p => !p.IsFinished))
            {
                CurrentPlayer = player;
                yield return PlayTurn(player);
                if (player.IsFinished) finishedPlayers.Add(player);
            }
        }

        CurrentPlayer = null;

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

        var rock = RockPath.Instance.GetRock(player.CurrentPosition).GetComponent<Rock>();
        yield return rock.ActivateEffectOnCurrentPlayer();
    }
}
