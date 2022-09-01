using NewHorizons.External.Modules;
using NewHorizons.Components;
using NewHorizons.Utility;
using OWML.Common;
using System;
using UnityEngine;
using Logger = NewHorizons.Utility.Logger;
namespace NewHorizons.Builder.Atmosphere
{
    public static class CloudsBuilder
    {
        private static Material[] _gdCloudMaterials;
        private static Material[] _qmCloudMaterials;
        private static Material _transparentCloud;
        private static GameObject _lightningPrefab;
        private static Texture2D _colorRamp;
        private static readonly int Color = Shader.PropertyToID("_Color");
        private static readonly int ColorRamp = Shader.PropertyToID("_ColorRamp");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int RampTex = Shader.PropertyToID("_RampTex");
        private static readonly int CapTex = Shader.PropertyToID("_CapTex");
        private static readonly int Smoothness = Shader.PropertyToID("_Glossiness");

        public static void Make(GameObject planetGO, Sector sector, AtmosphereModule atmo, bool cloaked, IModBehaviour mod)
        {
            if (_lightningPrefab == null) _lightningPrefab = SearchUtilities.Find("GiantsDeep_Body/Sector_GD/Clouds_GD/LightningGenerator_GD");
            if (_colorRamp == null) _colorRamp = ImageUtilities.GetTexture(Main.Instance, "Assets/textures/Clouds_Bottom_ramp.png");

            GameObject cloudsMainGO = new GameObject("Clouds");
            cloudsMainGO.SetActive(false);
            cloudsMainGO.transform.parent = sector?.transform ?? planetGO.transform;

            if (atmo.clouds.cloudsPrefab != CloudPrefabType.Transparent) MakeTopClouds(cloudsMainGO, atmo, mod);
            else
            {
                MakeTransparentClouds(cloudsMainGO, atmo, mod);
                if (atmo.clouds.hasLightning) MakeLightning(cloudsMainGO, sector, atmo);
                cloudsMainGO.transform.position = planetGO.transform.TransformPoint(Vector3.zero);
                cloudsMainGO.SetActive(true);
                return;
            }

            GameObject cloudsBottomGO = new GameObject("BottomClouds");
            cloudsBottomGO.SetActive(false);
            cloudsBottomGO.transform.parent = cloudsMainGO.transform;
            cloudsBottomGO.transform.localScale = Vector3.one * atmo.clouds.innerCloudRadius;

            TessellatedSphereRenderer bottomTSR = cloudsBottomGO.AddComponent<TessellatedSphereRenderer>();
            bottomTSR.tessellationMeshGroup = SearchUtilities.Find("CloudsBottomLayer_QM").GetComponent<TessellatedSphereRenderer>().tessellationMeshGroup;
            var bottomTSRMaterials = SearchUtilities.Find("CloudsBottomLayer_QM").GetComponent<TessellatedSphereRenderer>().sharedMaterials;

            // If they set a colour apply it to all the materials else keep the default QM one
            if (atmo.clouds.tint != null)
            {
                var bottomColor = atmo.clouds.tint.ToColor();

                var bottomTSRTempArray = new Material[2];

                bottomTSRTempArray[0] = new Material(bottomTSRMaterials[0]);
                bottomTSRTempArray[0].SetColor(Color, bottomColor);
                bottomTSRTempArray[0].SetTexture(ColorRamp, ImageUtilities.TintImage(_colorRamp, bottomColor));

                bottomTSRTempArray[1] = new Material(bottomTSRMaterials[1]);

                bottomTSR.sharedMaterials = bottomTSRTempArray;
            }
            else
            {
                bottomTSR.sharedMaterials = bottomTSRMaterials;
            }

            bottomTSR.maxLOD = 6;
            bottomTSR.LODBias = 0;
            bottomTSR.LODRadius = 1f;

            if (cloaked)
                cloudsBottomGO.AddComponent<CloakedTessSphereSectorToggle>()._sector = sector;
            else
                cloudsBottomGO.AddComponent<TessSphereSectorToggle>()._sector = sector;

            GameObject cloudsFluidGO = new GameObject("CloudsFluid");
            cloudsFluidGO.SetActive(false);
            cloudsFluidGO.layer = 17;
            cloudsFluidGO.transform.parent = cloudsMainGO.transform;

            SphereCollider fluidSC = cloudsFluidGO.AddComponent<SphereCollider>();
            fluidSC.isTrigger = true;
            fluidSC.radius = atmo.size;

            OWShellCollider fluidOWSC = cloudsFluidGO.AddComponent<OWShellCollider>();
            fluidOWSC._innerRadius = atmo.size * 0.9f;

            CloudLayerFluidVolume fluidCLFV = cloudsFluidGO.AddComponent<CloudLayerFluidVolume>();
            fluidCLFV._layer = 5;
            fluidCLFV._priority = 1;
            fluidCLFV._density = 1.2f;
            fluidCLFV._fluidType = atmo.clouds.fluidType.ConvertToOW(FluidVolume.Type.CLOUD);
            fluidCLFV._allowShipAutoroll = true;
            fluidCLFV._disableOnStart = false;

            // Fix the rotations once the rest is done
            cloudsMainGO.transform.rotation = planetGO.transform.TransformRotation(Quaternion.Euler(0, 0, 0));

            // Lightning
            if (atmo.clouds.hasLightning)
            {
                MakeLightning(cloudsMainGO, sector, atmo);
            }

            cloudsMainGO.transform.position = planetGO.transform.TransformPoint(Vector3.zero);
            cloudsBottomGO.transform.position = planetGO.transform.TransformPoint(Vector3.zero);
            cloudsFluidGO.transform.position = planetGO.transform.TransformPoint(Vector3.zero);

            cloudsBottomGO.SetActive(true);
            cloudsFluidGO.SetActive(true);
            cloudsMainGO.SetActive(true);
        }

