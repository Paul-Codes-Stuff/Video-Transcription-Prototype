using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using VideoSpeechToText.Console.Configuration;
using VideoSpeechToText.Console.VideoToWavConversion;

namespace VideoSpeechToText.Console.SpeechTranscription;
public class AzureSpeechTranscriber : ISpeechTranscriber
{
    private readonly SpeechConfig _speechConfig;
    private readonly IVideoToWavConverter _videoToWavConverter;
    public AzureSpeechTranscriber(
        AzureSpeechServiceConfig config,
        IVideoToWavConverter videoToWavConverter
        )
    {
        _speechConfig = SpeechConfig.FromSubscription(
            config.Key,
            config.Region
            );

        _videoToWavConverter = videoToWavConverter;
    }

    public async Task<string> TranscribeAsync(
        string filePath,
        string fileName,
        CancellationToken cancellationToken = default
        )
    {
        string fullFilePath = Path.Combine(filePath, fileName);

        bool convertToWavFile = !fullFilePath.EndsWith(".wav", StringComparison.InvariantCultureIgnoreCase);

        if (convertToWavFile)
        {
            string newFilePath = Path.Combine(filePath, $"{Guid.NewGuid().ToString()}.wav");
            await _videoToWavConverter.ConvertToWavFileAsync(fullFilePath, newFilePath);
            fullFilePath = newFilePath;
        }

        AudioConfig audioConfig = AudioConfig.FromWavFileInput(fullFilePath);
        return await RecogniseSpeechAsync(audioConfig);
    }

    private async Task<string> RecogniseSpeechAsync(
        AudioConfig audioConfig
        )
    {

        SpeechRecognizer speechRecognizer = new(_speechConfig, audioConfig);

        var stopRecognition = new TaskCompletionSource<int>();
        string text = string.Empty;

        speechRecognizer.Recognizing += (s, e) =>
        {
            // no need to do anything here
        };

        speechRecognizer.Recognized += (s, e) =>
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                text = text + e.Result.Text + " ";
            }
            else if (e.Result.Reason == ResultReason.NoMatch)
            {
                // ignore it
            }
        };

        speechRecognizer.Canceled += (s, e) =>
        {
            stopRecognition.TrySetResult(0);
        };

        speechRecognizer.SessionStopped += (s, e) =>
        {
            stopRecognition.TrySetResult(0);
        };

        await speechRecognizer.StartContinuousRecognitionAsync();

        // Waits for completion. Use Task.WaitAny to keep the task rooted.
        Task.WaitAny(new[] { stopRecognition.Task });

        return text.Trim();
    }
}
