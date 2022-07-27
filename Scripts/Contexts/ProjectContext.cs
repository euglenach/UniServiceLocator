using System;
using UnityEngine;

namespace UniServiceLocator
{
    [DefaultExecutionOrder(-9999)]
    public class ProjectContext : MonoBehaviour
    {
        public Locator Locator{get;private set;} = new Locator();
        
        private static ProjectContext inst;
        public ProjectContext Inst
        {
            get
            {
                if(inst == null) inst = FindObjectOfType<ProjectContext>();
                if(inst == null) inst = new GameObject("ProjectContext").AddComponent<ProjectContext>();
                return inst;
            }
        }
        
        private void Awake()
        {
            if(!Application.isPlaying) return;
            
            if(inst == null)
            {
                inst = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            
            Destroy(gameObject);
        }

    }
}
