﻿using System.Runtime.Serialization;
using NewHorizons.Utility;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NewHorizons.External.Modules
{
    [JsonObject]
    public class PropModule 
    {
        /// <summary>
        /// Scatter props around this planet's surface
        /// </summary>
        public ScatterInfo[] Scatter;
        
        /// <summary>
        /// Place props in predefined positions on the planet
        /// </summary>
        public DetailInfo[] Details;
        
        /// <summary>
        /// Add rafts to this planet
        /// </summary>
        public RaftInfo[] Rafts;
        
        /// <summary>
        /// Add Geysers to this planet
        /// </summary>
        public GeyserInfo[] Geysers;
        
        /// <summary>
        /// Add tornadoes to this planet
        /// </summary>
        public TornadoInfo[] Tornados;
        
        /// <summary>
        /// Add volcanoes to this planet
        /// </summary>
        public VolcanoInfo[] Volcanoes;
        
        /// <summary>
        /// Add dialogue triggers to this planet
        /// </summary>
        public DialogueInfo[] Dialogue;
        
        /// <summary>
        /// Add triggers that reveal parts of the ship log on this planet
        /// </summary>
        public RevealInfo[] Reveal;
        
        /// <summary>
        /// Add ship log entry locations on this planet
        /// </summary>
        public EntryLocationInfo[] EntryLocation;
        
        /// <summary>
        /// Add translatable text to this planet
        /// </summary>
        public NomaiTextInfo[] NomaiText;
        
        /// <summary>
        /// Add slideshows (from the DLC) to the planet
        /// </summary>
        public ProjectionInfo[] SlideShows;
        
        /// <summary>
        /// Details which will be shown from 50km away. Meant to be lower resolution.
        /// </summary>
        public DetailInfo[] ProxyDetails;

        [JsonObject]
        public class ScatterInfo
        {
            /// <summary>
            /// The number used as entropy for scattering the props
            /// </summary>
            public int seed;
            
            /// <summary>
            /// Number of props to scatter
            /// </summary>
            public int count;
            
            /// <summary>
            /// Either the path in the scene hierarchy of the item to copy or the path to the object in the supplied asset bundle
            /// </summary>
            public string path;
            
            /// <summary>
            /// Relative filepath to an asset-bundle"
            /// </summary>
            public string assetBundle;
            
            /// <summary>
            /// Offset this prop once it is placed
            /// </summary>
            public MVector3 offset;
            
            /// <summary>
            /// Rotate this prop once it is placed
            /// </summary>
            public MVector3 rotation;
            
            /// <summary>
            /// Scale this prop once it is placed
            /// </summary>
            public float scale = 1f;
        }

        [JsonObject]
        public class DetailInfo
        {
            /// <summary>
            /// Either the path in the scene hierarchy of the item to copy or the path to the object in the supplied asset bundle
            /// </summary>
            public string path;
            
            /// <summary>
            /// [DEPRECATED] Path to the .obj file to load a 3d model from
            /// </summary>
            public string objFilePath;
            
            /// <summary>
            /// [DEPRECATED] Path to the .mtl file to load a 3d model from
            /// </summary>
            public string mtlFilePath;
            
            /// <summary>
            /// Relative filepath to an asset-bundle to load the prefab defined in `path` from/
            /// </summary>
            public string assetBundle;
            
            /// <summary>
            /// Position of this prop relative to the body's center
            /// </summary>
            public MVector3 position;
            
            /// <summary>
            /// Rotate this prop
            /// </summary>
            public MVector3 rotation;
            
            /// <summary>
            /// Scale the prop
            /// </summary>
            [DefaultValue(1f)]
            public float scale = 1f;
            
            /// <summary>
            /// Do we override rotation and try to automatically align this object to stand upright on the body's surface?
            /// </summary>
            public bool alignToNormal;
            
            /// <summary>
            /// A list of children to remove from this detail
            /// </summary>
            public string[] removeChildren;
            
            /// <summary>
            /// Do we reset all the components on this object? Useful for certain props that have dialogue components attached to them.
            /// </summary>
            public bool removeComponents;
        }

        [JsonObject]
        public class RaftInfo
        {
            /// <summary>
            /// Position of the raft
            /// </summary>
            public MVector3 position;
        }

        [JsonObject]
        public class GeyserInfo
        {
            /// <summary>
            /// Position of the geyser
            /// </summary>
            public MVector3 position;
        }

        [JsonObject]
        public class TornadoInfo
        {
            /// <summary>
            /// Position of the tornado
            /// </summary>
            public MVector3 position;
            
            /// <summary>
            /// Alternative to setting the position. Will choose a random place at this elevation.
            /// </summary>
            public float elevation;
            
            /// <summary>
            /// The height of this tornado.
            /// </summary>
            [DefaultValue(30f)]
            public float height = 30f;
            
            /// <summary>
            /// The colour of the tornado.
            /// </summary>
            public MColor tint;
            
            /// <summary>
            /// Should it pull things downwards? Will push them upwards by default.
            /// </summary>
            public bool downwards;
            
            /// <summary>
            /// The rate at which the tornado will wander around the planet. Set to 0 for it to be stationary. Should be around 0.1.
            /// </summary>
            public float wanderRate;

            /// <summary>
            /// Angular distance from the starting position that it will wander, in terms of the angle around the x-axis.
            /// </summary>
            [DefaultValue(45f)]
            public float wanderDegreesX = 45f;
            
            /// <summary>
            /// Angular distance from the starting position that it will wander, in terms of the angle around the z-axis.
            /// </summary>
            [DefaultValue(45f)]
            public float wanderDegreesZ = 45f;
        }

        [JsonObject]
        public class VolcanoInfo
        {
            /// <summary>
            /// Position of this volcano
            /// </summary>
            public MVector3 position;
            
            /// <summary>
            /// Scale of this volcano
            /// </summary>
            public float scale = 1;
            
            /// <summary>
            /// The colour of the meteor's stone.
            /// </summary>
            public MColor stoneTint;
            
            /// <summary>
            /// The colour of the meteor's lava.
            /// </summary>
            public MColor lavaTint;
            
            /// <summary>
            /// Minimum random speed at which meteors are launched.
            /// </summary>
            public float minLaunchSpeed = 50f;
            
            /// <summary>
            /// Maximum random speed at which meteors are launched.
            /// </summary>
            public float maxLaunchSpeed = 150f;
            
            /// <summary>
            /// Minimum time between meteor launches.
            /// </summary>
            public float minInterval = 5f;
            
            /// <summary>
            /// Maximum time between meteor launches.
            /// </summary>
            public float maxInterval = 20f;
        }

        [JsonObject]
        public class DialogueInfo
        {
            /// <summary>
            /// When you enter into dialogue, you will look here.
            /// </summary>
            public MVector3 position;
            
            /// <summary>
            /// Radius of the spherical collision volume where you get the "talk to" prompt when looking at. If you use a remoteTriggerPosition, you can set this to 0 to make the dialogue only trigger remotely.
            /// </summary>
            public float radius = 1f;
            
            /// <summary>
            /// The radius of the remote trigger volume.
            /// </summary>
            public float remoteTriggerRadius;
            
            /// <summary>
            /// Relative path to the xml file defining the dialogue.
            /// </summary>
            public string xmlFile;
            
            /// <summary>
            /// Allows you to trigger dialogue from a distance when you walk into an area.
            /// </summary>
            public MVector3 remoteTriggerPosition;
            
            /// <summary>
            /// Prevents the dialogue from being created after a specific persistent condition is set. Useful for remote dialogue triggers that you want to have happen only once.
            /// </summary>
            public string blockAfterPersistentCondition;
            
            /// <summary>
            /// If this dialogue is meant for a character, this is the relative path from the planet to that character's CharacterAnimController or SolanumAnimController.
            /// </summary>
            public string pathToAnimController;
            
            /// <summary>
            /// If a pathToAnimController is supplied, if you are within this distance the character will look at you. If it is set to 0, they will only look at you when spoken to.
            /// </summary>
            public float lookAtRadius;
        }

        [JsonObject]
        public class RevealInfo
        {
            public enum RevealVolumeType
            {
                [EnumMember(Value = @"enter")]
                Enter = 0,
                
                [EnumMember(Value = @"observe")]
                Observe = 1,
                
                [EnumMember(Value = @"snapshot")]
                Snapshot = 2
            }
            
            /// <summary>
            /// What needs to be done to the volume to unlock the facts
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public RevealVolumeType revealOn = RevealVolumeType.Enter;
            
            /// <summary>
            /// A list of facts to reveal
            /// </summary>
            public string[] reveals;
            
            /// <summary>
            /// The position to place this volume at
            /// </summary>
            public MVector3 position;
            
            /// <summary>
            /// The radius of this reveal volume
            /// </summary>
            public float radius = 1f;
            
            /// <summary>
            /// The max distance the user can be away from the volume to reveal the fact (`snapshot` and `observe` only)
            /// </summary>
            public float maxDistance = -1f; // Snapshot & Observe Only
            
            /// <summary>
            /// The max view angle (in degrees) the player can see the volume with to unlock the fact (`observe` only)
            /// </summary>
            public float maxAngle = 180f; // Observe Only
        }

        [JsonObject]
        public class EntryLocationInfo
        {
            /// <summary>
            /// ID of the entry this location relates to
            /// </summary>
            public string id;
            
            /// <summary>
            /// Whether this location is cloaked
            /// </summary>
            public bool cloaked;
            
            /// <summary>
            /// The position of this entry location
            /// </summary>
            public MVector3 position;
        }

        [JsonObject]
        public class NomaiTextInfo
        {
            public enum NomaiTextType
            {
                [EnumMember(Value = @"wall")]
                Wall = 0,
                
                [EnumMember(Value = @"scroll")]
                Scroll = 1,
                
                [EnumMember(Value = @"Computer")]
                Computer = 2,
                
                [EnumMember(Value = @"Cairn")]
                Cairn = 3,
                
                [EnumMember(Value = @"Recorder")]
                Recorder = 4
            }
            
            /// <summary>
            /// Position of the root of this text
            /// </summary>
            public MVector3 position;
            
            /// <summary>
            /// The normal vector for this object. Used for writing on walls and positioning computers.
            /// </summary>
            public MVector3 normal;
            
            /// <summary>
            /// The euler angle rotation of this object. Not required if setting the normal. Computers and cairns will orient themselves to the surface of the planet automatically.
            /// </summary>
            public MVector3 rotation;
            
            /// <summary>
            /// The type of object this is.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public NomaiTextType type = NomaiTextType.Wall;
            
            /// <summary>
            /// The relative path to the xml file for this object.
            /// </summary>
            public string xmlFile;
            
            /// <summary>
            /// The random seed used to pick what the text arcs will look like.
            /// </summary>
            public int seed; // For randomizing arcs
            
            /// <summary>
            /// Additional information about each arc in the text
            /// </summary>
            public NomaiTextArcInfo[] arcInfo;
        }

        [JsonObject]
        public class NomaiTextArcInfo
        {
            public enum NomaiTextArcType
            {
                [EnumMember(Value = @"adult")]
                Adult = 0,
                
                [EnumMember(Value = @"child")]
                Child = 1,
                
                [EnumMember(Value = @"stranger")]
                Stranger = 2
            }
            
            /// <summary>
            /// The local position of this object on the wall.
            /// </summary>
            public MVector2 position;
            
            /// <summary>
            /// The z euler angle for this arc.
            /// </summary>
            public float zRotation;
            
            /// <summary>
            /// The type of text to display.
            /// </summary>
            public NomaiTextArcType type = NomaiTextArcType.Adult;
        }

        [JsonObject]
        public class ProjectionInfo
        {
            public enum SlideShowType
            {
                [EnumMember(Value = @"slideReel")]
                SlideReel = 0,
                
                [EnumMember(Value = @"autoProjector")]
                AutoProjector = 1
            }
            
            /// <summary>
            /// The position of this slideshow.
            /// </summary>
            public MVector3 position;
            
            /// <summary>
            /// The rotation of this slideshow.
            /// </summary>
            public MVector3 rotation;
            
            /// <summary>
            /// The type of object this is.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public SlideShowType type = SlideShowType.SlideReel;
            
            /// <summary>
            /// The ship log entries revealed after finishing this slide reel.
            /// </summary>
            public string[] reveals;
            
            /// <summary>
            /// The list of slides for this object.
            /// </summary>
            public SlideInfo[] slides;
        }

        [JsonObject]
        public class SlideInfo
        {
            /// <summary>
            /// The path to the image file for this slide.
            /// </summary>
            public string imagePath;

            // SlideBeatAudioModule
            
            /// <summary>
            /// The name of the AudioClip for a one-shot sound when opening the slide.
            /// </summary>
            public string beatAudio;
            
            /// <summary>
            /// The time delay until the one-shot audio
            /// </summary>
            public float beatDelay;

            // SlideBackdropAudioModule
            
            /// <summary>
            /// The name of the AudioClip that will continuously play while watching these slides
            /// </summary>
            public string backdropAudio;
            
            /// <summary>
            /// The time to fade into the backdrop audio
            /// </summary>
            public float backdropFadeTime;

            
            // SlideAmbientLightModule
            
            /// <summary>
            /// Ambient light intensity when viewing this slide.
            /// </summary>
            public float ambientLightIntensity;
            
            /// <summary>
            /// Ambient light range when viewing this slide.
            /// </summary>
            public float ambientLightRange;
            
            /// <summary>
            /// Ambient light colour when viewing this slide.
            /// </summary>
            public MColor ambientLightColor;
            
            /// <summary>
            /// Spotlight intensity modifier when viewing this slide.
            /// </summary>
            public float spotIntensityMod;
            

            // SlidePlayTimeModule
            
            /// <summary>
            /// Play-time duration for auto-projector slides.
            /// </summary>
            public float playTimeDuration;

            
            // SlideBlackFrameModule
            
            /// <summary>
            /// Before viewing this slide, there will be a black frame for this many seconds.
            /// </summary>
            public float blackFrameDuration;

            
            // SlideShipLogEntryModule
            
            /// <summary>
            /// Ship log entry revealed when viewing this slide
            /// </summary>
            public string reveal;
        }
    }
}
