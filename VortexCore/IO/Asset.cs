namespace VortexCore
{
    public abstract class Asset
    {
        public string Id { get; internal set; }

        internal abstract void Dispose();
    }
}
