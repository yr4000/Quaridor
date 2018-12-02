using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    class Agent
    {

        #region Members

        #endregion Members


        #region Constructor

        public Agent()
        {
            //TODO: complete
        }
        #endregion Constructor

        /*
         *  calcualtes the next step the agent should pick
         */
        private string DoMove()
        {
            throw new NotImplementedException();
        }

        /*
         *  returns the reward for the current state of the board.
         */
        private int GetReward()
        {
            //TODO: complete. should it be oppenent_shortest_path - Agents_shortest_path?
            //      The idea is that the longer his path the better and the shorter the agents path the better

            throw new NotImplementedException();
        }
    }
}
