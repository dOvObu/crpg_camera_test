
using UnityEngine;

namespace DefaultNamespace
{
    public enum VecParam
    {
        X = 0,
        Y,
        Z,
        W
    }
    public static class MathUtils
    {
        public static Vector3 With(this Vector3 self, float num, VecParam @in)
        {
            self[(int)@in] = num;
            return self;
        }

        public static Vector3 ToVec3(this Vector2 self, VecParam x, VecParam y)
        {
            var res = Vector3.zero;
            res[(int)x] = self.x;
            res[(int)y] = self.y;
            return res;
        }

        public static Quaternion With(this Quaternion self, float num, VecParam @in)
        {
            return Quaternion.Euler(self.eulerAngles.With(num, @in));
        }
    }
}