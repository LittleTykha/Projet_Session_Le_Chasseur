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
    public class Bouclier
    {
        // Constantes
        private const byte MIN_DP = 3;
        private const byte MAX_DP = 6;
        private const byte CHANCE_BRIS_BOUCLIER = 4;


        // Proprietes
        private byte dp = 0;
        private string etat = Parametres.ETAT_FONCTIONNEL;
        public byte positionX = 0;
        public byte positionY = 0;
        public static List<Bouclier> bassinBoucliers = new List<Bouclier>();


        // Getter / Setter
        public byte DP
        {
            get { return dp; }
            private set { dp = value; }
        }


        // Constructeur
        public Bouclier(byte positionX, byte positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;

            // Assigne une puissance au hasard entre min (incl) et max + 1 (exclu)
            this.DP = (byte)Hasard.RNG.Next(MIN_DP, MAX_DP + 1);
        }


        // Methodes
        public bool VerifierBouclier()
        {
            try
            {
                // Genere un chiffre aleatoire selon les chances de bris
                byte etatIndex = (byte)Hasard.RNG.Next(0, CHANCE_BRIS_BOUCLIER);


                // Si index est egal a une valeur aleatoire selon les chances de bris
                if (etatIndex == (byte)Hasard.RNG.Next(0, CHANCE_BRIS_BOUCLIER))
                {
                    // Change l'etat
                    this.etat = Parametres.ETAT_BRISE;

                    // Retourne TRUE
                    return true;
                }
                // Sinon
                else
                {
                    // Retourne FALSE
                    return false;
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
    }
}
