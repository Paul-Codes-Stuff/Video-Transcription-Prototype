using NAudio.Wave;

namespace VideoSpeechToText.Console.VideoToWavConversion;
public class VideoToWavConverter : IVideoToWavConverter
{
    public async Task ConvertToWavFileAsync(
        string inputFilePath,
        string outputFilePath,
        CancellationToken cancellationToken = default
        )
    {
        var fileBytes = await File.ReadAllBytesAsync(inputFilePath, cancellationToken);

        using (var video = new MediaFoundationReader(inputFilePath))
        {
            using (var memorySteam = new MemoryStream())
            {
                WaveFileWriter.WriteWavFileToStream(memorySteam, video);
                byte[] bytes = memorySteam.ToArray();
                await File.WriteAllBytesAsync(outputFilePath, bytes, cancellationToken);
            }
        }
    }
}
