using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public GameObject healthBarCanvas;
    private CharAnimBase characterAnim;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image backHealBar;
    [SerializeField] private TextMeshPro healthText;

    public int MaxHealth { get; set; }
    public int CurHealth {  get; set; }

    public event Action Die;
    public event Action DieAnimation;

    private void Start()
    {
        characterAnim = GetComponentInChildren<CharAnimBase>();
    }

    public void SetHealth(int health)
    {
        MaxHealth = health;
        CurHealth = health;

        healthBar.fillAmount = HealthRatio;
        healthText.text = CurHealth.ToString();
    }

    public float HealthRatio
    {
        get { return (float)CurHealth / (float)MaxHealth; }
    }

    public void TakeDamage(BattleKeyWords.Damage damage)// 실제 데미지 입힘
    {
        characterAnim.SetDamage(damage);

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
        BattleKeyWords.Damage damage = new BattleKeyWords.Damage();

        damage.damage = -n;

        ChangeHealth(damage);
    }

    public void HealHealth(BattleKeyWords.Damage n)// 실제 체력 회복
    {
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
        BattleKeyWords.Damage damage = new BattleKeyWords.Damage();

        damage.damage = n;

        ChangeHealth(damage);
    }

    public void ChangeHealth(BattleKeyWords.Damage n)//체력 변화
    {
        CurHealth += n.damage;

        if(CurHealth > MaxHealth)
        {
            CurHealth = MaxHealth;
        }

        if(CurHealth < 0)
        {
            CurHealth = 0;
        }

        if(n.damage < 0)
        {
            TakeDamage(n);
        }
        else
        {
            HealHealth(n);
        }
    }

    public void ChangeHealthByInt(int n)
    {
        if(n < 0)
        {
            n = -n;
            TakeDamageByInt(n);
        }
        else
        {
            HealHealthByInt(n);
        }
    }

    public void TakeDamageHealthBar(BattleKeyWords.Damage n)// 데미지 입힘
    {
        StartCoroutine(TakeHealthBar(false));

        healthText.text = CurHealth.ToString();

        characterAnim.ShowDamage();

        if (CurHealth <= 0)
        {
            DieAnimation?.Invoke();
        }
    }

    public void HealHealthBar(BattleKeyWords.Damage n)// 체력 회복함
    {
        StartCoroutine(TakeHealthBar(true));

        healthText.text = CurHealth.ToString();

        characterAnim.ShowDamage();
    }


    private IEnumerator TakeHealthBar(bool heal)// 체력바 변화
    {
        if(!heal)
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
        else if(heal)
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

    }
}
