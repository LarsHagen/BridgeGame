using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BridgeGame
{
    public interface IConnection
    {
        IPoint A { get; }
        IPoint B { get; }
        bool Broken { get; }

        void Setup(IPoint a, IPoint b, Bridge bridge);
        void StartSimulation();
        void StopSimulation();
        void Remove();
        float Stress();
        void Break();
        void DrawDebug();
    }
}
