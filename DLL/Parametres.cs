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
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public static class Parametres
    {
        // Proprietes (constantes)
        public const string ETUDIANT = "\"Patrick Tremblay\"";
        public const string ENTER = "\n\r";
        public const string ERROR_LOG = "error.log";
        public const string FICHIER_SCORE = "score.txt";
        public const bool DEBUG_MODE = true;

        public const string NOM_JEU = "Chasseur de Monstres";
        public const string SOULIGNEMENT = "--------------------------------------";
        public const string TITRE_PROJET = "Pojet de Session: " + NOM_JEU;

        #region MESSAGES
        public const string MESSAGE_CHOIX_NOM = "Veuillez choisir un nom: ";
        public const string NOM_AVATAR = "Héloïse";
        public const string MESSAGE_DESC_JEU = "Vous incarnez " + NOM_AVATAR + ", une fillette en proie aux cauchemars." + ENTER +
                                        "Votre objectif consiste à retrouver votre compagnon de rêve, Mr. Oursenpluche," + ENTER + 
                                        "afin de trouver la force de vaincre vos peurs. Il vous faudra être prudent, cependant," + ENTER + 
                                        "car des monstres ont envahi vos songes." + "Vous êtes libre d'utiliser tous les moyens à" + ENTER + 
                                        "votre disposition pour quitter ce mauvais rêve." + ENTER;
        public const string MESSAGE_VOULEZ_VOUS_CONTINUER = "Souhaitez-vous continuer? (" + REPONSE_OUI_MAJ_S + " ou " + REPONSE_NON_MAJ_S + ")" + " ";
        public const string MESSAGE_BONNE_CHANCE = "Bonne chance!";
        public const string MESSAGE_ABANDON = "\"" + NOM_AVATAR + " arrivera-t-elle à vaincre ses peurs? Nous ne le sauront jamais... Bien que de marbre, lorsque la nuit tombe, " + ENTER +
                                       "vous entendez des hurlements dans vos cauchemars....\"";
        public const string MESSAGE_SELECTION_NIVEAU_1 = "Choix de cartes: " + ENTER;
        public const string MESSAGE_SELECTION_NIVEAU_2 = "Veuillez choisir votre aventure: ";
        public const string MESSAGE_NOUVELLE_CARTE = "Voulez-vous retourner à la sélection du niveau? ";
        public const string MESSAGE_NIVEAU_SUIVANT = "Souhaitez-vous passer au niveau suivant? ";
        public const string PERDRE_PARTIE = NOM_AVATAR + " a succombée à ses peurs...";
        #endregion

        #region VALIDATION
        public const char REPONSE_OUI_MAJ = 'O';
        public const char REPONSE_OUI_MIN = 'o';
        public const char REPONSE_NON_MAJ = 'N';
        public const char REPONSE_NON_MIN = 'n';
        public const string REPONSE_OUI_MAJ_S = "O";
        public const string REPONSE_NON_MAJ_S = "N";
        public const string CHOIX_1 = "1";
        public const string CHOIX_2 = "2";
        public const string CHOIX_3 = "3";
        public const string STRING_VIDE = "";

        public const string ERREUR_NOM_TROP_LONG = "Erreur: Le nom ne doit pas avoir plus de 20 caractères.";
        public const string ERREUR_NOM_VIDE = "Erreur: Le nom ne doit pas être vide.";
        public const string MESSAGE_ERREUR_CHAR = "Veuillez répondre par " + REPONSE_OUI_MAJ_S + " ou par " + REPONSE_NON_MAJ_S;
        public const string MESSAGE_ERREUR_CHOIX_NIVEAU = "Veuillez choisir une option valide...";
        public const string MESSAGE_ERREUR_AUCUNE_SELECTION = "Erreur: Aucune carte n'a été sélectionnée.";
        #endregion

        #region PAMARETRES_JEU
        public const byte NBR_MAX_CHARACTERE = 20;
        public const byte MAX_LIGNE_HISTORIQUE_INFO = 5;
        public const int DECALAGE_CARTE_Y = 11;
        public const int DECALAGE_CARTE_X = 0;
        public const byte HAUTEUR_CARTE_MAX = 20;
        public const byte LARGEUR_CARTE_MAX = 70;
        public const int TAILLE_BLOC = 50;
        public const string CHEMIN = @"..\Debug";
        public const string OPTION_DE_RECHERCHE = "*.map";
        #endregion

        #region POINTAGE
        public const byte POINT_DETRUIRE_MUR = 25;
        public const int POINT_OBJECTIF = 500;
        public const byte POINT_RAMMASSER_OBJET = 50;
        public const byte POINT_VAINCRE_MONSTRE = 100;
        public const byte POINT_PRENDRE_POTION = 25;
        public const byte POINT_SURVIE = 5;
        #endregion

        public const string ETAT_FONCTIONNEL = "Fonctionnel";
        public const string ETAT_BRISE = "Brise";

        #region SYMBOLES
        public const char SYMBOLE_MUR = '#';
        public const char SYMBOLE_JOUEUR = 'J';
        public const char SYMBOLE_MONSTRE = 'M';
        public const char SYMBOLE_VIDE = ' ';
        public const char SYMBOLE_OBJECTIF = 'A';
        public const char SYMBOLE_PIC = 'T';
        public const char SYMBOLE_BOUCLIER = 'B';
        public const char SYMBOLE_EPEE = 'E';
        public const char SYMBOLE_POTION = 'P';
        #endregion

        public const char AXE_X = 'X';
        public const char AXE_Y = 'Y';

        #region ACTIONS
        public const string ACTION_BAS = "Le joueur est allé vers le bas                                                  ";
        public const string ACTION_HAUT = "Le joueur est allé vers le haut                                              ";
        public const string ACTION_GAUCHE = "Le joueur est allé vers la gauche                                              ";
        public const string ACTION_DROITE = "Le joueur est allé vers la droite                                              ";
        public const string MESSAGE_PIC_DETRUIT = "Le pic est détruit                                                      ";
        public const string MESSAGE_EPEE_DETRUIT = "L'épée est détruite                                                    ";
        public const string MESSAGE_BOUCLIER_DETRUIT = "Le bouclier est détruit                                            ";
        public const string ACTION_DETRUIRE_MUR = "Le joueur a detruit le mur bloquant son chemin                          ";
        public const string ACTION_PRENDRE_UN_PIC = "Le joueur a trouvé un pic                                             ";
        public const string ACTION_PRENDRE_UN_NOUVEAU_PIC = "Le joueur remplace son pic par un nouveau pic                  ";
        public const string ACTION_PRENDRE_UN_BOUCLIER = "Le joueur a trouvé un bouclier                                   ";
        public const string ACTION_PRENDRE_UNE_EPEE = "Le joueur a trouvé une épée                                        ";
        public const string ACTION_PRENDRE_UNE_POTION = "Le joueur a trouvé une potion de type: ";
        public const string ETAT_FORT = "Le joueur devient robuste                                                      ";
        public const string ETAT_EMPOISONNE = "Le joueur est vulnerable                                                  ";
        public const string ETAT_VITESSE = "Le joueur est rapide                                                         ";
        public const string ETAT_INVISIBLE = "Le joueur est invisible                                                   ";
        #endregion
    }
}