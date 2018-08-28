using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXGame
{
    public class LuaUtil
    {
        public static int Int(object o)
        {
            return Convert.ToInt32(o);
        }

        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o)
        {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }


        public static UnityEngine.Object LoadInstantiate(string path)
        {
            return FXGame.Managers.ResourceManager.Instance.LoadInstantiate<UnityEngine.Object>(path);
        }

		/// <summary>
		/// 对比两个时间差距多少天
		/// </summary>
		/// <returns>The differ form date.</returns>
		/// <param name="year1">Year1.</param>
		/// <param name="month1">Month1.</param>
		/// <param name="day1">Day1.</param>
		/// <param name="year2">Year2.</param>
		/// <param name="month2">Month2.</param>
		/// <param name="day2">Day2.</param>
		public static int GetDifferFormDate(int year1,int month1,int day1,int year2,int month2,int day2)
		{
			DateTime d1 = new DateTime(year1,month1,day1);
			DateTime d2 = new DateTime(year2,month2,day2);
			TimeSpan ts = d2 - d1;
			return ts.Days;
		}
    }

}

