using NewHorizons.External.Modules.Audio;
using NewHorizons.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NewHorizons.External.Modules.Volumes
{
    [JsonObject]
    public class VolumesModule
    {
        /// <summary>
        /// Add audio volumes to this planet.
        /// </summary>
        public AudioVolumeInfo[] audioVolumes;

        /// <summary>
        /// Add destruction volumes to this planet.
        /// </summary>
        public DestructionVolumeInfo[] destructionVolumes;

        /// <summary>
        /// Add fluid volumes to this planet.
        /// </summary>
        public FluidVolumeInfo[] fluidVolumes;

        /// <summary>
        /// Add hazard volumes to this planet.
        /// </summary>
        public HazardVolumeInfo[] hazardVolumes;

        /// <summary>
        /// Add interference volumes to this planet.
        /// </summary>
        public VolumeInfo[] interferenceVolumes;

        /// <summary>
        /// Add insulating volumes to this planet. These will stop electricty hazard volumes from affecting you (just like the jellyfish).
        /// </summary>
        public VolumeInfo[] insulatingVolumes;

        /// <summary>
        /// Add light source volumes to this planet. These will activate rafts and other light detectors.
        /// </summary>
        public VolumeInfo[] lightSourceVolumes;

        /// <summary>
        /// Add map restriction volumes to this planet.
        /// </summary>
        public VolumeInfo[] mapRestrictionVolumes;

        /// <summary>
        /// Add notification volumes to this planet.
        /// </summary>
        public NotificationVolumeInfo[] notificationVolumes;

        /// <summary>
        /// Add oxygen volumes to this planet.
        /// </summary>
        public OxygenVolumeInfo[] oxygenVolumes;

        /// <summary>
        /// Add probe-specific volumes to this planet.
        /// </summary>
        public ProbeModule probe;

        /// <summary>
        /// Add reference frame blocker volumes to this planet. These will stop the player from seeing/targeting any reference frames.
        /// </summary>
        public VolumeInfo[] referenceFrameBlockerVolumes;

        /// <summary>
        /// Add triggers that reveal parts of the ship log on this planet.
        /// </summary>
        public RevealVolumeInfo[] revealVolumes;

        /// <summary>
        /// Add reverb volumes to this planet. Great for echoes in caves.
        /// </summary>
        public VolumeInfo[] reverbVolumes;

        /// <summary>
        /// Add ruleset volumes to this planet.
        /// </summary>
        public RulesetModule rulesets;

        /// <summary>
        /// Add speed trap volumes to this planet. Slows down the player when they enter this volume.
        /// </summary>
        public SpeedTrapVolumeInfo[] speedTrapVolumes;

        /// <summary>
        /// Add visor effect volumes to this planet.
        /// </summary>
        public VisorEffectModule visorEffects;

        /// <summary>
        /// Add zero-gravity volumes to this planet. 
        /// Good for surrounding planets which are using a static position to stop the player being pulled away.
        /// </summary>
        public PriorityVolumeInfo[] zeroGravityVolumes;

        /// <summary>
        /// Entering this volume will load a new solar system.
        /// </summary>
        public ChangeStarSystemVolumeInfo[] solarSystemVolume;

        /// <summary>
        /// Enter this volume to be sent to the end credits scene
        /// </summary>
        public LoadCreditsVolumeInfo[] creditsVolume;
    }
}
