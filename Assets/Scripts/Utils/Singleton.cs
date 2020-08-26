﻿using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get { return instance; }
        }

        public static bool IsInitialized
        {
            get { return instance != null; }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T) this;
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }   
        }
    }
}