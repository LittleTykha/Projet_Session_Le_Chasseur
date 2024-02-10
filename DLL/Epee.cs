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
    public class Epee
    {
        // Constantes
        private const byte MIN_AP = 4;
        private const byte MAX_AP = 9;
        private const byte CHANCE_BRIS_EPEE = 5;


        // Proprietes
        private byte ap = 0;
        private string etat = Parametres.ETAT_FONCTIONNEL;
        public byte positionX = 0;
        public byte positionY = 0;
        public static List<Epee> bassinEpees = new List<Epee>();


        // Getter / Setter
        public byte AP
        {
            get { return ap; }
            private set { ap = value; }
        }


        // Constructeur
        public Epee(byte positionX, byte positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;

            // Assigne une puissance au hasard entre min (incl) et max + 1 (exclu)
            this.AP = (byte)Hasard.RNG.Next(MIN_AP, MAX_AP + 1);
        }


        // Methodes
        public bool VerifierEpee()
        {
            try
            {
                // Genere un chiffre aleatoire selon les chances de bris
                byte etatIndex = (byte)Hasard.RNG.Next(0, CHANCE_BRIS_EPEE);


                // Si index est egal a une valeur aleatoire selon les chances de bris
                if (etatIndex == (byte)Hasard.RNG.Next(0, CHANCE_BRIS_EPEE))
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
