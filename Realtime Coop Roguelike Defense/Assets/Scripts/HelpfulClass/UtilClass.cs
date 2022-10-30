using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Litkey.Utility
{
    public static class UtilClass 
    {
        /*
         * <return> returns the angle the direction </return>
         */
        public static float GetAngleFromDirection(Vector3 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        /*
         * <return> returns the quaternion rotation </return>
         */
        public static Quaternion GetRotationFromDirection(Vector3 direction)
        {
            return Quaternion.AngleAxis(GetAngleFromDirection(direction), Vector3.forward);
        }

    }
}