        public static CloudLightningGenerator MakeLightning(GameObject rootObject, Sector sector, AtmosphereModule atmo, bool noAudio = false)
        {
            var lightning = _lightningPrefab.InstantiateInactive();
            lightning.name = "LightningGenerator";
            lightning.transform.parent = rootObject.transform;
            lightning.transform.localPosition = Vector3.zero;

            var lightningGenerator = lightning.GetComponent<CloudLightningGenerator>();
            lightningGenerator._altitude = atmo.clouds.cloudsPrefab != CloudPrefabType.Transparent ? (atmo.clouds.outerCloudRadius + atmo.clouds.innerCloudRadius) / 2f : atmo.clouds.outerCloudRadius;
            if (noAudio)
            {
                lightningGenerator._audioPrefab = null;
                lightningGenerator._audioSourcePool = null;
            }
            lightningGenerator._audioSector = sector;
            if (atmo.clouds.lightningGradient != null)
            {
                var gradient = new GradientColorKey[atmo.clouds.lightningGradient.Length];

                for (int i = 0; i < atmo.clouds.lightningGradient.Length; i++)
                {
                    var pair = atmo.clouds.lightningGradient[i];
                    gradient[i] = new GradientColorKey(pair.tint.ToColor(), pair.time);
                }

                lightningGenerator._lightColor.colorKeys = gradient;
            }
            lightning.SetActive(true);
            return lightningGenerator;
        }

