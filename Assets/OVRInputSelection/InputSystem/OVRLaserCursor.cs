using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerSelection
{
    /// <summary>
    /// Implements a cursor with a reacting reticle that will cast a laser from the origin point of the ray if the controller isn't gazed
    /// </summary>
    public class OVRLaserCursor : OVRCursor
    {
        struct RayTargetData
        {
            public Ray CurrentRay;
            public bool HasTarget;
            public Vector3 Location;
            public Vector3 Normal;

            public void SetTargetData(Vector3 InLocation, Vector3 InNormal)
            {
                Location = InLocation;
                Normal = InNormal;
                HasTarget = true;
            }

            public void Invalidate()
            {
                HasTarget = false;
            }
        }

        //Ray target data if available
        private RayTargetData RayData;

        //If the cursor is currently being triggered
        private bool bCursorPressed;

        /// <summary>
        /// Line renderer to use when drawing the laser
        /// </summary>
        [Header("Laser")]
        public LineRenderer LaserRenderer;

        /// <summary>
        /// Changes the laser ray distance depending on the target's location
        /// </summary>
        public bool VariableRayDistance = true;

        /// <summary>
        /// Max length for the laser 
        /// </summary>
        public float LaserLength = 1.0f;

        /// <summary>
        /// Line renderer to use when drawing the laser
        /// </summary>
        [Header("Reticle")]
        public MeshRenderer ReticleMesh;

        /// <summary>
        /// Max length for the laser 
        /// </summary>
        public float ReticleMaxDistance = 10.0f;

        /// <summary>
        /// Scale of the reticle when inactive
        /// </summary>
        public float ReticleInactiveScale = 0.01f;

        /// <summary>
        /// Color to lerp when the reticle is inactive
        /// </summary>
        public Color ReticleInactiveColor = new Color(1.0f, 1.0f, 1.0f, 0.25f);

        /// <summary>
        /// Scale of the reticle when active on top of a interatable button
        /// </summary>
        public float ReticleActiveScale = 0.075f;

        /// <summary>
        /// Color to lerp when the reticle is inactive
        /// </summary>
        public Color ReticleActiveColor = new Color(1.0f, 1.0f, 1.0f, 0.75f);
        
        /// <summary>
        /// Color to lerp when the controller is triggered and held 
        /// </summary>
        public Color ReticlePressedColor = new Color(0.25f, 0.28f, 0.55f, 0.75f);

        /// <summary>
        /// Scale of the reticle when inactive
        /// </summary>
        [Range(0.0f, 1.0f)]
        public float ReticleAttributeChangeSpeed = 0.1f;

        /// <summary>
        /// Scale of the reticle when inactive
        /// </summary>
        public int ReticleSortingOrder = 32767;

        void Start()
        {
            if(ReticleMesh)
            {
                ReticleMesh.sortingOrder = ReticleSortingOrder;
            }

            if(LaserRenderer)
            {
                LaserRenderer.startColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                LaserRenderer.endColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }

        /// <summary>
        /// Late update so we render the real position of the laser
        /// </summary>
        private void LateUpdate()
        {
            if(LaserRenderer)
            {
                LaserRenderer.SetPosition(0, RayData.CurrentRay.origin);
                if(VariableRayDistance && RayData.HasTarget)
                {
                    LaserRenderer.SetPosition(1, RayData.Location);
                }
                else
                {
                    LaserRenderer.SetPosition(1, RayData.CurrentRay.origin + RayData.CurrentRay.direction * LaserLength);
                }
            }

            if(ReticleMesh)
            {
                if(RayData.HasTarget)
                {
                    Color reticleColor = bCursorPressed ? ReticlePressedColor : ReticleActiveColor;
                    ReticleMesh.transform.position  = RayData.Location;
                    ReticleMesh.transform.localScale = Vector3.Lerp(ReticleMesh.transform.localScale, ReticleActiveScale * Vector3.one, ReticleAttributeChangeSpeed);
                    ReticleMesh.material.color = Color.Lerp(ReticleMesh.material.color, reticleColor, ReticleAttributeChangeSpeed);
                }
                else
                {
                    ReticleMesh.transform.position = RayData.CurrentRay.origin + RayData.CurrentRay.direction * ReticleMaxDistance;
                    ReticleMesh.transform.localScale = Vector3.Lerp(ReticleMesh.transform.localScale, ReticleInactiveScale * Vector3.one, ReticleAttributeChangeSpeed);
                    ReticleMesh.material.color = Color.Lerp(ReticleMesh.material.color, ReticleInactiveColor, ReticleAttributeChangeSpeed);
                }
            }
        }


        public override void SetRay(Ray ray)
        {
            RayData.CurrentRay = ray;
            RayData.Invalidate();
        }

        public override void SetRayHit(Vector3 Location, Vector3 Normal)
        {
            RayData.SetTargetData(Location, Normal);
        }

        public override void SetButtonPressed(bool bPressed)
        {
            bCursorPressed = bPressed;
        }
    }
}
