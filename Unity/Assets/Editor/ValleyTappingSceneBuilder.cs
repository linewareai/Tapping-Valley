#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ValleyTapping.Achievements;
using ValleyTapping.Economy;
using ValleyTapping.Farm;
using ValleyTapping.Pets;
using ValleyTapping.Quests;

namespace ValleyTapping.EditorTools
{
    public static class ValleyTappingSceneBuilder
    {
        const string Root = "Assets/ValleyTappingGenerated";
        const string Data = Root + "/Data";

        [MenuItem("ValleyTapping/Create Prototype Scene")]
        public static void Create()
        {
            Folder("Assets", "ValleyTappingGenerated");
            Folder(Root, "Data");
            Folder(Root, "Scenes");

            var crop = Asset<CropDefinition>("Carrot.asset", a => {
                Set(a, "cropId", "carrot"); Set(a, "displayName", "Zanahoria");
                Set(a, "seedCost", 5); Set(a, "growSeconds", 30); Set(a, "harvestReward", 12);
            });
            var pet = Asset<PetDefinition>("Mishi.asset", a => {
                Set(a, "petId", "mishi"); Set(a, "displayName", "Mishi"); Set(a, "adoptionCost", 25);
            });
            var tap = Asset<UpgradeDefinition>("TapPower.asset", a => {
                Set(a, "upgradeId", "tap-power"); Set(a, "displayName", "Dedos veloces");
                Set(a, "baseCost", 25L); Set(a, "costMultiplier", 1.5f);
                Set(a, "valuePerLevel", 1L); Set(a, "kind", (int)UpgradeKind.TapPower);
            });
            var passive = Asset<UpgradeDefinition>("PassiveIncome.asset", a => {
                Set(a, "upgradeId", "passive-income"); Set(a, "displayName", "Ingreso pasivo");
                Set(a, "baseCost", 50L); Set(a, "costMultiplier", 1.6f);
                Set(a, "valuePerLevel", 1L); Set(a, "kind", (int)UpgradeKind.PassiveIncome);
            });
            var tapQuest = Asset<QuestDefinition>("FirstTap.asset", a => {
                Set(a, "questId", "first-tap"); Set(a, "displayName", "Primer toque");
                Set(a, "target", 1); Set(a, "rewardCoins", 10);
            });
            var harvestQuest = Asset<QuestDefinition>("FirstHarvest.asset", a => {
                Set(a, "questId", "first-harvest"); Set(a, "displayName", "Primera cosecha");
                Set(a, "target", 1); Set(a, "rewardCoins", 20);
            });
            var achievement = Asset<AchievementDefinition>("FirstTapAchievement.asset", a => {
                Set(a, "achievementId", "first-tap"); Set(a, "displayName", "¡A tocar!");
                Set(a, "requiredLifetimeCoins", 1L); Set(a, "requiredTapCount", 1);
            });

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGO.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);

