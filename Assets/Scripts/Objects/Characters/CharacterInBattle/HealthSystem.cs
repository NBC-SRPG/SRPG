using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public GameObject healthBarCanvas;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image backHealBar;

    public int MaxHealth { get; set; }
    public int CurHealth {  get; set; }

    public event Action Die;
    public event Action DieAnimation;

    private void Start()
    {
        //AnimationController.instance.onAnimationEnd += SetHealthBar;
    }

    public void SetHealth(int health)
    {
        MaxHealth = health;
        CurHealth = health;

        healthBar.fillAmount = HealthRatio;
    }

    public float HealthRatio
    {
        get { return (float)CurHealth / (float)MaxHealth; }
    }

    public void TakeDamage(int damage)// 실제 데미지 입힘
    {
        ChangeHealth(-damage);

        if(CurHealth == 0)
        {
            Die?.Invoke();
        }

        if (!AnimationController.instance.CheckAnimation())// 애니메이션 재생중이 아니면 곧바로 체력바 갱신
        {
            TakeDamageHealthBar(damage);
        }
        else
        {
            AnimationController.instance.StitchAnimation(() => TakeDamageHealthBar(damage));
        }
    }

    public void HealHealth(int n)// 실제 체력 회복
    {
        ChangeHealth(n);

        if (!AnimationController.instance.CheckAnimation())
        {
            HealHealthBar(n);
        }
        else
        {
            AnimationController.instance.StitchAnimation(() => HealHealthBar(n));
        }
    }

    private void ChangeHealth(int n)//체력 변화
    {
        CurHealth += n;

        if(CurHealth > MaxHealth)
        {
            CurHealth = MaxHealth;
        }

        if(CurHealth < 0)
        {
            CurHealth = 0;
        }
    }

    public void TakeDamageHealthBar(int n)// 데미지 입힘
    {
        StartCoroutine(TakeHealthBar(false));

        //Managers.UI.FindUI<BattleUI>().ShowDamageText(n, transform);

        if (CurHealth <= 0)
        {
            DieAnimation?.Invoke();
        }
    }

    public void HealHealthBar(int n)// 체력 회복함
    {
        StartCoroutine(TakeHealthBar(true));

        Managers.UI.FindUI<BattleUI>().ShowDamageText(n, transform, true);
    }

    public void SetHealthBar()// 체력바 실제 수치랑 맞추기
    {
        healthBar.fillAmount = HealthRatio;

        if (CurHealth <= 0)
        {
            DieAnimation?.Invoke();
        }
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
