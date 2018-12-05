using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apprentice_learncsharp_2018_12.Blockbuster
{
    public class PrepareVideo
    {
        public void SeekMovieStart(VHSMachine vhsMachine)
        {
            // rewind to the beginning
            var hasRewoundToBeginning = false;
            do
            {
                hasRewoundToBeginning = vhsMachine.Rewind(TimeSpan.FromMinutes(1));
            } while (!hasRewoundToBeginning);
            // start playing the video
            vhsMachine.Play();

            // fastforward bit by bit to find blockbusters logo
            while (!NotAtBlockbusterLogo(vhsMachine))
            {
                vhsMachine.FastForward(TimeSpan.FromSeconds(5));
            }

            // Stop the machine!
            vhsMachine.Stop();
        }
        private bool NotAtBlockbusterLogo(VHSMachine vhsMachine)
        {
            var currentPic = vhsMachine.CurrentImage;

            // examine the current pic to see if it is the blockbuster logo
            return true;
        }
    }
}
