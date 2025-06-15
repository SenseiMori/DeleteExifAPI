namespace AppCore.Model.Interfaces
{
    public interface IMainViewModel
    {
        bool IsResize { get; }
        bool IsCompress { get; }
        bool IsRemove { get; set; }
        bool IsBestResolution { get; set; }
        bool IsNormalResolution { get; set; }
        bool IsExtraResolution { get; set; }
        bool IsBestCompress { get; set; }
        bool IsNormalCompress { get; set; }
        bool IsExtraCompress { get; set; }
        public bool IsOpenDirectory { get; set; }

    }
}
