﻿using System;
using UnityEngine;

namespace UniServiceLocator
{
    [DefaultExecutionOrder(-10000)]
    public class ProjectContext : MonoBehaviour
    {
        private readonly ServiceLocator serviceLocator = new ServiceLocator();
        
        private static ProjectContext _inst;

        /// <summary>
        /// Singleton
        /// </summary>
        private static ProjectContext inst
        {
            get
            {
                if(_inst == null)
                {
                    var prev = FindObjectOfType<ProjectContext>();
                    _inst = prev == null? new GameObject(nameof(ProjectContext)).AddComponent<ProjectContext>() : prev;
                    DontDestroyOnLoad(_inst.gameObject);
                }

                return _inst;
            }
        }

        /// <summary>
        /// Project Context ServiceLocator
        /// </summary>
        public static IServiceLocator Locator => inst.serviceLocator;
        
        private void Awake()
        {
            if(_inst == null)
            {
                _inst = this;
                return;
            }
            
            Destroy(gameObject);
        }

    }
}
