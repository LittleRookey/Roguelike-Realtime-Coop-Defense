using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnlyNew.AnimeStyleCharacters
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnityEngine.U2D.Animation.SpriteLibrary))]
    [RequireComponent(typeof(ColorSetter))]
    [ExecuteAlways]
    public class SpriteLibrarySetter : MonoBehaviour
    {
        public string label;
        List<UnityEngine.U2D.Animation.SpriteResolver> spriteResolvers = new List<UnityEngine.U2D.Animation.SpriteResolver>();
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        ColorSetter colorSetter;

        private void Awake()
        {
            GetComponentsInChildren(true, spriteRenderers);
            colorSetter = GetComponent<ColorSetter>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.gameObject.AddComponent<UnityEngine.U2D.Animation.SpriteResolver>();
            }
            SetSkin();
        }
        void SetSkin()
        {
            GetComponentsInChildren(true, spriteResolvers);
            if (spriteResolvers.Count > 0)
            {
                foreach (var item in spriteResolvers)
                {
                    setLabel(item, label);
                }
            }
            else
            {
                Debug.LogError("cannot find SpriteResolver");
            }
        }

        private static void setLabel(UnityEngine.U2D.Animation.SpriteResolver item, string label)
        {
            foreach (var name in GroupSettings.skin)
            {
                if (item.gameObject.name == name)
                {
                    item.SetCategoryAndLabel(item.gameObject.name, "base skin");
                    return;
                }
            }
            item.SetCategoryAndLabel(item.gameObject.name, label);

        }
        private void OnValidate()
        {
            SetSkin();
        }
    }
}