using System.Reflection;
using UnityEngine;
using Shared;
namespace Client
{
    public class ActiveItemContainer : MonoBehaviour
    {
        public Item item;
        void Use()
        {
            if (item.activeEquipement)
            {
                ParameterModifier parameterModifier = new ParameterModifier(2);
                var method = item.activeEffect.GetMethod("Use");
                method.Invoke(null, null);
            }
        }
    }
}