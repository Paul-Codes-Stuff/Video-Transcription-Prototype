namespace VideoSpeechToText.Console.VideoToWavConversion;
public interface IVideoToWavConverter
{
    Task ConvertToWavFileAsync(
        string inputFilePath,
        string outputFilePath,
        CancellationToken cancellationToken = default
        );
}