        public static GameObject MakeTopClouds(GameObject rootObject, AtmosphereModule atmo, IModBehaviour mod)
        {
            Texture2D image, cap, ramp;

            try
            {
                // qm cloud type = should wrap, otherwise clamp like normal
                image = ImageUtilities.GetTexture(mod, atmo.clouds.texturePath, wrap: atmo.clouds.cloudsPrefab == CloudPrefabType.QuantumMoon);

                if (atmo.clouds.capPath == null) cap = ImageUtilities.ClearTexture(128, 128, wrap: true);
                else cap = ImageUtilities.GetTexture(mod, atmo.clouds.capPath, wrap: true);
                if (atmo.clouds.rampPath == null) ramp = ImageUtilities.CanvasScaled(image, 1, image.height);
                else ramp = ImageUtilities.GetTexture(mod, atmo.clouds.rampPath);
            }
            catch (Exception e)
            {
                Logger.LogError($"Couldn't load Cloud textures for [{atmo.clouds.texturePath}]:\n{e}");
                return null;
            }

            GameObject cloudsTopGO = new GameObject("TopClouds");
            cloudsTopGO.SetActive(false);
            cloudsTopGO.transform.parent = rootObject.transform;
            cloudsTopGO.transform.localScale = Vector3.one * atmo.clouds.outerCloudRadius;

            MeshFilter topMF = cloudsTopGO.AddComponent<MeshFilter>();
            topMF.mesh = SearchUtilities.Find("CloudsTopLayer_GD").GetComponent<MeshFilter>().mesh;

            MeshRenderer topMR = cloudsTopGO.AddComponent<MeshRenderer>();

            if (_gdCloudMaterials == null) _gdCloudMaterials = SearchUtilities.Find("CloudsTopLayer_GD").GetComponent<MeshRenderer>().sharedMaterials;
            if (_qmCloudMaterials == null) _qmCloudMaterials = SearchUtilities.Find("CloudsTopLayer_QM").GetComponent<MeshRenderer>().sharedMaterials;
            Material[] prefabMaterials = atmo.clouds.cloudsPrefab == CloudPrefabType.GiantsDeep ? _gdCloudMaterials : _qmCloudMaterials;
            var tempArray = new Material[2];

            if (atmo.clouds.cloudsPrefab == CloudPrefabType.Basic)
            {
                var material = new Material(Shader.Find("Standard"));
                if (atmo.clouds.unlit) material.renderQueue = 3000;
                material.name = atmo.clouds.unlit ? "BasicCloud" : "BasicShadowCloud";
                material.SetFloat(Smoothness, 0f);
                tempArray[0] = material;
            }
            else
            {
                var material = new Material(prefabMaterials[0]);
                if (atmo.clouds.unlit) material.renderQueue = 3000;
                material.name = atmo.clouds.unlit ? "AdvancedCloud" : "AdvancedShadowCloud";
                tempArray[0] = material;
            }

            // This is the stencil material for the fog under the clouds
            tempArray[1] = new Material(prefabMaterials[1]);
            topMR.sharedMaterials = tempArray;

            foreach (var material in topMR.sharedMaterials)
            {
                material.SetTexture(MainTex, image);
                material.SetTexture(RampTex, ramp);
                material.SetTexture(CapTex, cap);
            }

            if (atmo.clouds.unlit)
            {
                cloudsTopGO.layer = LayerMask.NameToLayer("IgnoreSun");
            }

            if (atmo.clouds.rotationSpeed != 0f)
            {
                var topRT = cloudsTopGO.AddComponent<RotateTransform>();
                topRT._localAxis = Vector3.up;
                topRT._degreesPerSecond = atmo.clouds.rotationSpeed;
                topRT._randomizeRotationRate = false;
            }

            cloudsTopGO.transform.localPosition = Vector3.zero;

            cloudsTopGO.SetActive(true);

            return cloudsTopGO;
        }

        public static GameObject MakeTransparentClouds(GameObject rootObject, AtmosphereModule atmo, IModBehaviour mod, bool isProxy = false)
        {
            Texture2D image;

            try
            {
                image = ImageUtilities.GetTexture(mod, atmo.clouds.texturePath);
            }
            catch (Exception e)
            {
                Logger.LogError($"Couldn't load Cloud texture for [{atmo.clouds.texturePath}]:\n{e}");
                return null;
            }

            GameObject cloudsTransparentGO = new GameObject("TransparentClouds");
            cloudsTransparentGO.SetActive(false);
            cloudsTransparentGO.transform.parent = rootObject.transform;
            cloudsTransparentGO.transform.localScale = Vector3.one * atmo.clouds.outerCloudRadius;

            MeshFilter filter = cloudsTransparentGO.AddComponent<MeshFilter>();
            filter.mesh = SearchUtilities.Find("CloudsTopLayer_GD").GetComponent<MeshFilter>().mesh;

            MeshRenderer renderer = cloudsTransparentGO.AddComponent<MeshRenderer>();
            if (_transparentCloud == null) _transparentCloud = Main.NHAssetBundle.LoadAsset<Material>("Assets/Resources/TransparentCloud.mat");
            var material = _transparentCloud;
            material.SetTexture(MainTex, image);
            renderer.sharedMaterial = material;

            if (atmo.clouds.rotationSpeed != 0f)
            {
                var rt = cloudsTransparentGO.AddComponent<RotateTransform>();
                rt._localAxis = Vector3.up;
                rt._degreesPerSecond = atmo.clouds.rotationSpeed;
                rt._randomizeRotationRate = false;
            }

            cloudsTransparentGO.transform.localPosition = Vector3.zero;

            cloudsTransparentGO.SetActive(true);

            return cloudsTransparentGO;
        }
    }
}
