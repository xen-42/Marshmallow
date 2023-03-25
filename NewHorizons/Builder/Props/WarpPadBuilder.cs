using NewHorizons.Builder.Props.TranslatorText;
using NewHorizons.External.Modules.Props;
using NewHorizons.External.Modules.WarpPad;
using NewHorizons.Utility;
using NewHorizons.Utility.OWMLUtilities;
using OWML.Utils;
using UnityEngine;
using Logger = NewHorizons.Utility.Logger;

namespace NewHorizons.Builder.Props
{
    public static class WarpPadBuilder
    {
        private static GameObject _detailedReceiverPrefab;
        private static GameObject _receiverPrefab;
        private static GameObject _transmitterPrefab;
        private static GameObject _platformContainerPrefab;

        public static void InitPrefabs()
        {
            if (_platformContainerPrefab == null)
            {
                // Put this around the platforms without details 
                // Trifid is a Nomai ruins genius
                _platformContainerPrefab = SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Sector_SouthHemisphere/Sector_SouthPole/Sector_Observatory/Interactables_Observatory/Prefab_NOM_RemoteViewer/Structure_NOM_RemoteViewer")
                    .InstantiateInactive()
                    .DontDestroyOnLoad();
                _platformContainerPrefab.transform.localScale = new Vector3(0.85f, 3f, 0.85f);
            }

            if (_detailedReceiverPrefab == null)
            {
                var thReceiverLamp = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_NomaiCrater/Geometry_NomaiCrater/OtherComponentsGroup/Structure_NOM_WarpReceiver_TimberHearth_Lamp");
                var thReceiver = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_NomaiCrater/Interactables_NomaiCrater/Prefab_NOM_WarpReceiver");

                _detailedReceiverPrefab = new GameObject("NomaiWarpReceiver");

                var detailedReceiver = thReceiver.InstantiateInactive();
                detailedReceiver.transform.parent = _detailedReceiverPrefab.transform;
                detailedReceiver.transform.localPosition = Vector3.zero;
                detailedReceiver.transform.localRotation = Quaternion.identity;

                var lamp = thReceiverLamp.InstantiateInactive();
                lamp.transform.parent = _detailedReceiverPrefab.transform;
                lamp.transform.localPosition = thReceiver.transform.InverseTransformPoint(thReceiverLamp.transform.position);
                lamp.transform.localRotation = thReceiver.transform.InverseTransformRotation(thReceiverLamp.transform.rotation);

                _detailedReceiverPrefab.SetActive(false);
                lamp.SetActive(true);
                detailedReceiver.SetActive(true);

                _detailedReceiverPrefab.DontDestroyOnLoad();

                GameObject.Destroy(_detailedReceiverPrefab.GetComponentInChildren<NomaiWarpStreaming>().gameObject);
            }

            if (_receiverPrefab == null)
            {
                _receiverPrefab = SearchUtilities.Find("SunStation_Body/Sector_SunStation/Sector_WarpModule/Interactables_WarpModule/Prefab_NOM_WarpReceiver")
                    .InstantiateInactive()
                    .DontDestroyOnLoad();
                GameObject.Destroy(_receiverPrefab.GetComponentInChildren<NomaiWarpStreaming>().gameObject);

                var structure = _platformContainerPrefab.Instantiate();
                structure.transform.parent = _receiverPrefab.transform;
                structure.transform.localPosition = new Vector3(0, 0.8945f, 0);
                structure.transform.localRotation = Quaternion.identity;
                structure.SetActive(true);
            }

            if (_transmitterPrefab == null)
            {
                _transmitterPrefab = SearchUtilities.Find("TowerTwin_Body/Sector_TowerTwin/Sector_Tower_SS/Interactables_Tower_SS/Tower_SS_VisibleFrom_TowerTwin/Prefab_NOM_WarpTransmitter")
                    .InstantiateInactive()
                    .DontDestroyOnLoad();
                GameObject.Destroy(_transmitterPrefab.GetComponentInChildren<NomaiWarpStreaming>().gameObject);

                var structure = _platformContainerPrefab.Instantiate();
                structure.transform.parent = _transmitterPrefab.transform;
                structure.transform.localPosition = new Vector3(0, 0.8945f, 0);
                structure.transform.localRotation = Quaternion.identity;
                structure.SetActive(true);
            }
        }

        public static void Make(GameObject planetGO, Sector sector, NomaiWarpReceiverInfo info)
        {
            var detailInfo = new DetailInfo(info);
            var receiverObject = DetailBuilder.Make(planetGO, sector, info.detailed ? _detailedReceiverPrefab : _receiverPrefab, detailInfo);

            Logger.Log($"Position is {detailInfo.position} was {info.position}");

            var receiver = receiverObject.GetComponentInChildren<NomaiWarpReceiver>();

            receiver._frequency = GetFrequency(info.frequency);

            receiver._alignmentTarget = planetGO?.transform;

            receiverObject.SetActive(true);

            if (info.computer != null)
            {
                CreateComputer(planetGO, sector, info.computer, receiver);
            }
        }

        public static void Make(GameObject planetGO, Sector sector, NomaiWarpTransmitterInfo info)
        {
            var transmitterObject = DetailBuilder.Make(planetGO, sector, _transmitterPrefab, new DetailInfo(info));

            var transmitter = transmitterObject.GetComponentInChildren<NomaiWarpTransmitter>();
            transmitter._frequency = GetFrequency(info.frequency);

            transmitter._alignmentWindow = info.alignmentWindow;

            transmitter._upsideDown = info.flipAlignment;

            transmitter.GetComponent<BoxShape>().enabled = true;

            transmitterObject.SetActive(true);
        }

        private static void CreateComputer(GameObject planetGO, Sector sector, NomaiWarpComputerLoggerInfo computerInfo, NomaiWarpReceiver receiver)
        {
            var computerObject = DetailBuilder.Make(planetGO, sector, TranslatorTextBuilder.ComputerPrefab, new DetailInfo(computerInfo));

            var computer = computerObject.GetComponentInChildren<NomaiComputer>();
            computer.SetSector(sector);

            Delay.FireOnNextUpdate(computer.ClearAllEntries);

            var computerLogger = computerObject.AddComponent<NomaiWarpComputerLogger>();
            computerLogger._warpReceiver = receiver;

            computerObject.SetActive(true);
        }

        private static NomaiWarpPlatform.Frequency GetFrequency(string frequency)
        {
            if (!EnumUtils.TryParse<NomaiWarpPlatform.Frequency>(frequency, out var frequencyEnum))
            {
                frequencyEnum = EnumUtilities.Create<NomaiWarpPlatform.Frequency>(frequency);
            }
            return frequencyEnum;
        }
    }
}