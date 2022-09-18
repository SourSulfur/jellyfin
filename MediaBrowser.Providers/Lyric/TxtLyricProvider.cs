using System.Collections.Generic;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Lyrics;
using MediaBrowser.Controller.Resolvers;

namespace MediaBrowser.Providers.Lyric;

/// <summary>
/// TXT Lyric Provider.
/// </summary>
public class TxtLyricProvider : ILyricProvider
{
    /// <inheritdoc />
    public string Name => "TxtLyricProvider";

    /// <summary>
    /// Gets the priority.
    /// </summary>
    /// <value>The priority.</value>
    public ResolverPriority Priority => ResolverPriority.Second;

    /// <inheritdoc />
    public IReadOnlyCollection<string> SupportedMediaTypes { get; } = new[] { "lrc", "txt" };

    /// <summary>
    /// Opens lyric file for the requested item, and processes it for API return.
    /// </summary>
    /// <param name="item">The item to to process.</param>
    /// <returns>If provider can determine lyrics, returns a <see cref="LyricResponse"/>; otherwise, null.</returns>
    public LyricResponse? GetLyrics(BaseItem item)
    {
        string? lyricFilePath = LyricInfo.GetLyricFilePath(this, item.Path);

        if (string.IsNullOrEmpty(lyricFilePath))
        {
            return null;
        }

        string[] lyricTextLines = System.IO.File.ReadAllLines(lyricFilePath);

        List<LyricLine> lyricList = new List<LyricLine>();

        if (lyricTextLines.Length == 0)
        {
            return null;
        }

        foreach (string lyricTextLine in lyricTextLines)
        {
            lyricList.Add(new LyricLine(lyricTextLine));
        }

        return new LyricResponse { Lyrics = lyricList };
    }
}
