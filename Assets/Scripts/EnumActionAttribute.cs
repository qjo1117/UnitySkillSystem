using System;

/// <summary>
/// �Ϲ� �Լ����� �����
/// [EnumAction( typeof(E_PlayerDNDStateType ) )]
//     public void _On_ChangeType(int p_type)
//     {
// 
// 
//     }
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class EnumActionAttribute : UnityEngine.PropertyAttribute
{
    public Type enumType;

    public EnumActionAttribute(Type enumType)
    {
        this.enumType = enumType;
    }
}
