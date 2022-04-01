using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE }

public class BattleManager : MonoBehaviour
{
    public GameObject character1Prefab;
    public GameObject enemy1Prefab;

    StatsScript characterStats;
    StatsScript enemyStats;

    public Text characterNameText;
    public Text characterHealthText;
    public Text enemyNameText;
    public Text enemyHealthText;
    public Text characterMPText;
    public Text enemyMPText;
    public Text outcomeText;

    public BattleState state;

    void Start()
    {
        state = BattleState.START;
        Spawn();
    }

    void Spawn()
    {
        GameObject characterSpawn = Instantiate(character1Prefab, new Vector3(500, 500), Quaternion.identity);
        characterStats = characterSpawn.GetComponent<StatsScript>();
        GameObject enemySpawn = Instantiate(enemy1Prefab, new Vector3(1500, 500), Quaternion.identity);
        enemyStats = enemySpawn.GetComponent<StatsScript>();

        characterNameText.text = characterStats.Name;
        characterHealthText.text = characterStats.currentHealth.ToString();
        characterMPText.text = characterStats.magicPoints.ToString();
        enemyNameText.text = enemyStats.Name;
        enemyHealthText.text = enemyStats.currentHealth.ToString();
        enemyMPText.text = enemyStats.magicPoints.ToString();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttacking()
    {
        bool isDefeated = enemyStats.Damage(characterStats.attackStat);

        outcomeText.text = characterStats.Name + " attacks!";

        yield return new WaitForSeconds(1f);

        enemyHealthText.text = (enemyStats.currentHealth).ToString();

        yield return new WaitForSeconds(1f);

        if (isDefeated)
        {
            state = BattleState.WIN;
            EndBattle();
        }

        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        outcomeText.text = characterStats.Name + " uses Heal!";

        yield return new WaitForSeconds(1f);

        if (characterStats.currentHealth >= characterStats.maxHealth)
        {
            outcomeText.text = "But " + characterStats.Name + " is already at full health.";
        }

 //       else if (characterStats.maxHealth >= characterStats.currentHealth >= characterStats.maxHealth - characterStats.healAmount)
 //       {
 //           characterStats.Heal(characterStats.maxHealth - characterStats.characterStats.currentHealth);
 //       }

        else
        {
            characterStats.magicPoints = characterStats.magicPoints - 2;
            characterMPText.text = (characterStats.magicPoints).ToString();
            characterStats.Heal(characterStats.healAmount);
            characterHealthText.text = (characterStats.currentHealth).ToString();
        }

        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerSpell()
    {
        bool isDefeated = enemyStats.Damage(characterStats.magicAttackStat);

        outcomeText.text = characterStats.Name + " casts Cyclone!";

        characterStats.magicPoints = characterStats.magicPoints - 8;

        characterMPText.text = (characterStats.magicPoints).ToString();

        enemyHealthText.text = (enemyStats.currentHealth).ToString();


        yield return new WaitForSeconds(1f);

        if (isDefeated)
        {
            state = BattleState.WIN;
            EndBattle();
        }

        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        outcomeText.text = enemyStats.Name + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDefeated = characterStats.Damage(enemyStats.attackStat);

        characterHealthText.text = (characterStats.currentHealth).ToString();

        yield return new WaitForSeconds(1f);

        if (isDefeated)
        {
            state = BattleState.LOSE;
            EndBattle();
        }

        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WIN)
        {
            outcomeText.text = "Victory!";
        }

        else if (state == BattleState.LOSE)
        {
            outcomeText.text = "Defeated.";
        }
    }

    void PlayerTurn()
    {
        outcomeText.text = "What will you do?";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerAttacking());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        if (characterStats.magicPoints < 2)
        {
            outcomeText.text = "Insufficient MP!";
        }

        else
        {
            StartCoroutine(PlayerHeal());
        }
    }

    public void OnSpellButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        if (characterStats.magicPoints < 8)
        {
            outcomeText.text = "Insufficient MP!";
        }

        else
        {
            StartCoroutine(PlayerSpell());
        }
    }
}
