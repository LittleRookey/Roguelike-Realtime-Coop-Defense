using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
namespace OnlyNew.AnimeStyleCharacters
{
    [CustomEditor(typeof(RandomAssetSetter))]
    public class RandomAssetSetterEditor : Editor
    {
        // Start is called before the first frame update
        public void ImportAssets(RandomAssetSetter t)
        {
            List<AnimeStyleCharactersAsset> m_assets = new List<AnimeStyleCharactersAsset>();
            m_assets.Clear();
            var setters = t.GetComponentsInChildren<ColorSetter>();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:AnimeStyleCharactersAsset");
            if (guids != null && guids.Length > 0)
            {
                foreach (var guid in guids)
                {
                    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<AnimeStyleCharactersAsset>(path);
                    if (asset.name != "basic")
                    {
                        m_assets.Add(asset);
                    }
                }


            }
            List<SetMatColor> setMatColors = new List<SetMatColor>();
            if (m_assets != null && m_assets.Count > 0)
            {
                foreach (var setter in setters)
                {

                    setter.animeStyleCharactersAsset = m_assets[Random.Range(0, m_assets.Count)];
                    setMatColors.Clear();
                    setter.GetComponentsInChildren(true, setMatColors);

                    setter.AssetToSetter(setMatColors);

                }
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = target as RandomAssetSetter;
            if (GUILayout.Button(new GUIContent("Random Import Color Asset", "Reference assets from path")))
            {
                ImportAssets(t);
            }
        }
    }
}