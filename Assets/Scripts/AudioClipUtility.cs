using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class AudioClipUtility
{
    const int HEADER_SIZE = 44;

    /// <summary>
    /// Saves an AudioClip as a .wav file.
    /// </summary>
    public static bool SaveAsWav(string filename, AudioClip clip)
    {
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }

        string filepath = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log($"Saving audio to: {filepath}");

        try
        {
            using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
            {
                WriteWavHeader(fileStream, clip);
                WriteWavData(fileStream, clip);
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save .wav file: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Trims silence from an AudioClip.
    /// </summary>
    public static AudioClip TrimSilence(AudioClip clip, float threshold, bool isStream = false)
    {
        if (clip == null) return null;

        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        int startIndex = 0;
        int endIndex = samples.Length - 1;

        // Find start of meaningful audio
        for (int i = 0; i < samples.Length; i++)
        {
            if (Mathf.Abs(samples[i]) > threshold)
            {
                startIndex = i;
                break;
            }
        }

        // Find end of meaningful audio
        for (int i = samples.Length - 1; i >= 0; i--)
        {
            if (Mathf.Abs(samples[i]) > threshold)
            {
                endIndex = i;
                break;
            }
        }

        // Extract meaningful samples
        int length = endIndex - startIndex + 1;
        if (length <= 0) return null;

        float[] trimmedSamples = new float[length];
        Array.Copy(samples, startIndex, trimmedSamples, 0, length);

        // Create new AudioClip
        AudioClip trimmedClip = AudioClip.Create("TrimmedClip", length / clip.channels, clip.channels, clip.frequency, isStream);
        trimmedClip.SetData(trimmedSamples, 0);

        return trimmedClip;
    }

    /// <summary>
    /// Writes the WAV header.
    /// </summary>
    private static void WriteWavHeader(FileStream fileStream, AudioClip clip)
    {
        int sampleCount = clip.samples;
        int channelCount = clip.channels;
        int sampleRate = clip.frequency;

        fileStream.Seek(0, SeekOrigin.Begin);

        // RIFF header
        fileStream.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, 4);
        fileStream.Write(BitConverter.GetBytes(HEADER_SIZE + sampleCount * channelCount * 2 - 8), 0, 4);
        fileStream.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, 4);

        // fmt subchunk
        fileStream.Write(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, 4);
        fileStream.Write(BitConverter.GetBytes(16), 0, 4); // Subchunk size
        fileStream.Write(BitConverter.GetBytes((ushort)1), 0, 2); // Audio format (PCM)
        fileStream.Write(BitConverter.GetBytes((ushort)channelCount), 0, 2);
        fileStream.Write(BitConverter.GetBytes(sampleRate), 0, 4);
        fileStream.Write(BitConverter.GetBytes(sampleRate * channelCount * 2), 0, 4); // Byte rate
        fileStream.Write(BitConverter.GetBytes((ushort)(channelCount * 2)), 0, 2); // Block align
        fileStream.Write(BitConverter.GetBytes((ushort)16), 0, 2); // Bits per sample

        // data subchunk
        fileStream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);
        fileStream.Write(BitConverter.GetBytes(sampleCount * channelCount * 2), 0, 4);
    }

    /// <summary>
    /// Writes WAV data.
    /// </summary>
    private static void WriteWavData(FileStream fileStream, AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        byte[] bytesData = new byte[samples.Length * 2];

        int rescaleFactor = 32767; // Convert float to Int16

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            byte[] byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);
    }
}