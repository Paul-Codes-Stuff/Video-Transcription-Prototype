using Microsoft.Extensions.Configuration;
using VideoSpeechToText.Console.Configuration;
using VideoSpeechToText.Console.SpeechTranscription;
using VideoSpeechToText.Console.VideoToWavConversion;

IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

var azureSpeechServiceConfig = config
    .GetSection("AzureSpeechService")
    .Get<AzureSpeechServiceConfig>();

IVideoToWavConverter videoToWavConverter = new VideoToWavConverter();

ISpeechTranscriber mySpeechRecognizer = new AzureSpeechTranscriber(
    azureSpeechServiceConfig, 
    videoToWavConverter
    );

//string text = await mySpeechRecognizer.TranscribeAsync("C:\\Test", "news.mp4"); // 3m23s BBC news broadcast
string text = await mySpeechRecognizer.TranscribeAsync("C:\\Test", "harvard.wav"); // 18s clear speech
//string text = await mySpeechRecognizer.TranscribeAsync("C:\\Test", "jackhammer.wav"); // 3s with noise in background
Console.WriteLine(text);

