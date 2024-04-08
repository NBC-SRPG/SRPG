using UnityEngine;
using static Constants;


[CreateAssetMenu(menuName = "ClassData", fileName = "Class_")]
public class ClassSO : PassiveSO
{
    [Header("Develope")]
    public int id;      //클래스 식별자
    public BaseClass baseClass;     //뿌리 클래스

    [Header("Class_description")]
    public string className;        //클래스 이름
    public string description;     //클래스 설명

}
