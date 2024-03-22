using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Image healthBar;

    public int MaxHealth { get; set; }
    [field:SerializeField] public int CurHealth {  get; set; }

    public event Action Die;

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
    }

    public void HealHealth(int n)// 실제 체력 회복
    {
        ChangeHealth(n);
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
        healthBar.fillAmount -= ((float)n / (float)MaxHealth);
    }

    public void HealHealthBar(int n)// 체력 회복함
    {
        healthBar.fillAmount += ((float)n / (float)MaxHealth);
    }

    public void SetHealthBar()// 체력바 실제 수치랑 맞추기
    {
        healthBar.fillAmount = HealthRatio;
    }

    private IEnumerator TakeHealthBar()
    {
        while (HealthRatio != healthBar.fillAmount)
        {
            if (HealthRatio < healthBar.fillAmount)
            {
                healthBar.fillAmount -= 0.1f;
            }
            else
            {
                healthBar.fillAmount += 0.1f;
            }

            yield return null;
        }
    }

    private IEnumerator TakeHealthBar(int n)// 체력바 변화
    {
        float number = (float)n / (float)MaxHealth;
        float tmp = 0;

        while (number != tmp)
        {
            if (number < 0)
            {
                healthBar.fillAmount -= 0.1f;
                tmp -= 0.1f;
            }
            else
            {
                healthBar.fillAmount += 0.1f;
                tmp += 0.1f;
            }

            yield return null;
        }
    }
}
