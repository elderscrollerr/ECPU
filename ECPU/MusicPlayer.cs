using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECPU
{
public static class MusicPlayer
    {
       public static bool MUSIC_IS_STOPPED = true;

       private static System.Media.SoundPlayer player;
        public static bool STOPPED_MANUALLY;

        public static void Play()
        {
            if (File.Exists(STYLE.MUSIC_FILE_PATH))
            {
                if (player == null)
                {
                    player = new System.Media.SoundPlayer();
                    player.SoundLocation = STYLE.MUSIC_FILE_PATH;

                }

                if (MUSIC_IS_STOPPED)
                {
                    player.PlayLooping();
                    MUSIC_IS_STOPPED = false;
                }
            }
            
        }
        public static void Stop()
        {
            if (player!=null)
            {
                if (!MUSIC_IS_STOPPED)
                {
                    player.Stop();
                    MUSIC_IS_STOPPED = true;


                }
            }
            
        }
    
        public static bool isPlaying()
        {
            return !MUSIC_IS_STOPPED;
        }

    }
}
