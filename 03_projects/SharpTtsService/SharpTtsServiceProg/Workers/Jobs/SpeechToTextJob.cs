// namespace SharpTtsServiceProg.Workers.Jobs;
//
// public class SpeechToTextJob
// {
//     async Task Listen(CancellationToken cancellationToken)
//     {
//         var isGranted = await speechToText.RequestPermissions(cancellationToken);
//         if (!isGranted)
//         {
//             await Toast.Make("Permission not granted").Show(CancellationToken.None);
//             return;
//         }
//
//         var recognitionResult = await speechToText.ListenAsync(
//             CultureInfo.GetCultureInfo(Language),
//             new Progress<string>(partialText =>
//             {
//                 RecognitionText += partialText + " ";
//             }), cancellationToken);
//
//         if (recognitionResult.IsSuccessful)
//         {
//             RecognitionText = recognitionResult.Text;
//         }
//         else
//         {
//             await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech").Show(CancellationToken.None);
//         }
//     }
// }