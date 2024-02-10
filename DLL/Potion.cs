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
    public class Potion
    {
        // Constantes
        private const byte CHANCE_TYPE = 6;
        

        // Proprietes
        public TypePotion type;
        public byte positionX = 0;
        public byte positionY = 0;
        public static List<Potion> bassinPotions = new List<Potion>();


        // Constructeur
        public Potion(byte posX, byte posY)
        {
            this.positionX = posX;
            this.positionY = posY;

            // Genere un chiffre aleatoire selon les chances de bris
            byte typeIndex = (byte)Hasard.RNG.Next(0, CHANCE_TYPE);


            // Attribu un type selon typeIndex
            switch (typeIndex)
            {
                case 1:
                    // Change le type pour empoisonnee
                    this.type = TypePotion.Empoisonnee;
                    break;

                case 2:
                case 3:
                    // Change le type pour vitesse
                    this.type = TypePotion.Vitesse;
                    break;

                case 4:
                case 5:
                    // Change le type pour invisibilite
                    this.type = TypePotion.Invisibilite;
                    break;

                case 6:
                    // Change le type pour force
                    this.type = TypePotion.Force;
                    break;
            }
        }
    }

    // Enumeration des types de potions
    public enum TypePotion
    {
        Force,
        Empoisonnee,
        Invisibilite,
        Vitesse
    }
}
