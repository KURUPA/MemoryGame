namespace MemoryGame;

/// <summary>
/// Managerlistener 介面定義了在遊戲管理器 (Manager) 和其他元件之間進行通信的方法。
/// </summary>
public interface IManagerListener
{
    /// <summary>
    /// 當選擇了一個歌曲標題並判斷是否匹配時調用的方法。
    /// </summary>
    /// <param name="songTitle">所選的歌曲標題</param>
    /// <param name="match">是否匹配</param>
    void PickSongTitle(SongTitle songTitle, bool match);
}