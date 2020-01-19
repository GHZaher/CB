using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    public class BallsName: MonoBehaviour
    {
        [SerializeField] private BallNamesEnum ballName;

        public enum BallNamesEnum
        {
            Default,
            Gear,
            Tire,
            Vortex,
            Only,
            Pattern,
        }
    }
}