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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class Carte
    {
        // Propriétées
        private byte hauteurCarte = 0;
        private byte largeurCarte = 0;
        private string[] listecarte = null;
        public char[][] carte = new char[][] { };

        public byte positionObjectifX = 0;
        public byte positionObjectifY = 0;

        private bool carteDejaChargee = false;


        // Getter / Setter
        public string ErreurValidation { get; private set; }

        public string[] ListeCarte
        {
            get { return this.listecarte; }
            private set
            {
                this.listecarte = value;
            }
        }

        public byte HauteurCarte 
        { 
            get { return this.hauteurCarte; }
            private set
            {
                // Si hauteur negative
                if (value < 0)
                {
                    // Message d'erreur
                    ErreurValidation = "Erreur: La hauteur de la carte ne peut pas etre negative.";
                }
                // Si hauteur supperieur au max
                else if (value > Parametres.HAUTEUR_CARTE_MAX) 
                {
                    // Message d'erreur
                    ErreurValidation = $"Erreur: La hauteur de la carte ne peut pas depasser {Parametres.HAUTEUR_CARTE_MAX} unites.";

                    // Assigne la valeur
                    this.hauteurCarte = value;
                }
                // Si hauteur valide
                else
                {
                    // Aucun message d'erreur
                    ErreurValidation = "";

                    // Assigne la valeur
                    this.hauteurCarte = value;
                }
            }
        }

        public byte LargeurCarte
        {
            get { return this.largeurCarte; }
            private set
            {
                // Si hauteur negative
                if (value < 0)
                {
                    // Message d'erreur
                    ErreurValidation = "Erreur: La largeur de la carte ne peut pas etre negative.";
                }
                // Si hauteur supperieur au max
                else if (value > Parametres.LARGEUR_CARTE_MAX)
                {
                    // Message d'erreur
                    ErreurValidation = $"Erreur: La largeur de la carte ne peut pas depasser {Parametres.LARGEUR_CARTE_MAX} unites.";


                }
                // Si hauteur valide
                else
                {
                    if (ErreurValidation == "")
                    {
                        // Aucun message d'erreur
                        ErreurValidation = "";
                    }
                    
                    // Assigne la valeur
                    this.largeurCarte = value;
                }
            }
        }


        // Constructeur
        public Carte()
        {
            // Creation d'un tableau temporaire de tous les fichier .txt (inclu le chemin)
            string[] listeTempCarte = Directory.GetFiles(Parametres.CHEMIN, Parametres.OPTION_DE_RECHERCHE);

            // Declaration de la longeur du tableau contenant la liste des noms de carte en fonction du tableau temporaire
            listecarte = new string[listeTempCarte.Length];

            // Declaration & initialisation a 0 de la position du chemin dans les strings du tableau temporaire
            int indexPath = 0;

            // Boucler dans le tableau temporaire
            for (int i = 0; i < listeTempCarte.Length; i++)
            {
                // Assignation de la position du chemnin a l'index
                indexPath = listeTempCarte[i].IndexOf(Parametres.CHEMIN);

                // Retrait du chemin dans le string, puis attribution du resultat au tableau des nom de carte
                listecarte[i] = listeTempCarte[i].Substring(indexPath + Parametres.CHEMIN.Length + 1);
            }
        }


        // Methodes
        public void ChargerCarte(string nomCarte, ref Chasseur chasseur, ref Monstres monstres)
        {
            try
            {
                // Si aucune carte chargee
                if (!carteDejaChargee)
                {
                    // Boucler dans toutes les lignes du fichier
                    foreach (string stringligneFichier in File.ReadLines(nomCarte))
                    {
                        // Convertir la ligne du fichier en tableau de char
                        char[] tableauLigneFichier = stringligneFichier.ToCharArray();

                        // Ajouter le tableau de char au tableau irregulier
                        Array.Resize(ref carte, carte.Length + 1);
                        carte[carte.GetUpperBound(0)] = tableauLigneFichier;
                    }

                    // Attribution des dimensions de la carte
                    HauteurCarte = (byte)(carte.GetLength(0) - 1);
                    LargeurCarte = (byte)carte[HauteurCarte - 1].GetLength(0);


                    // Boucle pour trouver le joueur, monstres, object, etc
                    for (byte y = 0; y < carte.GetLength(0); y++)
                    {
                        // 2em dim -> colone
                        for (byte x = 0; x < carte[y].Length; x++)
                        {
                            switch (carte[y][x])
                            {
                                // Si element de la carte est le joueur
                                case Parametres.SYMBOLE_JOUEUR:
                                    // Creer un chasseur avec les coordonnees et le nom existant
                                    chasseur = new Chasseur(chasseur.NomChasseur, x, y, LargeurCarte, HauteurCarte);
                                    break;

                                // Si element de la carte est l'objectif
                                case Parametres.SYMBOLE_OBJECTIF:
                                    // Enregistre les coordonnees de l'objectif
                                    positionObjectifX = x;
                                    positionObjectifY = y;
                                    break;

                                // Si element de la carte est un pic
                                case Parametres.SYMBOLE_PIC:
                                    // Ajoute le pic au bassin de pics
                                    Pic.bassinPics.Add(new Pic(x, y));
                                    break;

                                // Si l'element est un bouclier
                                case Parametres.SYMBOLE_BOUCLIER:
                                    // Ajoute le bouclier au bassin de bouclier
                                    Bouclier.bassinBoucliers.Add(new Bouclier(x, y));
                                    break;

                                // Si l'element est une epee
                                case Parametres.SYMBOLE_EPEE:
                                    // Ajoute l'epee au bassin d'epee
                                    Epee.bassinEpees.Add(new Epee(x, y));
                                    break;

                                // Si element de la carte est un monstre
                                case Parametres.SYMBOLE_MONSTRE:
                                    // Ajoute le monstre a la liste de monstres
                                    monstres.bassinMonstres.Add(new Monstre(x, y, LargeurCarte, HauteurCarte));
                                    break;

                                // Si l'element est une potion
                                case Parametres.SYMBOLE_POTION:
                                    // Ajoute les potions au bassin de potions
                                    Potion.bassinPotions.Add(new Potion(x, y));
                                    break;
                            }
                        }
                    }

                    // La carte devient chargee
                    carteDejaChargee = true;
                }

                // Si une carte est deja chargee
                else
                {
                    // Ecrase le contenu de la carte
                    carte = new char[][] { };

                    // Ecrase le bassin de monstres
                    monstres.bassinMonstres.Clear();

                    // Reinitialisation des bassins pour eviter les doublons cause par le premier chargement de carte
                    Pic.bassinPics.Clear();
                    Bouclier.bassinBoucliers.Clear();
                    Epee.bassinEpees.Clear();
                    Potion.bassinPotions.Clear();

                    // La carte devient non-chargee
                    carteDejaChargee = false;

                    // Charge la nouvelle carte
                    ChargerCarte(nomCarte, ref chasseur, ref monstres);
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
