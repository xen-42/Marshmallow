using NewHorizons.Components;
using NewHorizons.External.Modules;
using NewHorizons.Utility;
using OWML.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = NewHorizons.Utility.Logger;
using NHNotificationVolume = NewHorizons.Components.NotificationVolume;

namespace NewHorizons.Builder.Props
{
    public static class NotificationVolumeBuilder
    {
        public static NHNotificationVolume Make(GameObject planetGO, Sector sector, PropModule.NotificationVolumeInfo info, IModBehaviour mod)
        {
            var go = new GameObject("NotificationVolume");
            go.SetActive(false);

            go.transform.parent = sector?.transform ?? planetGO.transform;
            go.transform.position = planetGO.transform.TransformPoint(info.position != null ? (Vector3)info.position : Vector3.zero);
            go.layer = LayerMask.NameToLayer("BasicEffectVolume");

            var shape = go.AddComponent<SphereShape>();
            shape.radius = info.radius;

            var owTriggerVolume = go.AddComponent<OWTriggerVolume>();
            owTriggerVolume._shape = shape;

            var notificationVolume = go.AddComponent<NHNotificationVolume>();
            notificationVolume.SetTarget(info.target);
            if (info.entryNotification != null) notificationVolume.SetEntryNotification(info.entryNotification.displayMessage, info.entryNotification.duration);
            if (info.exitNotification != null) notificationVolume.SetExitNotification(info.exitNotification.displayMessage, info.exitNotification.duration);

            go.SetActive(true);

            return notificationVolume;
        }
    }
}
