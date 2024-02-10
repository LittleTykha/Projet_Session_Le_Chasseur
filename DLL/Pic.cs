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
    public class Pic
    {
        // Constantes
        private const byte CHANCE_BRIS = 2; // 1/2 chance me semble mieux, car en testant avec 1/3 chance, je pouvais presque briser la moitie des murs avec un seul pic


        // Proprietes
        public string etat = Parametres.ETAT_FONCTIONNEL;
        public byte positionX = 0;
        public byte positionY = 0;
        public static List<Pic> bassinPics = new List<Pic>(); // Pour pouvoir gerer plusieur pics sur une meme carte


        // Constructeur
        public Pic(byte x, byte y)
        {
            this.positionX = x;
            this.positionY = y;
        }


        // Methode
        public bool VerifierPic()
        {
            try
            {
                // Genere un chiffre aleatoire selon les chances de bris
                byte etatIndex = (byte)Hasard.RNG.Next(0, CHANCE_BRIS);


                // Si index est egal a une valeur aleatoire selon les chances de bris
                if (etatIndex == (byte)Hasard.RNG.Next(0, CHANCE_BRIS))
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
