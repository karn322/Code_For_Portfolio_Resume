using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public enum AnimState { Attack, Idel, Wait, Dead }
public class BattleSystem : MonoBehaviour
{
    [SerializeField] private GameObject _PlayerPrefab;
    [SerializeField] private GameObject[] _EnemyPrefab;

    [SerializeField] private Transform _PlayerPosition;
    [SerializeField] private Transform[] _EnemyPosition;

    [SerializeField] private Transform[] _PlayerAttackPosition;
    [SerializeField] private Transform _EnemyAttackPosition;

    [SerializeField] private Text _DialogueText;

    [SerializeField] private BattleHUD _PlayerHUD;
    [SerializeField] private BattleHUD[] _EnemyHUD;
    [SerializeField] private GameObject[] _EnemyHUDGameObjects;

    [SerializeField] private Image _PlayerHealth;
    [SerializeField] private Image[] _EnemyHealth;

    [SerializeField] private IngredientSo _Ingredient;

    [SerializeField] private GameObject _TargetMark;
    private float _targetMarkOffset = 3f;
    private int _EnemyIndex = 0;

    private Character _PlayerUnit;
    private Character[] _EnemyUnit;

    private PlayerAbility _PlayerAbility;

    private int _EnemyNumber;
    private int _EnemyType;

    private bool _PlayerAction;
    private bool[] _IsDeadEnemy;

    [SerializeField] private Text _DamageText;
    [SerializeField] private Transform[] _EnemyDamagePosition;
    [SerializeField] private Transform _PlayerDamagePosition;

    private BattleState _State;


    void Start()
    {
        _State = BattleState.START;
        _DamageText.enabled = false;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject PlayerGO = Instantiate(_PlayerPrefab, _PlayerPosition);
        _PlayerUnit = PlayerGO.GetComponent<Character>();
        _PlayerAbility = PlayerGO.GetComponent<PlayerAbility>();

        _EnemyNumber = Random.Range(1, 4);
        _EnemyType = Random.Range(0, _EnemyPrefab.Length);

        _EnemyUnit = new Character[_EnemyNumber];
        _IsDeadEnemy = new bool[_EnemyNumber];

        for (int i = 0; i < _EnemyNumber; i++)
        {
            GameObject EnemyGO = Instantiate(_EnemyPrefab[_EnemyType], _EnemyPosition[i]);
            _EnemyUnit[i] = EnemyGO.GetComponent<Character>();
            _EnemyUnit[i].SetIndex(i);
        }

        for (int i = 0; i < 3; i++)
        {
            if (_EnemyNumber > i)
                continue;
            _EnemyHUDGameObjects[i].SetActive(false);
        }

        _DialogueText.text = " " + _EnemyUnit[0]._Name + " approaches... " + _EnemyNumber + " ";

        _PlayerHUD.SetHUD(_PlayerUnit);
        for (int i = 0; i < _EnemyNumber; i++)
        {
            _EnemyHUD[i].SetHUD(_EnemyUnit[i]);
        }

        ShowTarget();

        yield return new WaitForSeconds(1f);

        _State = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        _DialogueText.text = "Choose an action";
        _PlayerAction = false;
        for (int i = 0; i < _EnemyNumber; i++)
        {
            if (!_IsDeadEnemy[i])
            {
                ChangeTarget(i);
                return;
            }
        }
    }

    public void OnAttackButton()
    {
        if (_State != BattleState.PLAYERTURN)
            return;
        if (_PlayerAction)
            return;

        _PlayerAction = true;
        StartCoroutine(PlayerAttack());
    }

    public void OnAbiliotyButton(int i)
    {
        if (_State != BattleState.PLAYERTURN)
            return;
        if (_PlayerAction)
            return;

        _PlayerAction = true;
        if (i == 0)
        {
            StartCoroutine(TestItem());
        }
    }

