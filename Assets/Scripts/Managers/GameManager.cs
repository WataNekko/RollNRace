using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Player[] players;
    private List<Player> finishedPlayers;
    private IEnumerator<Player> playerTurn;

    /// <summary>
    /// Retrieves the current player.
    /// </summary>
    public Player CurrentPlayer => playerTurn?.Current;

    /// <summary>
    /// Starts the game by initializing the game and starting the first player's turn.
    /// </summary>
    public void StartGame()
    {
        InitGame();
        NextPlayerTurn();
    }

    private void InitGame()
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

    /// <summary>
    /// Generates a random map by assigning rock types based on probability.
    /// </summary>
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

    /// <summary>
    /// Enumerator for getting the next player in turn.
    /// </summary>
    /// <returns>The enumerator for iterating through players' turns.</returns>
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
            EndGame();
            return;
        }

        var player = CurrentPlayer;
        GameplayUI.Instance.HUD.PlayerTurn = player;

        Dice.Instance.OnDiceRollingEnabled.Invoke();
        Dice.Instance.OnDiceRolled += HandleDiceRolled;
        // enable the dice and wait for the dice rolling event to continue the
        // player's turn
    }

    private void HandleDiceRolled(int rolledValue)
    {
        Dice.Instance.OnDiceRolled -= HandleDiceRolled;
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

    /// <summary>
    /// Ends the game and displays the end game statistics.
    /// </summary>
    public void EndGame()
    {
        GameplayUI.Instance.HUD.gameObject.SetActive(false);
        GameplayUI.Instance.StatWindow.gameObject.SetActive(true);
        GameplayUI.Instance.StatWindow.FillStat(finishedPlayers);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
