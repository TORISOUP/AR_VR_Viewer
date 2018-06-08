namespace VrmArPlayer
{
    /// <summary>
    /// 設定値とか
    /// </summary>
    public class Configs
    {
        public bool IsVrMode { get; }

        public Configs(bool isVrMode)
        {
            IsVrMode = isVrMode;
        }
    }
}
