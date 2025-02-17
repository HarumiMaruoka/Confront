using INab.Dissolve;
using System;
using System.Collections;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public class LeviathanDisslver : MonoBehaviour
    {
        public DissolveState dissolveState;
        public Dissolver dissolver;

        private void OnEnable()
        {
            switch (dissolveState)
            {
                case DissolveState.Dissolv:
                    BeginDisslve();
                    break;
                case DissolveState.Materialize:
                    BeginMaterialize();
                    break;
            }
        }

        public void BeginDisslve()
        {
            dissolver.Dissolve();
        }

        public void BeginMaterialize()
        {
            dissolver.Materialize();
        }
    }

    public enum DissolveState
    {
        None,
        Dissolv,
        Materialize
    }
}