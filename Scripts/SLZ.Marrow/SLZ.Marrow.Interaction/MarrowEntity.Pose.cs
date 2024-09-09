using System;
 
using SLZ.Marrow.Utilities;
using UnityEngine;

namespace SLZ.Marrow.Interaction
{
    [Serializable]
    public class MarrowEntityPose
    {
        [SerializeField]
        public SimpleTransform[] bodyPoses;
    }

    public partial class MarrowEntity
    {
    }
}