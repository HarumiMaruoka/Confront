using System;
using UnityEngine;

namespace NexEditor
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ExpandableAttribute : PropertyAttribute
    {
        public ExpandableAttribute()
        {

        }
    }
}