    IEnumerator PlayerAttack()
    {
        _PlayerUnit.PlayWaitAnim();

        for (float t = 0f; t <= 1; t += _PlayerUnit.GetAnimTime(AnimState.Wait) * 2 * Time.deltaTime)
        {
            _PlayerUnit.transform.position = Vector3.Lerp(_PlayerPosition.position, _PlayerAttackPosition[_EnemyIndex].position, t);
            yield return null;
        }

        _PlayerUnit.PlayAttackAnim();

        yield return new WaitForSeconds(_PlayerUnit.GetAnimTime(AnimState.Attack));

        //play slash anim at enemy position
        SoundManager.Instance.PlayEffect(Sound.SoundEffectName.PlayerAttack);

        _IsDeadEnemy[_EnemyIndex] = _EnemyUnit[_EnemyIndex].TakeDamage(_PlayerUnit._Damage);

        _EnemyHUD[_EnemyIndex].SetHP(_EnemyUnit[_EnemyIndex]._CurrentHP);
        _DialogueText.text = "You deal " + _PlayerUnit._Damage + " to Enemy.";

        ShowDamage(_EnemyDamagePosition[_EnemyIndex], _PlayerUnit);
        // Show Damage at enemy

        _PlayerUnit.PlayWaitAnim();

        if (_IsDeadEnemy[_EnemyIndex])
        {
            _EnemyUnit[_EnemyIndex].Dead();
            _EnemyHealth[_EnemyIndex].color = Color.black;
        }

        for (float t = 0f; t <= 1; t += _PlayerUnit.GetAnimTime(AnimState.Wait) * 2 * Time.deltaTime)
        {
            _PlayerUnit.transform.position = Vector3.Lerp(_PlayerAttackPosition[_EnemyIndex].position, _PlayerPosition.position, t);
            yield return null;
        }

        _DamageText.enabled = false;
        _PlayerUnit.PlayIdelAnim();

        yield return new WaitForSeconds(0.5f);

        bool isDeadAll = false;
        if (_IsDeadEnemy[_EnemyIndex])
        {
            int enemyCount = _EnemyNumber;
            for (int i = 0; i < _EnemyNumber; i++)
            {
                if (_IsDeadEnemy[i])
                {
                    enemyCount--;
                }
            }

            if (enemyCount <= 0)
            {
                isDeadAll = true;
            }
        }

        if (isDeadAll)
        {
            _State = BattleState.WON;
            EndBattle();
        }
        else
        {
            _State = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator TestItem()
    {
        _PlayerUnit.TakeHeal(_PlayerAbility.GetItem0());
        _PlayerHUD.SetHP(_PlayerUnit._CurrentHP);
        _DialogueText.text = "You used Healing Item ! heal " + _PlayerAbility.GetItem0() + " point";

        yield return new WaitForSeconds(1f);

        _State = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        bool isDead = false;
        

        if (_EnemyUnit[0]._IsRangeType)
        {
            for (int i = 0; i < _EnemyUnit.Length; i++)
            {
                if (_IsDeadEnemy[i])
                {
                    continue;
                }

                _EnemyUnit[i].PlayAttackAnim();

                if (_EnemyUnit[i]._Name == "SleepyPig")
                {
                    _EnemyUnit[i]._Body.SetActive(true);
                    _EnemyUnit[i]._Bullet.GetComponent<SpriteRenderer>().enabled = true;

                    yield return new WaitForSeconds(_EnemyUnit[i].GetAnimTime(AnimState.Attack));
                }

                _EnemyUnit[i]._Bullet.GetComponent<SpriteRenderer>().enabled = true;

                for (float t = 0f; t <= 1; t += _EnemyUnit[i].GetAnimTime(AnimState.Attack) * 2 * Time.deltaTime)
                {
                    _EnemyUnit[i]._Bullet.transform.position = Vector3.Lerp(_EnemyPosition[i].position, _EnemyAttackPosition.position, t);
                    yield return null;
                }

                _EnemyUnit[i]._Bullet.GetComponent<SpriteRenderer>().enabled = false;
                isDead = _PlayerUnit.TakeDamage(_EnemyUnit[i]._Damage);
                SoundManager.Instance.PlayEffect(Sound.SoundEffectName.PlayerAttack);
                _PlayerHUD.SetHP(_PlayerUnit._CurrentHP);
                _DialogueText.text = _EnemyUnit[i]._Name + " deal " + _EnemyUnit[i]._Damage + " to you.";
                ShowDamage(_PlayerDamagePosition, _EnemyUnit[i]);

                yield return new WaitForSeconds(.5f);
                // Show Damage At Player
                _DamageText.enabled = false;
                _EnemyUnit[i].PlayIdelAnim();

                if (_EnemyUnit[i]._Name == "SleepyPig")
                {
                    _EnemyUnit[i]._Body.SetActive(false);
                }

                _EnemyUnit[i]._Bullet.transform.position = new Vector3(_EnemyUnit[i]._BulletTransform.position.x, _EnemyUnit[i]._BulletTransform.position.y, 0);
               

                if (isDead)
                {
                    _PlayerHealth.color = Color.black;
                    _State = BattleState.LOST;
                    EndBattle();
                }
            }

            _State = BattleState.PLAYERTURN;

            yield return new WaitForSeconds(0.5f);
            PlayerTurn();
        }
        else
        {
            for (int i = 0; i < _EnemyUnit.Length; i++)
            {
                if (_IsDeadEnemy[i])
                {
                    continue;
                }

                _EnemyUnit[i].PlayWaitAnim();

                for (float t = 0f; t <= 1; t += _EnemyUnit[i].GetAnimTime(AnimState.Wait) * 2 * Time.deltaTime)
                {
                    _EnemyUnit[i].transform.position = Vector3.Lerp(_EnemyPosition[i].position, _EnemyAttackPosition.position, t);
                    yield return null;
                }

                _EnemyUnit[i].PlayAttackAnim();
                yield return new WaitForSeconds(_EnemyUnit[i].GetAnimTime(AnimState.Attack));

                //play slash anim at player position
                SoundManager.Instance.PlayEffect(Sound.SoundEffectName.PlayerAttack);

                isDead = _PlayerUnit.TakeDamage(_EnemyUnit[i]._Damage);
                _PlayerHUD.SetHP(_PlayerUnit._CurrentHP);
                _DialogueText.text = _EnemyUnit[i]._Name + " deal " + _EnemyUnit[i]._Damage + " to you.";
                ShowDamage(_PlayerDamagePosition, _EnemyUnit[i]);

                // Show Damage At Player

                _EnemyUnit[i].PlayWaitAnim();
                for (float t = 0f; t <= 1; t += _EnemyUnit[i].GetAnimTime(AnimState.Wait) * 2 * Time.deltaTime)
                {
                    _EnemyUnit[i].transform.position = Vector3.Lerp(_EnemyAttackPosition.position, _EnemyPosition[i].position, t);
                    yield return null;
                }

                _DamageText.enabled = false;
                _EnemyUnit[i].PlayIdelAnim();

                if (isDead)
                {
                    _PlayerHealth.color = Color.black;
                    _State = BattleState.LOST;
                    EndBattle();
                }
            }
            _State = BattleState.PLAYERTURN;

            yield return new WaitForSeconds(0.5f);
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (_State == BattleState.WON)
        {
            StartCoroutine(WonEnemy());

        }
        if (_State == BattleState.LOST)
        {
            _DialogueText.text = "You lost";

            Finish();
        }
    }

    IEnumerator WonEnemy()
    {
        if (_EnemyUnit[0]._Name == "CottonSpider")
        {
            _DialogueText.text = "Check your backpack on top right of the scene to check the drop.";
        }
        else
        {
            _DialogueText.text = "You won the battle!!! and get " + _EnemyNumber + " " + _EnemyUnit[0]._IngredientName + ".";
        }

        for (int i = 0; i < _EnemyNumber; i++)
        {
            _Ingredient.AddIngredientToBackpack(_EnemyUnit[i]._IngredientName, 1);
        }

        yield return new WaitForSeconds(1f);

        Finish();
    }

    public void RunOut()
    {
        int DefeatEnemyCount = 0;
        for (int i = 0; i < _EnemyNumber; i++)
        {
            if (_IsDeadEnemy[i])
            {
                DefeatEnemyCount++;
                _Ingredient.AddIngredientToBackpack(_EnemyUnit[i]._IngredientName, 1);
            }
        }

        Finish();
    }

    public void ChangeTarget(int i)
    {
        _EnemyIndex = i;
        ShowTarget();
    }

    private void ShowTarget()
    {
        _TargetMark.transform.position = _EnemyPosition[_EnemyIndex].position + new Vector3(0,_targetMarkOffset,0);
    }

    private void ShowDamage(Transform transform, Character Unit)
    {
        _DamageText.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        _DamageText.text = $"{Unit._Damage}";
        _DamageText.enabled = true;
    }

    public void Finish()
    {
        changeScene.LoadNextScene("Map");
    }

}
