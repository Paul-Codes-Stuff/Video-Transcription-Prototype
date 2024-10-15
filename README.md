# Video-Transcription-Prototype
Source code for blog article at:
https://paulcodesstuff.com/blog/coding/video-transcription-in-dotnet-with-azure-ai-speech-services

This requires an Azure account with Azure Speech Service.

Add the Speech Service key and region to your user secrets:
```
{
  "AzureSpeechService": {
    "Key": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "Region": "uksouth"
  }
}
```
You will also need some files to transcribe. Sample audio files can be found on [Kaggle](https://www.kaggle.com/datasets/pavanelisetty/sample-audio-files-for-speech-recognition).
