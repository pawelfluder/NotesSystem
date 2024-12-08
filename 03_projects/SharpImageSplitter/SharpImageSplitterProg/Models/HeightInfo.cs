namespace SharpImageSplitterProg.Models;

public record HeightInfo(
    int YSHC_StartPosOfHeightCrop,
    int YSHM_StartPosOfHeightMiddle,
    int YEHM_EndPosOfHeightMiddle,
    int YEHC_EndPosOfHeightCrop,
    int StrapTop,
    int StrapBottom,
    int HeightCrop,
    int HeightMiddle)
{
    public override string ToString()
    {
        string result = string.Format(
            "HInfo [ Y_SHC={0}; Y_SHM={1}; Y_EHM={2}; Y_EHC={3}; ST={4}; SB={5}; HM={6}; HC={7}; ]",
            YSHC_StartPosOfHeightCrop,
            YSHM_StartPosOfHeightMiddle,
            YEHM_EndPosOfHeightMiddle,
            YEHC_EndPosOfHeightCrop,
            StrapTop,
            StrapBottom,
            HeightMiddle,
            HeightCrop);
        
        return result;
    }
}
