using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OnlyNew.AnimeStyleCharacters
{
    [CustomEditor(typeof(ColorSetter))]
    [CanEditMultipleObjects]
    public class ColorSetterEditor : Editor
    {

        List<Color> colors = new List<Color>();
        List<SetMatColor> setters = new List<SetMatColor>();
        List<SetMatColorGUI> settersGUI = new List<SetMatColorGUI>();
        Dictionary<string, SetMatColorGUI> settersGUIDict = new Dictionary<string, SetMatColorGUI>();

        List<SetMatColorGUI> settersSkin = new List<SetMatColorGUI>();
        List<SetMatColorGUI> settersHair = new List<SetMatColorGUI>();
        List<SetMatColorGUI> settersFace = new List<SetMatColorGUI>();
        List<SetMatColorGUI> settersCloth = new List<SetMatColorGUI>();
        List<SetMatColorGUI> settersOthers = new List<SetMatColorGUI>();
        int hash = 0;
        static bool showSkin = true;
        static bool showHair = true;
        static bool showCloth = true;
        static bool showFace = true;
        static bool showOthers = true;

        List<bool> setterFoldouts = new List<bool>();
        int setterCount = 0;
        static bool advance = false;


        class SetMatColorGUI
        {
            public SetMatColorGUI(SetMatColor t_setter)
            {
                setter = t_setter;
                foldout = false;
            }
            public SetMatColor setter;
            public bool foldout;
        }
        public void OnEnable()
        {
            ColorSetter t = target as ColorSetter;
            var renderers = t.GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in renderers)
            {
                if (renderer.GetComponent<SetMatColor>() == null)
                {
                    renderer.gameObject.AddComponent<SetMatColor>();
                }
            }
            t.GetComponentsInChildren(true, setters);
            //foreach (var setter in setters)
            //{
            //    t.colorDict.Clear();
            //    t.colorDict.Add(setter.gameObject.name,new ColorSets(setter.PrimaryColor,setter.SecondColor,setter.ThirdColor));
            //}
            t.InitialMaterial();
            //t.InitializeAsset();

            Undo.RecordObjects(setters.ToArray(), "ColorSetters");
        }



        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ColorSetter t = target as ColorSetter;
            //t.InitialMaterial();
            //t.InitializeAsset();
            //t.GetComponentsInChildren<SetMatColor>(true, setters);
            //t.AssetToSetter(t, setters);
            Undo.RecordObjects(setters.ToArray(), "ColorSetters");
            //settersGUI.Clear();
            settersGUIDict.Clear();

            foreach (var setter in setters)
            {
                var gui = new SetMatColorGUI(setter);
                //settersGUI.Add(gui);
                settersGUIDict.Add(gui.setter.gameObject.name, gui);
            }
            int currentHash = target.GetHashCode();
            if (hash != currentHash)
            {
                hash = currentHash;
                ClearGroup();
                PickSettersToGroups(t);
            }
            advance = EditorGUILayout.ToggleLeft("Advance", advance);

            if (advance)
            {
                DrawAdvanceOptions();
            }
            else
            {
                DrawEasyOptions();

            }
            EditorGUILayout.BeginVertical();

            if (GUILayout.Button(new GUIContent("Save Asset", "Save Color to Asset")))
            {
                SaveAsset(t);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawEasyOptions()
        {
            setterCount = 0;
            showSkin = EditorGUILayout.Foldout(showSkin, "Skin", true, EditorStyles.foldoutPreDrop);
            if (showSkin)
            {
                if (settersSkin.Count > 0)
                {
                    if (settersSkin[0] != null)
                    {
                        settersSkin[0].setter.PrimaryColor = EditorGUILayout.ColorField("Skin Base Color", settersSkin[0].setter.PrimaryColor);
                        settersSkin[0].setter.SecondColor = EditorGUILayout.ColorField("Skin Shadow Color 1", settersSkin[0].setter.SecondColor);
                        settersSkin[0].setter.ThirdColor = EditorGUILayout.ColorField("Skin Shadow Color 2", settersSkin[0].setter.ThirdColor);
                        foreach (SetMatColorGUI setterGUI in settersSkin)
                        {
                            if (setterGUI == null)
                            {
                                Debug.LogError("error, setter == null");
                            }
                            else
                            {
                                setterGUI.setter.PrimaryColor = settersSkin[0].setter.PrimaryColor;
                                setterGUI.setter.SecondColor = settersSkin[0].setter.SecondColor;
                                setterGUI.setter.ThirdColor = settersSkin[0].setter.ThirdColor;
                            }
                        }
                    }
                }
            }
            EditorGUILayout.Space();

            showHair = EditorGUILayout.Foldout(showHair, "Hair", true, EditorStyles.foldoutPreDrop);
            if (showHair)
            {
                if (settersHair.Count > 0)
                {
                    if (settersHair[0] != null)
                    {
                        settersHair[0].setter.PrimaryColor = EditorGUILayout.ColorField("Hair Base Color", settersHair[0].setter.PrimaryColor);
                        settersHair[0].setter.SecondColor = EditorGUILayout.ColorField("Hair Shadow Color 1", settersHair[0].setter.SecondColor);
                        settersHair[0].setter.ThirdColor = EditorGUILayout.ColorField("Hair Shadow Color 2", settersHair[0].setter.ThirdColor);
                        foreach (SetMatColorGUI setterGUI in settersHair)
                        {
                            if (setterGUI == null)
                            {
                                Debug.LogError("error, setter == null");
                            }
                            else
                            {
                                setterGUI.setter.PrimaryColor = settersHair[0].setter.PrimaryColor;
                                setterGUI.setter.SecondColor = settersHair[0].setter.SecondColor;
                                setterGUI.setter.ThirdColor = settersHair[0].setter.ThirdColor;
                            }
                        }
                    }
                }
            }
            EditorGUILayout.Space();

            showFace = EditorGUILayout.Foldout(showFace, "Face", true, EditorStyles.foldoutPreDrop);
            if (showFace)
            {
                DrawSetters(settersFace);
            }
            EditorGUILayout.Space();

            showCloth = EditorGUILayout.Foldout(showCloth, "Cloth", true, EditorStyles.foldoutPreDrop);
            if (showCloth)
            {
                DrawSetters(settersCloth);
            }
            EditorGUILayout.Space();

            showOthers = EditorGUILayout.Foldout(showOthers, "Others", true, EditorStyles.foldoutPreDrop);
            if (showOthers)
            {
                DrawSetters(settersOthers);
            }
            EditorGUILayout.Space();
        }

        private void DrawAdvanceOptions()
        {
            setterCount = 0;
            showSkin = EditorGUILayout.Foldout(showSkin, "Skin", true, EditorStyles.foldoutPreDrop);
            if (showSkin)
            {
                DrawSetters(settersSkin);
            }
            EditorGUILayout.Space();

            showHair = EditorGUILayout.Foldout(showHair, "Hair", true, EditorStyles.foldoutPreDrop);
            if (showHair)
            {
                DrawSetters(settersHair);
            }
            EditorGUILayout.Space();

            showFace = EditorGUILayout.Foldout(showFace, "Face", true, EditorStyles.foldoutPreDrop);
            if (showFace)
            {
                DrawSetters(settersFace);
            }
            EditorGUILayout.Space();

            showCloth = EditorGUILayout.Foldout(showCloth, "Cloth", true, EditorStyles.foldoutPreDrop);
            if (showCloth)
            {
                DrawSetters(settersCloth);
            }
            EditorGUILayout.Space();

            showOthers = EditorGUILayout.Foldout(showOthers, "Others", true, EditorStyles.foldoutPreDrop);
            if (showOthers)
            {
                DrawSetters(settersOthers);
            }
            EditorGUILayout.Space();
        }

        bool setterFoldout = true;
        Color color;
        private void DrawSetters(List<SetMatColorGUI> settersGUI)
        {
            for (int i = 0; i < settersGUI.Count; i++)
            {
                var setter = settersGUI[i].setter;
                if (setter == null)
                {
                    Debug.LogError("error, setter == null");
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    setterFoldouts[setterCount] = EditorGUILayout.Foldout(setterFoldouts[setterCount], setter.gameObject.name, true);
                    if (setterFoldouts[setterCount])
                    {
                        setter.PrimaryColor = EditorGUILayout.ColorField(setter.PrimaryColor);
                        EditorGUILayout.EndHorizontal();

                        setter.SecondColor = EditorGUILayout.ColorField("SecondColor", setter.SecondColor);
                        setter.ThirdColor = EditorGUILayout.ColorField("ThirdColor", setter.ThirdColor);
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        setter.PrimaryColor = EditorGUILayout.ColorField(setter.PrimaryColor);
                        EditorGUILayout.EndHorizontal();
                    }


                }
                setterCount++;
            }
        }




        private void ClearGroup()
        {


            settersSkin.Clear();
            settersHair.Clear();
            settersFace.Clear();
            settersCloth.Clear();
            settersOthers.Clear();
            setterFoldouts.Clear();
        }

        private void PickSettersToGroups(ColorSetter t)
        {
            PickSettersToGroup(GroupSettings.skin, settersSkin);
            PickSettersToGroup(GroupSettings.hair, settersHair);
            PickSettersToGroup(GroupSettings.face, settersFace);
            PickSettersToGroup(GroupSettings.cloth, settersCloth);
            settersOthers.AddRange(settersGUIDict.Values);
            for (int i = 0; i < settersGUIDict.Count; i++)
            {
                setterFoldouts.Add(false);
            }
        }

        private void PickSettersToGroup(string[] category, List<SetMatColorGUI> groupedSetters)
        {
            foreach (var str in category)
            {
                //var setterGUI = settersGUI.Find(x => x.setter.gameObject.name.EndsWith(str));
                SetMatColorGUI setterGUI;
                if (settersGUIDict.TryGetValue(str, out setterGUI))
                {
                    groupedSetters.Add(setterGUI);
                    settersGUIDict.Remove(str);
                    //settersGUI.Remove(setterGUI);
                    setterFoldouts.Add(false);
                }
                else
                {
                    Debug.LogWarning("not found " + str);
                }
            }
        }


        void SaveAsset(ColorSetter t)
        {
            SetterToAsset(t);
        }
        public void SetterToAsset(ColorSetter t)
        {
            if (t.animeStyleCharactersAsset != null)
            {
                t.animeStyleCharactersAsset.Clear();
                foreach (var setter in setters)
                {
                    t.animeStyleCharactersAsset.Add(setter.gameObject.name,
                        new ColorSets(setter.PrimaryColor, setter.SecondColor, setter.ThirdColor));
                }
                UnityEditor.EditorUtility.SetDirty(t.animeStyleCharactersAsset);
            }
        }
    }
}