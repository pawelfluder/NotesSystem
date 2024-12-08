namespace SharpImageSplitterTests.Helper;

internal class SettingsProvider
{
    public (float heightByWidth, int overlap) GetRatioTuple(string type)
    {
        // iPhone 11 Pro Max iOS 14.6
        // 414 x 896 -> 450 x 900

        if (type == "tinder2")
        {
            var ratio = (2.0f, 5);
            return ratio;
        }

        if (type == "tinder")
        {
            var ratio = (2.5f, 5);
            return ratio;
        }

        if (type == "instagram")
        {
            // instagram
            // 450 = 109 + 341
            // 559 = 109 + 450
            // 559 x 900
            var ratio = (2.0f, 5);
            return ratio;
        }

        return default;
    }

    public (string, string ) GetPathTuple()
    {
        var imageFolderPath = "N:\\01_files_refrenced\\private\\01_files_photos\\03_messages\\24-05-15_tf_Roksana_Chołdyk";
        var imageFileName = "02_tinder.png";

        return (imageFolderPath, imageFileName);

        //var imageFolderPath = "/Users/pawelfluder/03_synch/02_files_refrenced/01_files_photos/10_messages/24-03-20_ti_Ola_Konopka";
        //var imageFileName = "01_oryginal.png";

        // vat-r
        // var imageFolderPath = @"D:\03_synch\03_files_refrenced\2023\03_files_photos\16_urząd_skarbowy";
        // var imageFileName = "24-03-04_urzad-skarbowy_vat-r.png";

        // vat-r
        // var imageFolderPath = @"D:\03_synch\03_files_refrenced\2023\03_files_photos\16_urząd_skarbowy";
        // var imageFileName = "24-03-04_urzad-skarbowy_vat-r.png";

        // tinder
        // var imageFolderPath = D:/03_synch/03_files_refrenced/2023/04_files_photos/10_tinder/01_messages/02_pawel-fluder/03_closed/23-02-20_tf_weronika_22
        // var imageFileName = "01_oryginal";

        //var imageFolderPath = @"D:\03_synch\03_files_refrenced\2023\04_files_photos\01_private\10_tinder\01_messages\02_pawel-fluder\03_closed\24-01-11_ti_Patrycja";
        //var imageFileName = "01_item.png";

        // D:\03_synch\03_files_refrenced\2023\03_files_photos\14_tinder\24-02-09_tx_Natalia

        //var imageFolderPath = @"D:\03_synch\03_files_refrenced\2023\03_files_photos\14_tinder\24-02-09_tx_Natalia";
        //var imageFileName = "01_oryginal.png";

        //var imageFolderPath = @"D:\03_synch\03_files_refrenced\2023\03_files_photos\16_urząd_skarbowy";
        //var imageFileName = "24-03-04_urzad-skarbowy_vat-r.png";
    }
}