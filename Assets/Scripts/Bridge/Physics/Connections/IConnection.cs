namespace BridgeGame
{
    public interface IConnection
    {
        IPoint A { get; }
        IPoint B { get; }
        ConnectionType Type { get; }
        bool Broken { get; }
        float MaxLength { get; }
        
        void Setup(IPoint a, IPoint b, Bridge bridge);
        void StartSimulation();
        void StopSimulation();
        void Remove();
        float Stress();
        void Break();
        void DrawDebug();
        float Weight();
    }
}
