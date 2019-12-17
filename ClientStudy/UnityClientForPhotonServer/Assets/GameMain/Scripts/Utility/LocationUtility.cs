/*
 * Author:     NOW
 * CreateTime: 20171218
 * Description: 地理位置实用类
 * 
*/
using System;
using UnityEngine;
using System.Collections;
using System.Timers;
using GameFramework;

namespace StarForce
{
    public class LocationUtility
    {
        private static bool m_IsLocationServiceRunning = false;

        /// <summary>
        /// 上次位置信息
        /// </summary>
        private static LocationInfo m_LastLocationData;

        /// <summary>
        /// 精确度，精确到10米为一个单位
        /// </summary>
        public static float m_DesiredAccuracyInMeters = 10;
        /// <summary>
        /// 更新距离，默认每10米更新一次
        /// </summary>
        public static float m_UpdateDistanceInMeters = 10;

        public static void StartService(CallBack<LocationInfo> luaCallback,float waitTime = 15, float desiredAccuracyInMeters = 10, float updateDistanceInMeters = 10)
        {
            //非手持设备则终止
            if (!Application.isMobilePlatform)
            {
                Log.Info("LocationService can not work on non-mobile platform.");
                return;
            }

            m_DesiredAccuracyInMeters = desiredAccuracyInMeters;
            m_UpdateDistanceInMeters = updateDistanceInMeters;

            if (!m_IsLocationServiceRunning)
            {
                GetLocationData(luaCallback, waitTime);
            }
        }

        public static void GetLocationData(CallBack<LocationInfo> luaCallback,float waitTime)
        {
            if (Input.location.isEnabledByUser)
            {
                Log.Info("GPS not available");
            }

            Task task = new Task(ActivateGPS(waitTime), true);
            task.Finished += delegate (bool manual)
            {
                if (luaCallback != null)
                {
                    luaCallback.DynamicInvoke(m_LastLocationData);
                }
            };
        }

        static IEnumerator ActivateGPS(float waitTime)
        {
            m_IsLocationServiceRunning = true;

            Input.location.Start(m_DesiredAccuracyInMeters, m_UpdateDistanceInMeters);
            Log.Info("Location service status = " + Input.location.status);

            float duration = 0;
            while (duration < waitTime)
            {
                Log.Info("=" + duration + " , " + Input.location.status);
                if (Input.location.status == LocationServiceStatus.Running || Input.location.status == LocationServiceStatus.Failed)
                {
                    break;
                }
                yield return new WaitForSeconds(1.0f);
                duration += 1.0f;
            }
            if (duration >= waitTime)
            {
                Log.Info("Get location data timed out.");
            }
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Log.Info("Get location data failed.");
                m_IsLocationServiceRunning = false;
            }
            else if (Input.location.status == LocationServiceStatus.Running)
            {
                Log.Info("Get location data success.");
                m_LastLocationData = Input.location.lastData;
            }
        }

        public static void StopService()
        {
            Input.location.Stop();
            m_IsLocationServiceRunning = false;
        }
    }
}