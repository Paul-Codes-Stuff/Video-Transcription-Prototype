namespace VideoSpeechToText.Console.SpeechTranscription;

public interface ISpeechTranscriber
{
    Task<string> TranscribeAsync(
        string filePath,
        string fileName,
        CancellationToken cancellationToken = default
        );
}