using System.IO;

namespace VctoonCore.Resources;

public class Video : BaseResource
{
    public double Framerate { get; protected set; }
    public string Codec { get; protected set; }
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public long Bitrate { get; protected set; }
    public string Ratio { get; protected set; }
    public TimeSpan Duration { get; protected set; }
    public string Path { get; protected set; }


    public virtual void SetPath(string path)
    {
        Check.NotNullOrWhiteSpace(path, nameof(path));

        if (!File.Exists(path))
            throw new Exception($@"{nameof(path)} is not valid path. {path} is not exist.");

        Path = path;
    }
    public void SetFramerate(double framerate)
    {
        Framerate = framerate;
    }

    public void SetCodec(string codec)
    {
        Check.NotNullOrWhiteSpace(codec, nameof(codec));
        Codec = codec;
    }

    public void SetWidth(int width)
    {
        Check.Range(width, nameof(width), 1, int.MaxValue);
        Width = width;
    }

    public void SetHeight(int height)
    {
        Check.Range(height, nameof(height), 1, int.MaxValue);
        Height = height;
    }

    public void SetBitrate(long bitrate)
    {
        Check.Range(bitrate, nameof(bitrate), 1, long.MaxValue);
        Bitrate = bitrate;
    }

    public void SetRatio(string ratio)
    {
        Check.NotNullOrWhiteSpace(ratio, nameof(ratio));
        Ratio = ratio;
    }

    public void SetDuration(TimeSpan duration)
    {
        Duration = duration;
    }



}
