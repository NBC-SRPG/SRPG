using UnityEngine;
using static Constants;


[CreateAssetMenu(menuName = "ClassData", fileName = "Class_")]
public class ClassSO : ScriptableObject
{
    [Header("Develope")]
    public int id;      //클래스 식별자
    public BaseClass baseClass;     //뿌리 클래스

    [Header("Class_description")]
    public string className;        //클래스 이름
    public string classDescription;     //클래스 설명

    
    [Header("Class_unique")]
    public int uniqueID;//클래스의 별도 고유 효과. 세부로직 미구현.
                           //Todo: 특성이 스탯 증가 외에 특수 기능을 구현하고 적용할 수 있도록 로직 설계 / 구현
}
