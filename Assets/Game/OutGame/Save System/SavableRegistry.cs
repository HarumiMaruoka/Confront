using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.SaveSystem
{
    public static class SavableRegistry
    {
        private static HashSet<ISavable> _savables = new HashSet<ISavable>();

        public static IEnumerable<ISavable> Savables => _savables;

        public static void Register(ISavable savable)
        {
            _savables.Add(savable);
        }

        public static void Unregister(ISavable savable)
        {
            _savables.Remove(savable);
        }
    }
}