            // Mobile-first parchment viewport with a real scrollable content column.
            var panel = new GameObject("MainPanel", typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            panel.transform.SetParent(canvasGO.transform, false);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(.035f, .025f); rect.anchorMax = new Vector2(.965f, .975f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            panel.GetComponent<Image>().color = new Color(.29f,.40f,.24f);
            var viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            viewport.transform.SetParent(panel.transform, false);
            var vr = viewport.GetComponent<RectTransform>();
            vr.anchorMin = Vector2.zero; vr.anchorMax = Vector2.one; vr.offsetMin = Vector2.zero; vr.offsetMax = Vector2.zero;
            viewport.GetComponent<Image>().color = new Color(.96f,.88f,.68f);
            viewport.GetComponent<Mask>().showMaskGraphic = false;
            var content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            content.transform.SetParent(viewport.transform, false);
            var cr = content.GetComponent<RectTransform>();
            cr.anchorMin = new Vector2(0,1); cr.anchorMax = new Vector2(1,1);
            cr.pivot = new Vector2(.5f,1); cr.anchoredPosition = Vector2.zero; cr.sizeDelta = Vector2.zero;
            var layout = content.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(22,22,24,28); layout.spacing = 9;
            layout.childControlWidth = true; layout.childControlHeight = false;
            layout.childForceExpandWidth = true; layout.childForceExpandHeight = false;
            content.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var scroll = panel.GetComponent<ScrollRect>();
            scroll.viewport = vr; scroll.content = cr; scroll.horizontal = false; scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 70f;

            Label(content.transform, "🌿  VALLEY TAPPING  🌿", 38, 76);
            Text coins = Label(content.transform, "Monedas: 0", 28, 52);
            Text gems = Label(content.transform, "Gemas: 0", 24, 48);
            Text power = Label(content.transform, "Por toque: 1", 24, 48);
            Text level = Label(content.transform, "Nivel: 1", 24, 48);
            Text status = Label(content.transform, "¡Tu granja empieza aquí!", 22, 52);

            var controllerGO = new GameObject("GameWorldController");
            var controller = controllerGO.AddComponent<GameWorldController>();
            var so = new SerializedObject(controller);
            Array(so, "crops", crop); Array(so, "pets", pet);
            Array(so, "upgrades", tap, passive); Array(so, "quests", tapQuest, harvestQuest);
            Array(so, "achievements", achievement);
            Ref(so, "coinsText", coins); Ref(so, "gemsText", gems);
            Ref(so, "tapPowerText", power); Ref(so, "playerLevelText", level);
            Ref(so, "statusText", status);
            var plots = new Text[4];

            Button(content.transform, "TOCAR · GANAR MONEDAS", () => controller.Tap(), 68);
            Button(content.transform, "Mejorar toque · 25 monedas", () => controller.BuyUpgrade("tap-power"), 60);
            Button(content.transform, "Comprar ingreso pasivo", () => controller.BuyUpgrade("passive-income"), 60);
            Label(content.transform, "GRANJA", 28, 48);
            for (int i = 0; i < 4; i++) {
                int index = i;
                plots[i] = Label(content.transform, "Parcela " + (i+1) + ": libre", 20, 42);
                Button(content.transform, "Plantar zanahoria · " + (i+1), () => controller.PlantDefaultCrop(index), 52);
                Button(content.transform, "Cosechar · " + (i+1), () => controller.Harvest(index), 52);
            }
            Label(content.transform, "MASCOTA", 28, 48);
            Text active = Label(content.transform, "Mascota activa: ninguna", 20, 42);
            Text needs = Label(content.transform, "Hambre · Felicidad · Energía", 20, 42);
            Button(content.transform, "Adoptar a Mishi · 25 monedas", () => controller.AdoptPet("mishi"), 52);
            Button(content.transform, "Cuidar mascota", controller.CareForActivePet, 52);
            Button(content.transform, "Dar comida · 5 monedas", controller.FeedActivePet, 52);
            Button(content.transform, "Dejar descansar", controller.RestActivePet, 52);
            Label(content.transform, "PROGRESO", 28, 48);
            Text quests = Label(content.transform, "Misiones: 0", 20, 42);
            Text achievements = Label(content.transform, "Logros: 0", 20, 42);
            Button(content.transform, "Reclamar misión: primer toque", () => controller.ClaimQuest("first-tap"), 52);
            Button(content.transform, "Reclamar misión: primera cosecha", () => controller.ClaimQuest("first-harvest"), 52);

            Ref(so, "activePetText", active); Ref(so, "petNeedsText", needs);
            Ref(so, "questSummaryText", quests); Ref(so, "achievementSummaryText", achievements);
            Array(so, "plotLabels", plots); so.ApplyModifiedPropertiesWithoutUndo();

            const string scenePath = Root + "/Scenes/ValleyTappingPrototype.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(scenePath, true) };
            AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("ValleyTapping", "Escena móvil desplazable creada. Abre ValleyTappingGenerated/Scenes y pulsa Play. El sistema de juego es una base de migración, aún requiere paridad visual y QA.", "OK");
        }

        static Text Label(Transform parent, string value, int size, int height)
        {
            var go = new GameObject(value, typeof(RectTransform), typeof(Text), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            var text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = value; text.fontSize = size; text.color = new Color(.17f,.25f,.15f);
            text.alignment = TextAnchor.MiddleCenter; text.horizontalOverflow = HorizontalWrapMode.Wrap;
            go.GetComponent<LayoutElement>().preferredHeight = height;
            return text;
        }

        static Button Button(Transform parent, string label, UnityEngine.Events.UnityAction action, int height)
        {
            var go = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = new Color(.45f,.68f,.35f);
            go.GetComponent<LayoutElement>().preferredHeight = height;
            var text = Label(go.transform, label, 21, height);
            var r = text.rectTransform; r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one;
            r.offsetMin = new Vector2(8,2); r.offsetMax = new Vector2(-8,-2);
            var button = go.GetComponent<Button>(); button.targetGraphic = go.GetComponent<Image>();
            button.onClick.AddListener(action); return button;
        }

        static T Asset<T>(string name, System.Action<T> configure) where T : ScriptableObject
        {
            string path = Data + "/" + name;
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null) { asset = ScriptableObject.CreateInstance<T>(); configure(asset); AssetDatabase.CreateAsset(asset,path); }
            else configure(asset);
            EditorUtility.SetDirty(asset); return asset;
        }

        static void Set(UnityEngine.Object target, string name, string value) { var s = new SerializedObject(target); var p=s.FindProperty(name); if(p!=null)p.stringValue=value; s.ApplyModifiedPropertiesWithoutUndo(); }
        static void Set(UnityEngine.Object target, string name, int value) { var s = new SerializedObject(target); var p=s.FindProperty(name); if(p!=null){if(p.propertyType==SerializedPropertyType.Enum)p.enumValueIndex=value;else p.intValue=value;} s.ApplyModifiedPropertiesWithoutUndo(); }
        static void Set(UnityEngine.Object target, string name, long value) { var s = new SerializedObject(target); var p=s.FindProperty(name); if(p!=null)p.longValue=value; s.ApplyModifiedPropertiesWithoutUndo(); }
        static void Set(UnityEngine.Object target, string name, float value) { var s = new SerializedObject(target); var p=s.FindProperty(name); if(p!=null)p.floatValue=value; s.ApplyModifiedPropertiesWithoutUndo(); }
        static void Ref(SerializedObject s, string name, UnityEngine.Object value) { var p=s.FindProperty(name); if(p!=null)p.objectReferenceValue=value; }
        static void Array(SerializedObject s, string name, params UnityEngine.Object[] values) { var p=s.FindProperty(name); if(p==null)return; p.arraySize=values.Length; for(int i=0;i<values.Length;i++)p.GetArrayElementAtIndex(i).objectReferenceValue=values[i]; }
        static void Array(SerializedObject s, string name, Text[] values) { var objs=new UnityEngine.Object[values.Length]; for(int i=0;i<values.Length;i++)objs[i]=values[i]; Array(s,name,objs); }
        static void Folder(string parent, string child) { if(!AssetDatabase.IsValidFolder(parent+"/"+child))AssetDatabase.CreateFolder(parent,child); }
    }
}
