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
    public class Monstre : Personnage
    {
        // Propriete
        private string nom = "";
        private static int indexMonstre = 1;
        private int pixelRestantPourDeplacement = Parametres.TAILLE_BLOC;
        public int de;
        public new static float freezeTime = 2500; //Je trouve ca plus simple pour gerer le freezetime de plusieurs monstres sachant qu'ils ont tous le meme


        // Getter/Setter
        public string Nom
        {
            get => nom;
        }

        public int PixelRestantPourDeplacement
        {
            get => pixelRestantPourDeplacement;
            set => pixelRestantPourDeplacement = value;
        }

        //Constructeur
        public Monstre(byte positionX, byte positionY, byte largeurCarte, byte hauteurCarte) : base(positionX, positionY, largeurCarte, hauteurCarte)
        {
            this.curHP = maxHP;
            this.curAP = 7;
            this.curDP = 4;
            this.nom = "Monstre" + indexMonstre++;
        }

        // Methodes
        public override bool EstDansLesLimitesDeLaCarte(byte limiteX, byte limiteY)
        {
            try
            {
                if (this.positionY >= 0 && this.positionY <= limiteY && this.positionX >= 0 && this.positionX <= limiteX)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public string Attaquer(Chasseur cible)
        {
            try
            {
                // Si la cible est empoisonnee, enregistre la defense modifiee pour l'ajouter en degat bonus
                byte defense = (byte)(cible.EtatActuel is EtatPoison ? cible.EtatActuel.CalculerDefense(cible) : 0);

                // Enregistre le montant des degats
                string degat = cible.PrendreDesDegats((byte)(this.curAP - (cible.inventaireBouclier.Count() > 0 ? cible.inventaireBouclier.Peek().DP : 0) + defense));

                // Retourne les degats
                return $"Le monstre attaque {cible.NomChasseur}. " + (cible.EstVivant() ? $"{cible.NomChasseur} perd {degat} HP. Il lui reste {cible.CurHP} HP           " : $"{cible.NomChasseur} est mort(e).                         ");
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }

        public void ChangerPosition(char axe, bool estPositif)
        {
            try
            {
                // Si axe des Y
                if (axe == Parametres.AXE_Y)
                {
                    // Incremente ou decremente selon mouvement positif ou negatif
                    this.PositionY = (estPositif ? ++this.PositionY : --this.PositionY);
                }

                // Si axe des Y
                else if (axe == Parametres.AXE_X)
                {
                    // Incremente ou decremente selon mouvement positif ou negatif
                    this.PositionX = (estPositif ? ++this.PositionX : --this.PositionX);
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void LancerDe()
        {
            try
            {
                this.de = Hasard.RNG.Next(0, 4);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
