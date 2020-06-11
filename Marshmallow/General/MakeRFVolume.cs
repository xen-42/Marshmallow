﻿using OWML.ModHelper.Events;
using UnityEngine;

namespace Marshmallow.General
{
    static class MakeRFVolume
    {
        public static void Make(GameObject body, OWRigidbody rigidbody)
        {
            GameObject rfGO = new GameObject("RFVolume");
            rfGO.transform.parent = body.transform;
            rfGO.layer = 19;
            rfGO.SetActive(false);

            SphereCollider SC = rfGO.AddComponent<SphereCollider>();
            SC.isTrigger = true;
            SC.radius = 600f;

            ReferenceFrameVolume RFV = rfGO.AddComponent<ReferenceFrameVolume>();

            ReferenceFrame RV = new ReferenceFrame(rigidbody);
            RV.SetValue("_minSuitTargetDistance", 300);
            RV.SetValue("_maxTargetDistance", 0);
            RV.SetValue("_autopilotArrivalDistance", 1000);
            RV.SetValue("_autoAlignmentDistance", 1000);
            RV.SetValue("_hideLandingModePrompt", false);
            RV.SetValue("_matchAngularVelocity", true);
            RV.SetValue("_minMatchAngularVelocityDistance", 70);
            RV.SetValue("_maxMatchAngularVelocityDistance", 400);
            RV.SetValue("_bracketsRadius", 300);

            RFV.SetValue("_referenceFrame", RV);
            RFV.SetValue("_minColliderRadius", 300);
            RFV.SetValue("_maxColliderRadius", 2000);
            RFV.SetValue("_isPrimaryVolume", true);
            RFV.SetValue("_isCloseRangeVolume", false);

            rfGO.SetActive(true);
        }
    }
}
