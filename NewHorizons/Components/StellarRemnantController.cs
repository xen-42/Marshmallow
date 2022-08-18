using NewHorizons.Components.SizeControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NewHorizons.Components
{
    public class StellarRemnantController : MonoBehaviour
    {
        private RemnantType _type = RemnantType.None;

        private StarEvolutionController _starEvolutionController;

        private StellarRemnantController _proxy;

        private StarController _starController;

        private float _surfaceGravity = 0;
        private float _surfaceSize = 0;
        private float _sphereOfInfluence = 0;
        private float _alignmentRadius = 0;

        public RemnantType GetRemnantType() => _type;
        public void SetRemnantType(RemnantType type) => _type = type;

        public void SetSurfaceGravity(float surfaceGravity) => _surfaceGravity = surfaceGravity;
        public void SetSurfaceSize(float surfaceSize) => _surfaceSize = surfaceSize;
        public void SetAlignmentRadius(float alignmentRadius) => _alignmentRadius = alignmentRadius;
        public void SetSphereOfInfluence(float sphereOfInfluence) => _sphereOfInfluence = sphereOfInfluence;
        public void SetStarController(StarController starController) => _starController = starController;

        public void SetProxy(StellarRemnantController proxy) => _proxy = proxy;

        public void SetStarEvolutionController(StarEvolutionController controller) => _starEvolutionController = controller;

        public void PartiallyActivate()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }

        public void FullyActivate()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);

            var gravityVolume = this.GetAttachedOWRigidbody().GetAttachedGravityVolume();
            if (gravityVolume != null)
            {
                gravityVolume._alignmentRadius = _alignmentRadius;
                gravityVolume._upperSurfaceRadius = _surfaceSize;
                gravityVolume._surfaceAcceleration = _surfaceGravity;
            }
            var referenceFrameVolume = this.GetAttachedOWRigidbody()._attachedRFVolume;
            if (referenceFrameVolume != null)
            {
                referenceFrameVolume.GetComponent<SphereCollider>().radius = _sphereOfInfluence * 2;
                referenceFrameVolume._maxColliderRadius = _sphereOfInfluence * 2;
            }

            if (_starController != null) StarLightController.AddStar(_starController);
        }

        public enum RemnantType
        {
            None,
            BlackHole,
            NeutronStar,
            WhiteDwarf,
            Custom
        }
    }
}
