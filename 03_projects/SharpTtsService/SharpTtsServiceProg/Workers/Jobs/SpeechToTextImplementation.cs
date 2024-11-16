// using System.Globalization;
// using Xamarin.Essentials;
//
// public sealed class SpeechToTextImplementation : ISpeechToText
// {
// 	private AVAudioEngine? audioEngine;
// 	private SFSpeechAudioBufferRecognitionRequest? liveSpeechRequest;
// 	private SFSpeechRecognizer? speechRecognizer;
// 	private SFSpeechRecognitionTask? recognitionTask;
// 	public async Task<string> Listen(CultureInfo culture, IProgress<string>? recognitionResult, CancellationToken cancellationToken)
// 	{
// 		speechRecognizer = new SFSpeechRecognizer(NSLocale.FromLocaleIdentifier(culture.Name));
// 		if (!speechRecognizer.Available)
// 		{
// 			throw new ArgumentException("Speech recognizer is not available");
// 		}
// 		if (SFSpeechRecognizer.AuthorizationStatus != SFSpeechRecognizerAuthorizationStatus.Authorized)
// 		{
// 			throw new Exception("Permission denied");
// 		}
// 		audioEngine = new AVAudioEngine();
// 		liveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
// #if MACCATALYST
// 		var audioSession = AVAudioSession.SharedInstance();
// 		audioSession.SetCategory(AVAudioSessionCategory.Record, AVAudioSessionCategoryOptions.DefaultToSpeaker);
// 		var mode = audioSession.AvailableModes.Contains("AVAudioSessionModeMeasurement") ? "AVAudioSessionModeMeasurement" : audioSession.AvailableModes.First();
// 		audioSession.SetMode(new NSString(mode), out var audioSessionError);
// 		if (audioSessionError != null)
// 		{
// 			throw new Exception(audioSessionError.LocalizedDescription);
// 		}
// 		audioSession.SetActive(true, AVAudioSessionSetActiveOptions.NotifyOthersOnDeactivation, out audioSessionError);
// 		if (audioSessionError is not null)
// 		{
// 			throw new Exception(audioSessionError.LocalizedDescription);
// 		}
// #endif
// 		var node = audioEngine.InputNode;
// 		var recordingFormat = node.GetBusOutputFormat(new UIntPtr(0));
// 		node.InstallTapOnBus(new UIntPtr(0), 1024, recordingFormat, (buffer, _) =>
// 		{
// 			liveSpeechRequest.Append(buffer);
// 		});
// 		audioEngine.Prepare();
// 		audioEngine.StartAndReturnError(out var error);
// 		if (error is not null)
// 		{
// 			throw new ArgumentException("Error starting audio engine - " + error.LocalizedDescription);
// 		}
// 		var currentIndex = 0;
// 		var taskResult = new TaskCompletionSource<string>();
// 		recognitionTask = speechRecognizer.GetRecognitionTask(liveSpeechRequest, (result, err) =>
// 		{
// 			if (err != null)
// 			{
// 				StopRecording();
// 				taskResult.TrySetException(new Exception(err.LocalizedDescription));
// 			}
// 			else
// 			{
// 				if (result.Final)
// 				{
// 					currentIndex = 0;
// 					StopRecording();
// 					taskResult.TrySetResult(result.BestTranscription.FormattedString);
// 				}
// 				else
// 				{
// 					for (var i = currentIndex; i < result.BestTranscription.Segments.Length; i++)
// 					{
// 						var s = result.BestTranscription.Segments[i].Substring;
// 						currentIndex++;
// 						recognitionResult?.Report(s);
// 					}
// 				}
// 			}
// 		});
// 		await using (cancellationToken.Register(() =>
// 		             {
// 			             StopRecording();
// 			             taskResult.TrySetCanceled();
// 		             }))
// 		{
// 			return await taskResult.Task;
// 		}
// 	}
// 	void StopRecording()
// 	{
// 		audioEngine?.InputNode.RemoveTapOnBus(new UIntPtr(0));
// 		audioEngine?.Stop();
// 		liveSpeechRequest?.EndAudio();
// 		recognitionTask?.Cancel();
// 	}
// }