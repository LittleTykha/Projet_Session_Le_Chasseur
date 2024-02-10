/*
 * Project Name: DLL
 * Student Name: Patrick Tremblay
 * Student ID:   2312796
 * Date:         Oct 27th 2023
 * Version:      1
 * Description:  Projet de Session : DLL (Moteur de Jeu)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class Hasard : Random
    {
        // Proprietes
        private static Hasard rng = null;

        public static Hasard RNG
        {
            get
            {
                if (rng == null)
                {
                    rng = new Hasard();
                }

                return rng;
            }
        }
    }
}
