using System;
using System.Linq;

namespace MediaPlayer.Library.Business
{
    [Serializable]
    public enum MediaItemTypeEnum
    {
        NotSet = 255,
        Video = 0,
        Song = 1
    }
}
