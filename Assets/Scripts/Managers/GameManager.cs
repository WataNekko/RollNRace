using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState _state;
    public GameState State
    {
        get => _state;
        set
        {
            if (_state == value) return;

            _state = value;
            switch (_state)
            {
                case GameState.Running:
                    InitGame();
                    NextPlayerTurn();
                    break;
                case GameState.Ended:
                    GameplayUI.Instance.HUD.gameObject.SetActive(false);
                    GameplayUI.Instance.StatWindow.gameObject.SetActive(true);
                    GameplayUI.Instance.StatWindow.FillStat(finishedPlayers);
                    break;
            }
        }
    }

    public Player CurrentPlayer => playerTurn?.Current;

    private Player[] players;
    private List<Player> finishedPlayers;
    private IEnumerator<Player> playerTurn;

    public void InitGame()
    {
        GenerateRandomMap();

        players = PlayerManager.Instance.GetPlayers();
        finishedPlayers = new List<Player>();
        playerTurn = PlayerTurnEnumerator();

        foreach (var player in players)
        {
            player.Init();
        }
    }

    public void GenerateRandomMap()
    {
        var rocks = RockPath.Instance.GetRocks();
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

    private void Update()
    {
        switch (State)
        {
            case GameState.Running:
                break;
            case GameState.Ended:
                break;
        }
    }

    /// <summary>
    /// For getting the next player in turn.
    /// </summary>
    /// <returns>Enumerator for getting the next player in turn</returns>
    private IEnumerator<Player> PlayerTurnEnumerator()
    {
        while (finishedPlayers.Count < players.Length)
        {
            foreach (var player in players.Where(p => !p.IsFinished))
            {
                if (player.CurrentTurnGain < 0)
                {
                    // loses turns
                    player.CurrentTurnGain++;
                    continue;
                }

                yield return player;

                // extra turns
                for (; player.CurrentTurnGain > 0; player.CurrentTurnGain--)
                {
                    if (player.IsFinished)
                    {
                        player.CurrentTurnGain = 0;
                        break;
                    }

                    yield return player;
                }
            }
        }
    }

    private void NextPlayerTurn()
    {
        if (playerTurn?.MoveNext() == false)
        {
            // every player finished
            playerTurn = null; // reset iterator
            State = GameState.Ended;
            return;
        }

        var player = CurrentPlayer;
        GameplayUI.Instance.HUD.PlayerTurn = player;

        Dice.Instance.OnDiceRollingEnabled.Invoke();
        Dice.Instance.OnDiceRolled += HandleDiceRolled;
        // enable the dice and wait for the dice rolling event to continue the
        // player's turn

        Debug.Log("Waiting for Dice Rolled event");
    }

    private void HandleDiceRolled(int rolledValue)
    {
        Dice.Instance.OnDiceRolled -= HandleDiceRolled;
        Debug.Log("Got a " + rolledValue);
        StartCoroutine(MovePlayerCoroutine(rolledValue));
    }

    private IEnumerator MovePlayerCoroutine(int rolledValue)
    {
        var player = CurrentPlayer;

        yield return player.MoveSteps(rolledValue);
        player.TurnCount++;

        if (RockPath.Instance.GetRock(player.CurrentPosition)
            .TryGetComponent(out Rock rock))
        {
            yield return rock.ActivateEffectOnCurrentPlayer();
        }

        if (player.IsFinished) finishedPlayers.Add(player);

        NextPlayerTurn();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}

public enum GameState
{
    Starting,
    Running,
    Ended
}