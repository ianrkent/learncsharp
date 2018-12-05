using System;
using System.Runtime.InteropServices.ComTypes;

namespace ConsoleApp.Blockbuster
{

    // Todo: Introduce a BetaMachine, but re-use the logic in SeekMovieStart by introducing a Generic
    public class PrepareVideo
    {
        public void SeekMovieStart(VHSMachine machine) 
        {
            // rewind to the beginning
            var hasRewoundToBeginning = false;
            do
            {
                hasRewoundToBeginning = machine.Rewind(TimeSpan.FromMinutes(1));
            } while (!hasRewoundToBeginning);

            // start playing the video
            machine.Play();

            // fastforward bit by bit to find blockbusters logo
            while (!NotAtBlockbusterLogo(machine))
            {
                machine.FastForward(TimeSpan.FromSeconds(5));
            }

            // Stop the machine!
            machine.Stop();
        }

        private bool NotAtBlockbusterLogo(VHSMachine vhsMachine)
        {
            var currentPic = vhsMachine.CurrentImage;

            // examine the current pic to see if it is the blockbuster logo
            return true;
        }
    }
}
