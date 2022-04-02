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

    public Animator characterAnimator;
    public Animator enemyAnimator;

    void Start()
    {
        state = BattleState.START;
        Spawn();
    }

    void Spawn()
    {
        GameObject characterSpawn = Instantiate(character1Prefab, new Vector3(500, 500), Quaternion.identity);
        characterStats = characterSpawn.GetComponent<StatsScript>();
        characterAnimator = characterSpawn.GetComponent<Animator>();
        GameObject enemySpawn = Instantiate(enemy1Prefab, new Vector3(1500, 500), Quaternion.identity);
        enemyStats = enemySpawn.GetComponent<StatsScript>();
        enemyAnimator = enemySpawn.GetComponent<Animator>();

        characterNameText.text = characterStats.Name;
        characterHealthText.text = "HP: " + characterStats.currentHealth + "/" + characterStats.maxHealth;
        characterMPText.text = "MP: " + characterStats.magicPoints + "/" + characterStats.maxMagicPoints;
        enemyNameText.text = enemyStats.Name;
        enemyHealthText.text = "HP: " + enemyStats.currentHealth + "/" + enemyStats.maxHealth;
        enemyMPText.text = "MP: " + enemyStats.magicPoints + "/" + enemyStats.maxMagicPoints;

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttacking()
    {
        bool isDefeated = enemyStats.Damage(characterStats.attackStat);

        outcomeText.text = characterStats.Name + " attacks!";

        characterAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.3f);

        enemyAnimator.SetTrigger("Hurt");

        characterAnimator.SetTrigger("Idle");

        yield return new WaitForSeconds(1f);

        enemyAnimator.SetTrigger("Idle");

        enemyHealthText.text = enemyHealthText.text = "HP: " + enemyStats.currentHealth + "/" + enemyStats.maxHealth;

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

        characterAnimator.SetTrigger("Heal");

        yield return new WaitForSeconds(1f);

        if (characterStats.currentHealth >= characterStats.maxHealth)
        {
            outcomeText.text = "But " + characterStats.Name + " is already at full health.";
            characterAnimator.SetTrigger("Idle");
        }

 //       else if (characterStats.maxHealth >= characterStats.currentHealth >= characterStats.maxHealth - characterStats.healAmount)
 //       {
 //           characterStats.Heal(characterStats.maxHealth - characterStats.characterStats.currentHealth);
 //       }

        else
        {
            characterAnimator.SetTrigger("Idle");

            characterStats.magicPoints = characterStats.magicPoints - 2;
            characterMPText.text = "MP: " + characterStats.magicPoints + "/" + characterStats.maxMagicPoints;
            characterStats.Heal(characterStats.healAmount);
            characterHealthText.text = "HP: " + characterStats.currentHealth + "/" + characterStats.maxHealth;
        }

        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerSpell()
    {
        bool isDefeated = enemyStats.Damage(characterStats.magicAttackStat);

        outcomeText.text = characterStats.Name + " casts Cyclone!";

        characterAnimator.SetTrigger("Spell");

        yield return new WaitForSeconds(1f);

        enemyAnimator.SetTrigger("Hurt");

        characterAnimator.SetTrigger("Idle");

        yield return new WaitForSeconds(1f);

        enemyAnimator.SetTrigger("Idle");

        characterStats.magicPoints = characterStats.magicPoints - 8;

        characterMPText.text = "MP: " + characterStats.magicPoints + "/" + characterStats.maxMagicPoints;

        enemyHealthText.text = enemyHealthText.text = "HP: " + enemyStats.currentHealth + "/" + enemyStats.maxHealth;

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
        int enemyDecision = Random.Range(1, 4);

        if (enemyDecision == 1)
        {
            bool isDefeated = characterStats.Damage(enemyStats.attackStat);

            outcomeText.text = enemyStats.Name + " attacks!";

            enemyAnimator.SetTrigger("Attack");

            yield return new WaitForSeconds(1.3f);

            characterAnimator.SetTrigger("Hurt");

            enemyAnimator.SetTrigger("Idle");

            yield return new WaitForSeconds(1f);

            characterAnimator.SetTrigger("Idle");

            characterHealthText.text = "HP: " + characterStats.currentHealth + "/" + characterStats.maxHealth;

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

        if (enemyDecision == 2 && enemyStats.magicPoints >= 8)
        {
            bool isDefeated = characterStats.Damage(enemyStats.magicAttackStat);

            outcomeText.text = enemyStats.Name + " casts Cyclone!";

            enemyAnimator.SetTrigger("Spell");

            yield return new WaitForSeconds(1f);
            
            characterAnimator.SetTrigger("Hurt");

            enemyAnimator.SetTrigger("Idle");

            yield return new WaitForSeconds(1f);

            characterAnimator.SetTrigger("Idle");

            enemyStats.magicPoints = enemyStats.magicPoints - 8;

            enemyMPText.text = "MP: " + enemyStats.magicPoints + "/" + enemyStats.maxMagicPoints;

            characterHealthText.text = "HP: " + characterStats.currentHealth + "/" + characterStats.maxHealth;

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

        if (enemyDecision == 2 && enemyStats.magicPoints < 8)
        {
            outcomeText.text = enemyStats.Name + " is too tired to move!";

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

        if (enemyDecision == 3 && enemyStats.magicPoints >= 2)
        {
            outcomeText.text = enemyStats.Name + " uses Heal!";

            enemyAnimator.SetTrigger("Heal");

            yield return new WaitForSeconds(1f);

            if (enemyStats.currentHealth >= enemyStats.maxHealth)
            {
                outcomeText.text = "But " + enemyStats.Name + " is already at full health.";
                enemyAnimator.SetTrigger("Idle");
            }

            else
            {
                enemyAnimator.SetTrigger("Idle");

                enemyStats.magicPoints = enemyStats.magicPoints - 2;
                enemyMPText.text = "MP: " + enemyStats.magicPoints + "/" + enemyStats.maxMagicPoints;
                enemyStats.Heal(enemyStats.healAmount);
                enemyHealthText.text = enemyHealthText.text = "HP: " + enemyStats.currentHealth + "/" + enemyStats.maxHealth;
            }

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

        if (enemyDecision == 3 && enemyStats.magicPoints < 2)
        {
            outcomeText.text = enemyStats.Name + " is too tired to move!";

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WIN)
        {
            outcomeText.text = "Victory!";
            characterAnimator.SetTrigger("Victory");
            enemyAnimator.SetTrigger("Defeated");

        }

        else if (state == BattleState.LOSE)
        {
            outcomeText.text = "Defeated.";
            characterAnimator.SetTrigger("Defeated");
            enemyAnimator.SetTrigger("Victory");
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
