using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OnlyNew.AnimeStyleCharacters
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ColorSetter : MonoBehaviour
    {
        public Material OriginSpriteBlendMat;
        public Material SpriteBlendMat;
        public AnimeStyleCharactersAsset animeStyleCharactersAsset;
        private Material preOriginSpriteBlendMat;
        private Material preTextureBlendMat;
        private List<SetMatColor> setters = new List<SetMatColor>();
        public Dictionary<string, ColorSets> colorDict = new Dictionary<string, ColorSets>();
        bool colorChanged = false;
        [SerializeField]
        [HideInInspector]
        private int assetHash;

        Materials mats = new Materials();
        private void Awake()
        {


        }
        private void OnEnable()
        {
            SetDefaultMaterials();
            InitialMaterial();
            InitializeAsset();
        }
        private void Update()
        {
            if (colorChanged)
            {
                if (!Application.isPlaying)
                {
                    //SetterToAsset();
                }
                colorChanged = false;
            }
        }

        public bool AssetIsDirty()
        {
            if (animeStyleCharactersAsset != null)
            {
                if (assetHash != animeStyleCharactersAsset.GetHashCode())
                {
                    assetHash = animeStyleCharactersAsset.GetHashCode();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void InitialMaterial()
        {

            GetComponentsInChildren(true, setters);

            if (preOriginSpriteBlendMat != OriginSpriteBlendMat || preTextureBlendMat != SpriteBlendMat)
            {
                if (setters.Count > 0)
                {
                    foreach (var setter in setters)
                    {

                        setter.mats = mats;
                        if (setter.materialType == MaterialType.fromColorSetter)
                        {
                            bool isInclude = false;
                            isInclude = CheckInclude(setter.gameObject.name);
                            if (isInclude)
                            {
                                if (OriginSpriteBlendMat != null)
                                    setter.material = OriginSpriteBlendMat;
                            }
                            else
                            {
                                if (SpriteBlendMat != null)
                                    setter.material = SpriteBlendMat;
                            }
                        }
                        else
                        {
                            setter.SetMat();
                        }
                    }
                    preOriginSpriteBlendMat = OriginSpriteBlendMat;
                    preTextureBlendMat = SpriteBlendMat;
                }
            }
        }

        public void AssetToSetter(List<SetMatColor> setters)
        {
            if (AssetIsDirty())
            {
                //set colors
                SetColors(this, setters);
                //Debug.Log("AssetToSetter");
            }
        }

        private void SetColors(ColorSetter t, List<SetMatColor> setters)
        {
            foreach (SetMatColor setter in setters)
            {
                for (int i = 0; i < t.animeStyleCharactersAsset.Keys.Count; i++)
                {

                    if (setter.gameObject.name == t.animeStyleCharactersAsset.Keys[i])
                    {

                        setter.PrimaryColor = t.animeStyleCharactersAsset.Values[i].primaryColor;
                        setter.SecondColor = t.animeStyleCharactersAsset.Values[i].secondColor;
                        setter.ThirdColor = t.animeStyleCharactersAsset.Values[i].thirdColor;
                        break;
                    }
                }
            }
        }

        private bool CheckInclude(string name)
        {
            foreach (var item in GroupSettings.originSpriteBlendGroup)
            {
                if (name != item)
                {
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void OnValidate()
        {
            InitialMaterial();
            InitializeAsset();
        }
        public void InitializeAsset()
        {
            if (animeStyleCharactersAsset != null)
            {
                if (assetHash != animeStyleCharactersAsset.GetHashCode())
                {
                    assetHash = animeStyleCharactersAsset.GetHashCode();
                    SetColors(this, setters);
                    //Debug.Log("InitializeAsset");
                }
            }
        }

        public void SetDefaultMaterials()
        {

            mats.OriginSpriteBlendMat = OriginSpriteBlendMat;
            mats.TextureBlendMat = SpriteBlendMat;
            GetComponentsInChildren(true, setters);


            if (setters.Count > 0)
            {
                foreach (var setter in setters)
                {
                    setter.mats = mats;
                }
            }


        }
        public void ColorChanged()
        {
            colorChanged = true;

            //Debug.Log("ColorChanged");
        }
    }
}