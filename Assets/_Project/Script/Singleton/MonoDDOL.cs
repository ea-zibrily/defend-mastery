﻿using UnityEngine;

namespace Defend.Singleton
{
    public class MonoDDOL<T> : MonoBehaviour where T: MonoBehaviour
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}