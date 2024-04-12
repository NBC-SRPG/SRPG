using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BattleKeyWords;

public class HealthSystem : MonoBehaviour
{
    public GameObject healthBarCanvas;
    private CharAnimBase characterAnim;

    private TempBonusStat tempBonus;
    private CharacterBufList characterBufList;

    private List<ShieldStat> shieldList;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image backHealBar;
    [SerializeField] private Image shieldBar;
    [SerializeField] private TextMeshPro healthText;

    public int MaxHealth { get; set; }
    public int CurHealth { get; set; }

    public event Action Die;
    public event Action DieAnimation;

    public void InitHealth(int health, TempBonusStat stat, CharacterBufList buflist, CharAnimBase charAnim)
    {
        shieldList = new List<ShieldStat>();

        MaxHealth = health;
        CurHealth = health;

        tempBonus = stat;
        characterBufList = buflist;
        characterAnim = charAnim;

        healthBar.fillAmount = HealthRatio;
        UpdateText();
    }

    public float HealthRatio
    {
        get
        {
            float health;
            int shield = GetShield();

            if (CurHealth + shield > MaxHealth)
            {
                health = (float)(CurHealth) / (float)(CurHealth + shield);
            }
            else
            {
                health = (float)CurHealth / (float)MaxHealth;
            }

            return health;
        }
    }

    public float ShieldRatio
    {
        get
        {
            float health;
            int shield = GetShield();

            if (CurHealth + shield > MaxHealth)
            {
                health = (float)(shield + CurHealth) / (float)(CurHealth + shield);
            }
            else
            {
                health = (float)shield + (float)CurHealth / (float)MaxHealth;
            }

            return health;
        }
    }

    public void TakeDamage(Damage damage)// 실제 데미지 입힘
    {
        if (damage.damage >= 0)
        {
            damage.damage = -damage.damage;
        }

        characterAnim.SetDamage(damage);

        damage.damage = TakeShiledDamage(damage.damage);

        ChangeHealth(damage);

        if (CurHealth == 0)
        {
            Die?.Invoke();
        }

        if (!AnimationController.instance.CheckAnimation())// 애니메이션 재생중이 아니면 곧바로 체력바 갱신
        {
            characterAnim.ShowDamage();
            TakeDamageHealthBar(damage);
        }
        else
        {
            AnimationController.instance.StitchAnimation(() => TakeDamageHealthBar(damage));
        }
    }

    public void TakeDamageByInt(int n)// int만 입력받아 데미지
    {
        Damage damage = new Damage();

        if (damage.damage >= 0)
        {
            damage.damage = -n;
        }
        else
        {
            damage.damage = n;
        }

        TakeDamage(damage);
    }

    public void HealHealth(Damage n)// 실제 체력 회복
    {
        if (n.damage < 0)
        {
            TakeDamage(n);
            return;
        }

        ChangeHealth(n);

        characterAnim.SetDamage(n);

        if (!AnimationController.instance.CheckAnimation())
        {
            HealHealthBar(n);
        }
        else
        {
            AnimationController.instance.StitchAnimation(() => HealHealthBar(n));
        }
    }

    public void HealHealthByInt(int n)// int만 입력받아 체력 회복
    {
        if (n < 0)
        {
            TakeDamageByInt(n);
            return;
        }

        Damage damage = new Damage();

        damage.damage = n;

        HealHealth(damage);
    }

    public void ChangeHealth(Damage n)//체력 변화
    {
        CurHealth += n.damage;

        if (CurHealth > MaxHealth)
        {
            CurHealth = MaxHealth;
        }

        if (CurHealth < 0)
        {
            CurHealth = 0;
        }
    }

    public void TakeDamageHealthBar(Damage n)// 데미지 입힘
    {
        StartCoroutine(TakeHealthBar(false));

        UpdateText();

        characterAnim.ShowDamage();

        if (CurHealth <= 0)
        {
            DieAnimation?.Invoke();
        }
    }

    public void HealHealthBar(Damage n)// 체력 회복함
    {
        StartCoroutine(TakeHealthBar(true));

        UpdateText();

        characterAnim.ShowDamage();
    }

    public void ChangeHealthBar()
    {
        healthBar.fillAmount = HealthRatio;
        shieldBar.fillAmount = ShieldRatio;

        UpdateText();
    }


    private IEnumerator TakeHealthBar(bool heal)// 체력바 변화
    {
        if (!heal)
        {
            backHealBar.color = Color.yellow;
            backHealBar.fillAmount = healthBar.fillAmount;
            healthBar.fillAmount = HealthRatio;

            while (backHealBar.fillAmount * 0.9 > healthBar.fillAmount)
            {
                backHealBar.fillAmount = Mathf.Lerp(backHealBar.fillAmount, healthBar.fillAmount, Time.deltaTime * 2f);

                yield return null;
            }

            backHealBar.fillAmount = healthBar.fillAmount;
        }
        else if (heal)
        {
            backHealBar.color = Color.green;
            backHealBar.fillAmount = HealthRatio;

            while (backHealBar.fillAmount * 0.9 > healthBar.fillAmount)
            {
                healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, backHealBar.fillAmount, Time.deltaTime * 2f);

                yield return null;
            }

            healthBar.fillAmount = backHealBar.fillAmount;
        }

        shieldBar.fillAmount = ShieldRatio;
    }

    public void UpdateText()
    {
        if (GetShield() > 0)
        {
            healthText.text = GetShield().ToString();
            healthText.color = Color.blue;
        }
        else
        {
            healthText.text = CurHealth.ToString();
            healthText.color = Color.white;
        }
    }

    public int TakeShiledDamage(int damage)
    {
        while (shieldList.Count > 0)
        {
            shieldList[0].Shield += damage;

            if (shieldList[0].Shield <= 0)
            {
                damage = shieldList[0].Shield;
                shieldList.RemoveAt(0);
            }
            else
            {
                damage = 0;
                break;
            }
        }

        return damage;
    }

    public int GetShield()
    {
        if(shieldList.Count == 0)
        {
            return 0;
        }

        int s = 0;

        foreach(ShieldStat shield in shieldList)
        {
            s += shield.Shield;
        }

        return s;
    }

    public void AddShield(ShieldStat shield)
    {
        shieldList.Add(shield);

        if (!AnimationController.instance.CheckAnimation())// 애니메이션 재생중이 아니면 곧바로 체력바 갱신
        {
            ChangeHealthBar();
        }
        else
        {
            AnimationController.instance.StitchAnimation(() => ChangeHealthBar());
        }
    }

    public void RemoveShield(ShieldStat shield)
    {
        if (shieldList.Contains(shield))
        {
            shieldList.Remove(shield);

            if (!AnimationController.instance.CheckAnimation())// 애니메이션 재생중이 아니면 곧바로 체력바 갱신
            {
                ChangeHealthBar();
            }
            else
            {
                AnimationController.instance.StitchAnimation(() => ChangeHealthBar());
            }
        }
        else
        {
            return;
        }
    }
}
