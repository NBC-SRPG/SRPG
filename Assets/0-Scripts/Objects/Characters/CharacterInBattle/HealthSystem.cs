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
    [SerializeField] private TextMeshPro healthText;

    public int MaxHealth { get; set; }
    public int CurHealth {  get; set; }

    public event Action Die;
    public event Action DieAnimation;

    private void Start()
    {

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

    public void TakeDamage(int damage)// 실제 데미지 입힘
    {
        ChangeHealth(-damage);

        Debug.Log("take damage " +  damage);

        if(CurHealth == 0)
        {
            Die?.Invoke();
        }

        if (!AnimationController.instance.CheckAnimation())// 애니메이션 재생중이 아니면 곧바로 체력바 갱신
        {
            TakeDamageHealthBar(damage);
            Managers.UI.FindUI<BattleUI>().ShowDamageText(damage, transform);
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

        healthText.text = CurHealth.ToString();

        if (CurHealth <= 0)
        {
            DieAnimation?.Invoke();
        }
    }

    public void HealHealthBar(int n)// 체력 회복함
    {
        StartCoroutine(TakeHealthBar(true));

        healthText.text = CurHealth.ToString();

        Managers.UI.FindUI<BattleUI>().ShowDamageText(n, transform, true);
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
