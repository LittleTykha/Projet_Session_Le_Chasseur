/*
 * Project Name: Jeu_Console
 * Student Name: Patrick Tremblay
 * Student ID:   2312796
 * Date:         Oct 27th 2023
 * Version:      1
 * Description:  Projet de Session - Jeu Console
*/

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DLL;

namespace Jeu_Console
{
    internal class JeuConsole
    {
        // Constantes
        const string ENTER = "\n\r";


        // Variables globales
        static Carte carte = new Carte();
        static Chasseur joueur = new Chasseur();
        static Monstres monstres = new Monstres();
        static byte niveau = 1;
        static bool EnTrainDeBouger = false;
        static bool partieEnCours = true;
        static bool estSurUnMonstre = false;
        static string carteChoisie = "";


        // Main
        static void Main(string[] args)
        {
            try
            {
                // Variables locale
                ConsoleKeyInfo toucheAppuyee;
                char reponseUser = ' ';
                string choixUser = "";
                bool AGagner = false;
                Thread threadJoueur;
                Thread threadMonstre;
                //Timer timerMonstres;


                // Modifier le titre de la console
                Console.Title = Parametres.TITRE_PROJET;

                // Afficher le nom du projet
                AfficherNomJeu();

                // Demander le nom du joueur
                Console.Write(ENTER + Parametres.MESSAGE_CHOIX_NOM);

                // Entrer le nom du joueur
                joueur.NomChasseur = Console.ReadLine();

                // Valider le nom
                ValiderNomJoueur();


                // Boucle principale du jeu
                do
                {
                    #region DEBUT_JEU
                    // Vide la console
                    Console.Clear();


                    // Affiche le nom du jeu a nouveau
                    AfficherNomJeu();


                    // Afficher un message de bienvenu (contexte et objectif du jeu)
                    Console.WriteLine(ENTER + $"Salutation, {joueur.NomChasseur}!" + ENTER);


                    // Affiche l'intro
                    AfficherIntro();


                    // Demander au user s'il veut continuer
                    Console.Write(Parametres.MESSAGE_VOULEZ_VOUS_CONTINUER);


                    // VALIDATION - Boucle si n'est pas un CHAR et si autre que valeur souhaitees
                    reponseUser = ValiderOuiNon(reponseUser);


                    // Si user repond non
                    if (reponseUser == Parametres.REPONSE_NON_MAJ || reponseUser == Parametres.REPONSE_NON_MIN)
                    {
                        // Message d'abandon
                        Console.WriteLine(ENTER + ENTER + Parametres.MESSAGE_ABANDON);

                        // Quitte la boucle principale
                        break;
                    }

                    // Si user repond oui
                    Console.WriteLine(ENTER + $"Merci pour votre courage, {joueur.NomChasseur}. " + Parametres.MESSAGE_BONNE_CHANCE + ENTER);
                    Console.ReadKey();
                    #endregion


                    // Boucle secondaire du jeu (niveau)
                    do
                    {
                        
                            #region SELECTION_NIVEAU
                            // Effacer et afficher le nom du jeu
                            AfficherNomJeu();

                        do
                        {
                            // Afficher le menu de selection de niveau
                            AfficherSelectionDuNiveau();


                            // Validation du choix de l'user
                            choixUser = ValiderChoixNiveau(choixUser);


                            // Chargement de la carte selon le choix
                            switch (choixUser)
                            {
                                // Charger carte 1
                                case "1":
                                    // Selection le nom de la carte
                                    carteChoisie = carte.ListeCarte[0];

                                    // Vide la console
                                    Console.Clear();

                                    // Affiche le nom du jeu
                                    AfficherNomJeu();

                                    // Charge la carte
                                    carte.ChargerCarte(carteChoisie, ref joueur, ref monstres);

                                    // Affiche l'UI
                                    AfficherUI(carteChoisie);

                                    // Dessine la carte
                                    DessinerCarte(carte);
                                    break;

                                // Charger carte 2
                                case "2":
                                    // Selection le nom de la carte
                                    carteChoisie = carte.ListeCarte[1];

                                    // Vide la console
                                    Console.Clear();

                                    // Affiche le nom du jeu
                                    AfficherNomJeu();

                                    // Charge la carte
                                    carte.ChargerCarte(carteChoisie, ref joueur, ref monstres);

                                    // Affiche l'UI
                                    AfficherUI(carteChoisie);

                                    // Dessine la carte
                                    DessinerCarte(carte);
                                    break;

                                // Charger carte 3
                                case "3":
                                    // Selection le nom de la carte
                                    carteChoisie = carte.ListeCarte[2];

                                    // Vide la console
                                    Console.Clear();

                                    // Affiche le nom du jeu
                                    AfficherNomJeu();

                                    // Charge la carte
                                    carte.ChargerCarte(carteChoisie, ref joueur, ref monstres);

                                    // Affiche l'UI
                                    AfficherUI(carteChoisie);

                                    // Dessine la carte
                                    DessinerCarte(carte);
                                    break;
                            }

                            // S'il y a une erreur de validation
                            if (carte.ErreurValidation != "")
                            {
                                // Remet les couleurs par defaut
                                ResetApparenceConsole(true);

                                // Affiche le nom du jeu
                                AfficherNomJeu();

                                // Afficher le message d'erreur en rouge
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"{ENTER}{carte.ErreurValidation}");
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                        while (carte.ErreurValidation != "");
                        #endregion


                        do
                        {
                            // Debute le timer monstre
                            //timerMonstres = new Timer(TimerCallBack, null, 0, (int)monstres.bassinMonstres.First().FreezeTime);


                            // Debuter la thread des monstres
                            threadMonstre = new Thread(new ThreadStart(BougerMonstreDansLaThread));
                            threadMonstre.IsBackground = true;
                            threadMonstre.Start();


                            // Controles du joueur
                            Console.CursorVisible = false;
                            toucheAppuyee = Console.ReadKey(true);


                            // Boucle infini pour deplacement
                            while (true)
                            {
                                // Si le joueur n'est pas en train de bouger
                                if (!EnTrainDeBouger)
                                {
                                    #region DEPLACEMENT_JOUEUR
                                    // Affiche le joueur en magenta et le background en gris pour chaque deplacement du joueur
                                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                    Console.BackgroundColor = ConsoleColor.Gray;


                                    // Afficher le symbole vide a la palce du joueur
                                    AfficherSymbole(Parametres.SYMBOLE_VIDE, joueur.PositionX, joueur.PositionY);


                                    // Deplacer le joueur en fonction de la touche et les limites de la carte
                                    switch (toucheAppuyee.Key)
                                    {
                                        // Bas
                                        case ConsoleKey.S:
                                            //Si dans les limites
                                            if (joueur.EstDansLesLimitesDeLaCarte(joueur.LimitePositionX, joueur.LimitePositionY))
                                            {
                                                // Si le joueur entre en contact avec un monstre
                                                if (carte.carte[joueur.PositionY + 1][joueur.PositionX] == Parametres.SYMBOLE_MONSTRE)
                                                {
                                                    // Si n'est pas invisible
                                                    if (!(joueur.EtatActuel is EtatInvisible))
                                                    {
                                                        // Boucle dans la liste de monstre 
                                                        foreach (Monstre monstre in monstres.bassinMonstres)
                                                        {
                                                            // Si le monstre est celui pres du joueur
                                                            if (joueur.PositionX == monstre.PositionX && joueur.PositionY + 1 == monstre.PositionY)
                                                            {
                                                                // Le joueur attaque
                                                                LeJoueurAttaque(monstre);

                                                                // Si monstre est vivant
                                                                if (monstre.EstVivant())
                                                                {
                                                                    // Le monstre attaque
                                                                    LeJoueurDefend(monstre);
                                                                }
                                                                // Si le monstre est mort
                                                                else
                                                                {
                                                                    // Supprime le monstre de la carte (tableau)
                                                                    SupprimerMonstre(monstre);

                                                                    // Supprime le monstre de la carte (niveau)
                                                                    AfficherSymbole(Parametres.SYMBOLE_VIDE, monstre.PositionX, monstre.PositionY);

                                                                    // Quitte la boucle
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    else
                                                    {
                                                        // Deplace le joueur
                                                        joueur.ChangerPosition(Parametres.AXE_Y, true);

                                                        // Ajoute l'action a info
                                                        joueur.AjouterHistoriqueInfo(Parametres.ACTION_BAS);

                                                        // Le joueur est sur un monstre
                                                        estSurUnMonstre = true;
                                                    }
                                                }


                                                // Si pas de mur
                                                else if (carte.carte[joueur.PositionY + 1][joueur.PositionX] != Parametres.SYMBOLE_MUR)
                                                {
                                                    // Deplacer le joueur
                                                    DeplacerJoueur(Parametres.AXE_Y, true, Parametres.ACTION_BAS);
                                                }


                                                // Si un mur et possede un pic
                                                else if (joueur.inventairePic.Count != 0)
                                                {
                                                    // Retire le mur de la carte (tableau)
                                                    joueur.DetruireMur(carte, joueur.PositionX, joueur.PositionY, Parametres.AXE_Y, true);


                                                    // Detuit le mur de la carte (niveau)
                                                    AfficherSymbole(Parametres.SYMBOLE_VIDE, joueur.PositionX, (byte)(joueur.PositionY + 1));


                                                    // Verifie l'etat du pic et retirer de l'inventaire si brise
                                                    joueur.VerifierPic();
                                                }
                                            }
                                            break;

                                        //Haut
                                        case ConsoleKey.W:
                                            //Si dans les limites
                                            if (joueur.EstDansLesLimitesDeLaCarte(joueur.LimitePositionX, joueur.LimitePositionY))
                                            {
                                                // Si le joueur entre en contact avec un monstre
                                                if (carte.carte[joueur.PositionY - 1][joueur.PositionX] == Parametres.SYMBOLE_MONSTRE)
                                                {
                                                    // Si n'est pas invisible
                                                    if (!(joueur.EtatActuel is EtatInvisible))
                                                    {
                                                        // Boucle dans la liste de monstre 
                                                        foreach (Monstre monstre in monstres.bassinMonstres)
                                                        {
                                                            // Si le monstre est celui pres du joueur
                                                            if (joueur.PositionX == monstre.PositionX && joueur.PositionY - 1 == monstre.PositionY)
                                                            {
                                                                // Le joueur attaque
                                                                LeJoueurAttaque(monstre);

                                                                if (monstre.EstVivant())
                                                                {
                                                                    // Le monstre attaque
                                                                    LeJoueurDefend(monstre);
                                                                }
                                                                else
                                                                {
                                                                    // Supprime le monstre de la carte (tableau)
                                                                    SupprimerMonstre(monstre);

                                                                    // Supprime le monstre de la carte (niveau)
                                                                    AfficherSymbole(Parametres.SYMBOLE_VIDE, monstre.PositionX, monstre.PositionY);

                                                                    // Quitte la boucle
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    // Si est invisible
                                                    else
                                                    {
                                                        // Deplace le joueur
                                                        joueur.ChangerPosition(Parametres.AXE_Y, false);

                                                        // Ajoute l'action a info
                                                        joueur.AjouterHistoriqueInfo(Parametres.ACTION_HAUT);

                                                        // Le joueur est sur un monstre
                                                        estSurUnMonstre = true;
                                                    }
                                                }


                                                // Si pas de mur
                                                else if (carte.carte[joueur.PositionY - 1][joueur.PositionX] != Parametres.SYMBOLE_MUR)
                                                {
                                                    // Deplace le joueur
                                                    DeplacerJoueur(Parametres.AXE_Y, false, Parametres.ACTION_HAUT);
                                                }


                                                // Si un mur et possede un pic
                                                else if (joueur.inventairePic.Count != 0)
                                                {
                                                    // Retire le mur de la carte
                                                    joueur.DetruireMur(carte, joueur.PositionX, joueur.PositionY, Parametres.AXE_Y, false);

                                                    // Detuit le mur de la carte (niveau)
                                                    AfficherSymbole(Parametres.SYMBOLE_VIDE, joueur.PositionX, (byte)(joueur.PositionY - 1));

                                                    // Verifie l'etat du pic et retirer de l'inventaire si brise
                                                    joueur.VerifierPic();
                                                }
                                            }
                                            break;

                                        // Gauche
                                        case ConsoleKey.A:
                                            //Si dans les limites
                                            if (joueur.EstDansLesLimitesDeLaCarte(joueur.LimitePositionX, joueur.LimitePositionY))
                                            {
                                                // Si le joueur entre en contact avec un monstre
                                                if (carte.carte[joueur.PositionY][joueur.PositionX - 1] == Parametres.SYMBOLE_MONSTRE)
                                                {
                                                    // Si n'est pas invisible
                                                    if (!(joueur.EtatActuel is EtatInvisible))
                                                    {
                                                        // Boucle dans la liste de monstre 
                                                        foreach (Monstre monstre in monstres.bassinMonstres)
                                                        {
                                                            // Si le monstre est celui pres du joueur
                                                            if (joueur.PositionX - 1 == monstre.PositionX && joueur.PositionY == monstre.PositionY)
                                                            {
                                                                // Le joueur attaque
                                                                LeJoueurAttaque(monstre);
                                                            }

                                                            if (monstre.EstVivant())
                                                            {
                                                                // Le monstre attaque
                                                                LeJoueurDefend(monstre);
                                                            }
                                                            else
                                                            {
                                                                // Supprime le monstre de la carte (tableau)
                                                                SupprimerMonstre(monstre);

                                                                // Supprime le monstre de la carte (niveau)
                                                                AfficherSymbole(Parametres.SYMBOLE_VIDE, monstre.PositionX, monstre.PositionY);

                                                                // Quitte la boucle
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    // Si le joueur est invisible
                                                    else
                                                    {
                                                        // Deplace le joueur
                                                        joueur.ChangerPosition(Parametres.AXE_X, false);

                                                        // Ajoute l'action a info
                                                        joueur.AjouterHistoriqueInfo(Parametres.ACTION_GAUCHE);

                                                        // Le joueur est sur un monstre
                                                        estSurUnMonstre = true;
                                                    }
                                                }


                                                // Si pas de mur
                                                else if (carte.carte[joueur.PositionY][joueur.PositionX - 1] != Parametres.SYMBOLE_MUR)
                                                {
                                                    // Deplace le joueur
                                                    DeplacerJoueur(Parametres.AXE_X, false, Parametres.ACTION_GAUCHE);
                                                }


                                                // Si un mur et possede un pic
                                                else if (joueur.inventairePic.Count != 0)
                                                {
                                                    // Retire le mur de la carte
                                                    joueur.DetruireMur(carte, joueur.PositionX, joueur.PositionY, Parametres.AXE_X, false);

                                                    // Detuit le mur de la carte (niveau)
                                                    AfficherSymbole(Parametres.SYMBOLE_VIDE, (byte)(joueur.PositionX - 1), joueur.PositionY);

                                                    // Verifie l'etat du pic et retirer de l'inventaire si brise
                                                    joueur.VerifierPic();
                                                }
                                            }
                                            break;

                                        // Droite
                                        case ConsoleKey.D:
                                            //Si dans les limites
                                            if (joueur.EstDansLesLimitesDeLaCarte(joueur.LimitePositionX, joueur.LimitePositionY))
                                            {
                                                // Si le joueur entre en contact avec un monstre
                                                if (carte.carte[joueur.PositionY][joueur.PositionX + 1] == Parametres.SYMBOLE_MONSTRE)
                                                {
                                                    // Si n'est pas invisible
                                                    if (!(joueur.EtatActuel is EtatInvisible))
                                                    {
                                                        // Boucle dans la liste de monstre 
                                                        foreach (Monstre monstre in monstres.bassinMonstres)
                                                        {
                                                            // Si le monstre est celui pres du joueur
                                                            if (joueur.PositionX + 1 == monstre.PositionX && joueur.PositionY == monstre.PositionY)
                                                            {
                                                                // Le joueur attaque
                                                                LeJoueurAttaque(monstre);

                                                                if (monstre.EstVivant())
                                                                {
                                                                    // Le monstre attaque
                                                                    LeJoueurDefend(monstre);
                                                                }
                                                                else
                                                                {
                                                                    // Supprime le monstre de la carte (tableau)
                                                                    SupprimerMonstre(monstre);

                                                                    // Supprime le monstre de la carte (niveau)
                                                                    AfficherSymbole(Parametres.SYMBOLE_VIDE, monstre.PositionX, monstre.PositionY);

                                                                    // Quitte la boucle
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    // Si le joueur est invisible
                                                    else
                                                    {
                                                        // Deplace le joueur
                                                        joueur.ChangerPosition(Parametres.AXE_X, true);

                                                        // Ajoute l'action a info
                                                        joueur.AjouterHistoriqueInfo(Parametres.ACTION_DROITE);

                                                        // Le joueur est sur un monstre
                                                        estSurUnMonstre = true;
                                                    }
                                                }


                                                // Si pas de mur
                                                else if (carte.carte[joueur.PositionY][joueur.PositionX + 1] != Parametres.SYMBOLE_MUR)
                                                {
                                                    // Deplace le joueur
                                                    DeplacerJoueur(Parametres.AXE_X, true, Parametres.ACTION_DROITE);
                                                }


                                                // Si un mur et possede un pic
                                                else if (joueur.inventairePic.Count != 0)
                                                {
                                                    // Retire le mur de la carte
                                                    joueur.DetruireMur(carte, joueur.PositionX, joueur.PositionY, Parametres.AXE_X, true);

                                                    // Detuit le mur de la carte (niveau)
                                                    AfficherSymbole(Parametres.SYMBOLE_VIDE, (byte)(joueur.PositionX + 1), joueur.PositionY);

                                                    // Verifie l'etat du pic et retirer de l'inventaire si brise
                                                    joueur.VerifierPic();
                                                }
                                            }
                                            break;
                                    }


                                    // Afficher le symbole du joueur a la nouvelle position
                                    AfficherSymbole(Parametres.SYMBOLE_JOUEUR, joueur.PositionX, joueur.PositionY);


                                    // Creer un processus secondaire pour appliquer le freezeTime du joueur
                                    threadJoueur = new Thread(new ThreadStart(FigerJoueur));
                                    threadJoueur.IsBackground = true;
                                    threadJoueur.Start();
                                    #endregion


                                    #region POST_DEPLACEMENT
                                    // Si le joueur atteint l'objectif
                                    if (joueur.PositionX == carte.positionObjectifX && joueur.PositionY == carte.positionObjectifY)
                                    {
                                        // Arrete les threads de monstre -> Affiche un message, mais je ne sais pas comment l'enlever. meme chose pour Abort(false)
                                        threadMonstre.Interrupt();

                                        // Gagne la partie
                                        AGagner = true;

                                        // Ajoute des points
                                        joueur.Score += Parametres.POINT_OBJECTIF;

                                        // Raffraichit l'UI
                                        RaffraichirUI(carteChoisie);

                                        // Sort de la boucle infini des mouvements du joueur
                                        break;
                                    }


                                    // Si le joueur atteint un pic
                                    joueur.RamasserPic(carte);


                                    // Si le joueur atteint un bouclier
                                    joueur.RamasserBouclier(carte);


                                    // Si le joueur atteint une epee
                                    joueur.RamasserEpee(carte);


                                    // Si le joueur atteint une potion
                                    joueur.RamasserPotion(carte);


                                    // Rafraichir l'UI a la fin du deplacement pour afficher les actions dans info
                                    RaffraichirUI(carteChoisie);


                                    // Si le joueur meurt
                                    if (!joueur.EstVivant())
                                    {
                                        // Sort de la boucle infini des mouvements du joueur
                                        break;
                                    }
                                    #endregion
                                }

                                // Lis la touche sans afficher la valeur
                                toucheAppuyee = Console.ReadKey(true);
                            }



                            #region FIN_DE_PARTIE
                            // Reset l'apparence de la console
                            ResetApparenceConsole(true);


                            // Change la position du curseur
                            Console.SetCursorPosition(0, carte.carte.GetLength(0) + 10);


                            // Affiche le message de fin
                            Console.WriteLine(ENTER + ENTER + ENTER + (AGagner ? $"Felicitation, {joueur.NomChasseur}" : Parametres.PERDRE_PARTIE));


                            // Si le joueur est mort
                            if (!joueur.EstVivant())
                            {
                                // Quitte la boucle
                                break;
                            }


                            // Demande au user s'il veut jouer le prochain niveau
                            Console.Write(Parametres.MESSAGE_NIVEAU_SUIVANT);


                            // VALIDATION - Boucle si n'est pas un CHAR et si autre que valeur souhaitees
                            reponseUser = ValiderOuiNon(reponseUser);


                            // Si repond oui
                            if (reponseUser == Parametres.REPONSE_OUI_MAJ || reponseUser == Parametres.REPONSE_OUI_MIN)
                            {
                                // Passe au niveau suivant
                                PasserAuNiveauSuivant(carteChoisie);
                            }
                        }
                        while (reponseUser == Parametres.REPONSE_OUI_MAJ || reponseUser == Parametres.REPONSE_OUI_MIN);

                        // Si repond non
                        // Remet le niveau a 1
                        niveau = 1;


                        // Enregistre le score
                        Score.AjouterScore(new Pointage(joueur.NomChasseur, joueur.Score));


                        // Demande au user s'il veut retourner a la selection de carte
                        Console.Write(Parametres.MESSAGE_NOUVELLE_CARTE);


                        // VALIDATION - Boucle si n'est pas un CHAR et si autre que valeur souhaitees
                        reponseUser = ValiderOuiNon(reponseUser);
                        #endregion
                    }
                    while (reponseUser == Parametres.REPONSE_OUI_MAJ || reponseUser == Parametres.REPONSE_OUI_MIN);

                    // Si est gagnant
                    if (AGagner)
                    {
                        // Quitte la boucle principale du jeu
                        break;
                    }
                }
                while (true);


                // Credits
                AfficherCredits();
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #region METHODES
        static void AfficherNomJeu()
        {
            try
            {
                // Affiche le nom du jeu, souligne
                Console.Clear();
                Console.WriteLine($"\n\t\t\t\t\t     {Parametres.NOM_JEU}");
                Console.WriteLine($"\t\t\t\t    {Parametres.SOULIGNEMENT}");
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void ValiderNomJoueur()
        {
            try
            {
                // Boucle jusqu'a ce que le nom soit valide
                while (joueur.ErreurValidation != "")
                {
                    // Afficher le message d'erreur en rouge
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ENTER}{joueur.ErreurValidation}");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    // Demande a nouveau le nom du chasseur
                    Console.Write(ENTER + Parametres.MESSAGE_CHOIX_NOM);
                    joueur.NomChasseur = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void AfficherIntro()
        {
            try
            {
                // Enregistre la position du bout de string cible
                int indexCible = Parametres.MESSAGE_DESC_JEU.IndexOf(Parametres.NOM_AVATAR);

                // Boucle dans la string
                for (int i = 0; i < Parametres.MESSAGE_DESC_JEU.Length; i++)
                {
                    // Si la position est celle de la cible
                    if (i == indexCible)
                    {
                        // Change la couleur des lettres
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        // Boucle dans le bout de string cible
                        for (int x = indexCible; x < indexCible + Parametres.NOM_AVATAR.Length; x++)
                        {
                            // Affiche les lettres de cible
                            Console.Write(Parametres.MESSAGE_DESC_JEU[x]);

                            // Incremente i
                            i++;
                        }

                        // Change la couleur en gris
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    // Affiche le reste du string
                    Console.Write(Parametres.MESSAGE_DESC_JEU[i]);
                }

                // Deux saut de ligne
                Console.Write(ENTER + ENTER);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static char ValiderOuiNon(char reponse)
        {
            try
            {
                string temp = reponse.ToString();

                // VALIDATION - Boucle si n'est pas un CHAR et si autre que valeur souhaitees
                while (!char.TryParse(Console.ReadLine(), out reponse) || reponse != Parametres.REPONSE_OUI_MAJ && reponse != Parametres.REPONSE_OUI_MIN && reponse != Parametres.REPONSE_NON_MAJ && reponse != Parametres.REPONSE_NON_MIN)
                {
                    // Affiche le message en rouge
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ENTER + Parametres.MESSAGE_ERREUR_CHAR);

                    // Affiche de nouveau en blanc
                    Console.ForegroundColor = ConsoleColor.White;

                    // Le systeme demande a nouveau si le user veut retourner au menu principal
                    Console.Write(ENTER + Parametres.MESSAGE_VOULEZ_VOUS_CONTINUER);
                }
                return reponse;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ' ';
            }
        }

        static string ValiderChoixNiveau(string choixUser)
        {
            try
            {
                // Lis l'input du user
                choixUser = Console.ReadLine();

                while (choixUser != Parametres.CHOIX_1 && choixUser != Parametres.CHOIX_2 && choixUser != Parametres.CHOIX_3)
                {
                    // Affiche le message en rouge
                    Console.ForegroundColor = ConsoleColor.Red;

                    // Message d'erreur
                    Console.WriteLine(ENTER + Parametres.MESSAGE_ERREUR_CHOIX_NIVEAU);

                    // Affiche de nouveau en gris
                    Console.ForegroundColor = ConsoleColor.Gray;

                    // Lis de nouveau choix de l'user
                    choixUser = Console.ReadLine();
                }

                // Retourne choixUser
                return choixUser;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return " ";
            }
        }

        static void AfficherSelectionDuNiveau()
        {
            try
            {
                Console.WriteLine(Parametres.MESSAGE_SELECTION_NIVEAU_1);

                // Boucle dans la liste de carte
                for (int i = 0; i < carte.ListeCarte.Length; i++)
                {
                    // Affiche le nom de la carte
                    Console.WriteLine($"{i + 1}: {carte.ListeCarte[i]}");
                }

                Console.Write(ENTER + Parametres.MESSAGE_SELECTION_NIVEAU_2);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void DessinerCarte(Carte carte)
        {
            try
            {
                // Ajoute un background a la carte
                Console.BackgroundColor = ConsoleColor.Gray;

                // Positionne le curseur au bon endroit avant de dessiner
                Console.SetCursorPosition(Parametres.DECALAGE_CARTE_X, Parametres.DECALAGE_CARTE_Y);

                // Bouclier dans la 1er dim du tableau -> Ligne
                for (int y = 0; y < carte.carte.GetLength(0); y++)
                {
                    // 2em dim -> colone
                    for (int x = 0; x < carte.carte[y].Length; x++)
                    {
                        // Si l'element de la carte est le joueur
                        switch (carte.carte[y][x])
                        {
                            case Parametres.SYMBOLE_JOUEUR:
                                // Change la couleur
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                break;

                            case Parametres.SYMBOLE_MONSTRE:
                                // Change la couleur
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;

                            case Parametres.SYMBOLE_OBJECTIF:
                                // Change la couleur
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;

                            case Parametres.SYMBOLE_PIC:
                            case Parametres.SYMBOLE_EPEE:
                            case Parametres.SYMBOLE_BOUCLIER:
                            case Parametres.SYMBOLE_POTION:
                            case Parametres.SYMBOLE_MUR:
                                // Affiche le reste en noir
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                        }
                        // Dessine l'element de la carte
                        Console.Write(carte.carte[y][x]);
                    }
                    // Changer de ligne
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void RedessinerMonstres()
        {
            try
            {
                // Bouclier dans la 1er dim du tableau -> Ligne
                for (int y = 0; y < carte.carte.GetLength(0); y++)
                {
                    // 2em dim -> colone
                    for (int x = 0; x < carte.carte[y].Length; x++)
                    {
                        // Si l'element de la carte est le joueur
                        if (carte.carte[y][x] == Parametres.SYMBOLE_MONSTRE)
                        {
                            // Change la couleur
                            Console.ForegroundColor = ConsoleColor.DarkRed;

                            // Dessine l'element de la carte
                            AfficherSymbole(Parametres.SYMBOLE_MONSTRE, (byte)x, (byte)y);
                        }
                    }
                }
                // Changer de ligne
                Console.SetCursorPosition(joueur.PositionX, joueur.PositionY);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void AfficherSymbole(char symbole, byte positionX, byte positionY)
        {
            try
            {
                Console.SetCursorPosition(positionX, positionY + Parametres.DECALAGE_CARTE_Y);
                Console.Write(symbole);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void ResetApparenceConsole(bool curseurVisible)
        {
            try
            {
                Console.CursorVisible = curseurVisible;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void AfficherCredits()
        {
            try
            {
                Console.WriteLine($"{ENTER}{ENTER}{ENTER}Conçu par {Parametres.ETUDIANT}");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void AfficherUI(string nomCarte)
        {
            try
            {
                // Positionne le curseur au bon endroit
                Console.SetCursorPosition(0, 4);

                Console.WriteLine($"Joueur: {joueur.NomChasseur}");
                if (joueur.CurHP <= 5)
                {
                    Console.Write("HP: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(joueur.CurHP);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.WriteLine($"HP: {joueur.CurHP}     ");
                }
                Console.WriteLine($"Score: {joueur.Score}");
                Console.WriteLine($"Carte: {nomCarte}");
                Console.WriteLine($"Niveau: {niveau}");

                Console.SetCursorPosition(30, 4);
                Console.WriteLine($"Infos: ");
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void RaffraichirUI(string nomCarte)
        {
            try
            {
                // Reset apparance console
                ResetApparenceConsole(false);

                // Variable locale
                byte ligneInfoY = 4;

                // Affiche l'UI
                AfficherUI(nomCarte);

                // Pour chaque valeur de type string dans la queue
                foreach (string info in joueur.historiqueInfo)
                {
                    // Place le curseur ou sera ecrit le message
                    Console.SetCursorPosition(30 + 7, ligneInfoY);

                    // Affiche le message
                    Console.Write(info);

                    // Incrementation
                    ligneInfoY++;
                }
                // Repositionne le curseur a l'emplacement du joueur
                Console.SetCursorPosition(joueur.PositionX, joueur.PositionY);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void FigerJoueur()
        {
            try
            {
                EnTrainDeBouger = true;
                Thread.Sleep((int)joueur.FreezeTime);
                EnTrainDeBouger = false;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void PasserAuNiveauSuivant(string nomCarte)
        {
            try
            {
                // Ajoute un niveau
                niveau++;

                // Ajoute la difficulter
                Monstre.freezeTime *= 0.9f;
                joueur.FreezeTime *= 1.1f;

                // Soigne le joueur
                joueur.CurHP += (byte)Math.Round((decimal)(joueur.MaxHP - joueur.CurHP) / 2);

                // Vide la section info
                joueur.historiqueInfo.Clear();

                // Vide la console
                Console.Clear();

                // Affiche le nom du jeu
                AfficherNomJeu();

                // Raffraichi UI
                RaffraichirUI(nomCarte);

                // Conserve les HP dans une variable temp
                byte tempHP = joueur.CurHP;

                // Conserve l'inventaire dans une variable temp
                Queue<Pic> picTemp = joueur.inventairePic;
                Queue<Epee> epeeTemp = joueur.inventaireEpee;
                Queue<Bouclier> bouclierTemp = joueur.inventaireBouclier;

                // Conserve le score du joueur dans une variable temporaire
                int scoreTemp = joueur.Score;

                // Charge la carte (pour remettre les objects, monstres, murs, etc.)
                carte.ChargerCarte(nomCarte, ref joueur, ref monstres);

                // Redonne au joueur ses HP
                joueur.CurHP = tempHP;

                // Redonne au joueur l'inventaire
                joueur.inventairePic = picTemp;
                joueur.inventaireEpee = epeeTemp;
                joueur.inventaireBouclier = bouclierTemp;

                // Redonne le score
                joueur.Score = scoreTemp;

                // Redessine la carte
                DessinerCarte(carte);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void SupprimerMonstre(Monstre monstre)
        {
            try
            {
                // Supprime le monstre de la carte (tableau)
                carte.carte[monstre.PositionY][monstre.PositionX] = ' ';

                // Supprime le monstre du bassin de monstre
                monstres.bassinMonstres.Remove(monstre);

                // Donne des points
                joueur.Score += Parametres.POINT_VAINCRE_MONSTRE;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void LeJoueurAttaque(Monstre monstre)
        {
            try
            {
                // Le joueur attaque
                joueur.AjouterHistoriqueInfo(joueur.Attaquer(monstre));

                // Si le joueur possedait une epee au moment de l'attaque
                joueur.VerifierEpee();
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void LeJoueurDefend(Monstre monstre)
        {
            try
            {
                // Le monstre attaque
                joueur.AjouterHistoriqueInfo(monstre.Attaquer(joueur));

                // Si le joueur possedait un bouclier au moment de l'attaque
                joueur.VerifierBouclier();
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void DeplacerMonstre(Monstre monstre, char axe, bool estPositif)
        {
            try
            {
                // Changer la couleur du momnstre
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.BackgroundColor = ConsoleColor.Gray;


                // Retirer le symbole a la position actuelle
                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;


                // Afficher un symbole vide a la position actuelle du monstre 
                AfficherSymbole(Parametres.SYMBOLE_VIDE, monstre.PositionX, monstre.PositionY);


                // Deplace le joueur
                monstre.ChangerPosition(axe, estPositif);


                // Ajouter le joueur a la nouvelle position
                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;


                // Afficher le monstre a sa nouvelle position
                AfficherSymbole(Parametres.SYMBOLE_MONSTRE, monstre.PositionX, monstre.PositionY);


                Console.SetCursorPosition(joueur.PositionX, joueur.PositionY);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void DeplacerJoueur(char axe, bool estPositif, string action)
        {
            try
            {
                // Retirer le symbole a la position actuelle
                carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_VIDE;

                // Deplace le joueur
                joueur.ChangerPosition(axe, estPositif);

                // Ajouter le joueur a la nouvelle position
                carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_JOUEUR;

                // Ajoute l'action a info
                joueur.AjouterHistoriqueInfo(action);

                // Si etait sur un monstre
                if (estSurUnMonstre)
                {
                    // Redessine les monstres
                    RedessinerMonstres();

                    // N'est plus sur un monstre
                    estSurUnMonstre = false;
                }

                Console.SetCursorPosition(joueur.PositionX, joueur.PositionY);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void BougerMonstreDansLaThread()
        {
            try
            {
                // Si la partie est en cours
                while (partieEnCours)
                {
                    // Applique le freeze time des monstres
                    Thread.Sleep((int)Monstre.freezeTime);

                    // Boucle dans le bassin de monstres
                    for (int i = 0; i < monstres.bassinMonstres.Count(); i++)
                    {
                        // Creer une variable temporaire pour faciliter la lecture du code
                        Monstre monstreTemp = monstres.bassinMonstres.ElementAt(i);


                        // Chaque monstre lance le de
                        monstreTemp.LancerDe();

                        BougerMonstre(monstreTemp);
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        static void BougerMonstre(Monstre monstreTemp)
        {
            try
            {
                // Selon le resultat du de
                switch (monstreTemp.de)
                {
                    // Bouge vers le bas
                    case 0:
                        //Si dans les limites
                        if (monstreTemp.EstDansLesLimitesDeLaCarte(monstreTemp.LimitePositionX, monstreTemp.LimitePositionY))
                        {
                            // Si le monstre entre en contact avec le joueur
                            if (carte.carte[monstreTemp.PositionY + 1][monstreTemp.PositionX] == Parametres.SYMBOLE_JOUEUR)
                            {
                                // Si le joueur n'est pas invisible
                                if (!(joueur.EtatActuel is EtatInvisible))
                                {
                                    // Le monstre attaque
                                    LeJoueurDefend(monstreTemp);

                                    // Si joueur est vivant
                                    if (joueur.EstVivant())
                                    {
                                        // Le joueur attaque
                                        LeJoueurAttaque(monstreTemp);
                                    }

                                    // Raffraichi l'UI pour afficher les messages d'attaque (s'il y a lieu)
                                    RaffraichirUI(carteChoisie);

                                    // Si le monstre est mort
                                    if (!monstreTemp.EstVivant())
                                    {
                                        // Supprime le monstre de la carte (niveau)
                                        AfficherSymbole(Parametres.SYMBOLE_VIDE, monstreTemp.PositionX, monstreTemp.PositionY);

                                        // Supprime le monstre du niveau
                                        SupprimerMonstre(monstreTemp);
                                    }
                                }
                                // Si le joueur est invisible
                                else
                                {
                                    // Deplacer le monstre
                                    DeplacerMonstre(monstreTemp, Parametres.AXE_Y, true);
                                }
                            }


                            // Si pas de mur
                            if (carte.carte[monstreTemp.PositionY + 1][monstreTemp.PositionX] != Parametres.SYMBOLE_MUR)
                            {
                                // Deplacer le monstre
                                DeplacerMonstre(monstreTemp, Parametres.AXE_Y, true);
                            }


                            // Si un mur devant lui
                            else
                            {
                                // Relance le de
                                //BougerMonstre(monstreTemp); // Creer une boucle infini qui fait planter le jeu (Stack Overflow)
                            }
                        }

                        break;

                    // Bouge vers le haut
                    case 1:
                        //Si dans les limites
                        if (monstreTemp.EstDansLesLimitesDeLaCarte(monstreTemp.LimitePositionX, monstreTemp.LimitePositionY))
                        {
                            // Si entre en contact avec le joueur
                            if (carte.carte[monstreTemp.PositionY - 1][monstreTemp.PositionX] == Parametres.SYMBOLE_JOUEUR)
                            {
                                // Si le joueur n'est pas invisible
                                if (!(joueur.EtatActuel is EtatInvisible))
                                {
                                    // Le monstre attaque
                                    LeJoueurDefend(monstreTemp);

                                    // Si joueur est vivant
                                    if (joueur.EstVivant())
                                    {
                                        // Le joueur attaque
                                        LeJoueurAttaque(monstreTemp);
                                    }

                                    // Raffraichi l'UI pour afficher les messages d'attaque (s'il y a lieu)
                                    RaffraichirUI(carteChoisie);

                                    // Si le monstre est mort
                                    if (!monstreTemp.EstVivant())
                                    {
                                        // Supprime le monstre de la carte (niveau)
                                        AfficherSymbole(Parametres.SYMBOLE_VIDE, monstreTemp.PositionX, monstreTemp.PositionY);

                                        // Supprime le monstre du niveau
                                        SupprimerMonstre(monstreTemp);
                                    }
                                }
                                // Si le joueur est invisible
                                else
                                {
                                    // Deplacer le monstre
                                    DeplacerMonstre(monstreTemp, Parametres.AXE_Y, false);
                                }
                            }


                            // Si pas de mur
                            else if (carte.carte[monstreTemp.PositionY - 1][monstreTemp.PositionX] != Parametres.SYMBOLE_MUR)
                            {
                                // Deplacer le monstre
                                DeplacerMonstre(monstreTemp, Parametres.AXE_Y, false);
                            }


                            // Si un mur devant lui
                            else
                            {
                                // Relance le de
                                //BougerMonstre(monstreTemp); // Creer une boucle infini qui fait planter le jeu (Stack Overflow)
                            }
                        }
                        break;

                    // Bouge vers la gauche
                    case 2:
                        //Si dans les limites
                        if (monstreTemp.EstDansLesLimitesDeLaCarte(monstreTemp.LimitePositionX, monstreTemp.LimitePositionY))
                        {
                            // Si le monstre entre en contact avec le joueur
                            if (carte.carte[monstreTemp.PositionY][monstreTemp.PositionX - 1] == Parametres.SYMBOLE_JOUEUR)
                            {
                                // Si le joueur n'est pas invisible
                                if (!(joueur.EtatActuel is EtatInvisible))
                                {
                                    // Le monstre attaque
                                    LeJoueurDefend(monstreTemp);

                                    // Si joueur est vivant
                                    if (joueur.EstVivant())
                                    {
                                        // Le joueur attaque
                                        LeJoueurAttaque(monstreTemp);
                                    }

                                    // Raffraichi l'UI pour afficher les messages d'attaque (s'il y a lieu)
                                    RaffraichirUI(carteChoisie);

                                    // Si le monstre est mort
                                    if (!monstreTemp.EstVivant())
                                    {
                                        // Supprime le monstre de la carte (niveau)
                                        AfficherSymbole(Parametres.SYMBOLE_VIDE, monstreTemp.PositionX, monstreTemp.PositionY);

                                        // Supprime le monstre du niveau
                                        SupprimerMonstre(monstreTemp);
                                    }
                                }
                                // Si le joueur est invisible
                                else
                                {
                                    // Deplacer le monstre
                                    DeplacerMonstre(monstreTemp, Parametres.AXE_X, false);
                                }
                            }

                            // Si pas de mur
                            else if (carte.carte[monstreTemp.PositionY][monstreTemp.PositionX - 1] != Parametres.SYMBOLE_MUR)
                            {
                                // Deplacer le monstre
                                DeplacerMonstre(monstreTemp, Parametres.AXE_X, false);
                            }


                            // Si un mur devant lui
                            else
                            {
                                // Relance le de
                                //BougerMonstre(monstreTemp); // Creer une boucle infini qui fait planter le jeu (Stack Overflow)
                            }
                        }
                        break;

                    // Bouge vers la droite
                    case 3:
                        //Si dans les limites
                        if (monstreTemp.EstDansLesLimitesDeLaCarte(monstreTemp.LimitePositionX, monstreTemp.LimitePositionY))
                        {
                            // Si le monstre entre en contact avec le joueur
                            if (carte.carte[monstreTemp.PositionY][monstreTemp.PositionX + 1] == Parametres.SYMBOLE_JOUEUR)
                            {
                                // Si le joueur n'est pas invisible
                                if (!(joueur.EtatActuel is EtatInvisible))
                                {
                                    // Le monstre attaque
                                    LeJoueurDefend(monstreTemp);

                                    // Si joueur est vivant
                                    if (joueur.EstVivant())
                                    {
                                        // Le joueur attaque
                                        LeJoueurAttaque(monstreTemp);
                                    }

                                    // Raffraichi l'UI pour afficher les messages d'attaque (s'il y a lieu)
                                    RaffraichirUI(carteChoisie);

                                    // Si le monstre est mort
                                    if (!monstreTemp.EstVivant())
                                    {
                                        // Supprime le monstre de la carte (niveau)
                                        AfficherSymbole(Parametres.SYMBOLE_VIDE, monstreTemp.PositionX, monstreTemp.PositionY);

                                        // Supprime le monstre du niveau
                                        SupprimerMonstre(monstreTemp);
                                    }
                                }
                                // Si le joueur est invisible
                                else
                                {
                                    // Deplacer le monstre
                                    DeplacerMonstre(monstreTemp, Parametres.AXE_X, true);
                                }
                            }


                            // Si pas de mur
                            else if (carte.carte[monstreTemp.PositionY][monstreTemp.PositionX + 1] != Parametres.SYMBOLE_MUR)
                            {
                                // Deplacer le monstre
                                DeplacerMonstre(monstreTemp, Parametres.AXE_X, true);
                            }


                            // Si un mur devant lui
                            else
                            {
                                // Relance le de
                                //BougerMonstre(monstreTemp); // Creer une boucle infini qui fait planter le jeu (Stack Overflow)
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        // J'ai essayer de faire un timer, mais je n'arriver pas a le faire fonctionner. Je vais utiliser les Threads a la place
        /*static void TimerCallBack(Object o)
        {
            //Console.WriteLine("Test");
            for (int i = 0; i > monstres.bassinMonstres.Count() - 1; i++)
            {
                //monstres.bassinMonstres.ElementAt(i).de = 0;//LancerDe();
                //BougerMonstre(monstres.bassinMonstres.ElementAt(i));

                // Si pas de mur
                if (carte.carte[monstres.bassinMonstres.ElementAt(i).PositionY + 1][monstres.bassinMonstres.ElementAt(i).PositionX] != Parametres.SYMBOLE_MUR)
                {
                    // Place le curseur a la place du monstre
                    Console.SetCursorPosition(monstres.bassinMonstres.ElementAt(i).PositionX, monstres.bassinMonstres.ElementAt(i).PositionY);

                    // Deplace le monstre
                    monstres.bassinMonstres.ElementAt(i).ChangerPosition(Parametres.AXE_Y, true);

                    //
                    AfficherSymbole(Parametres.SYMBOLE_MONSTRE, monstres.bassinMonstres.ElementAt(i).PositionX, monstres.bassinMonstres.ElementAt(i).PositionY);

                    // Replace le curseur sur le joueur
                    Console.SetCursorPosition(joueur.PositionX, joueur.PositionY);
                }
            }
        }*/
        #endregion
    }
}
