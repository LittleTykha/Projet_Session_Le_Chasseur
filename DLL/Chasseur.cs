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
    public class Chasseur : Personnage
    {
        // Proprietes
        private string nomChasseur = "";
        private int score = 0;
        public Queue<string> historiqueInfo = new Queue<string>();
        public Queue<Pic> inventairePic = new Queue<Pic>();
        public Queue<Epee> inventaireEpee = new Queue<Epee>();
        public Queue<Bouclier> inventaireBouclier = new Queue<Bouclier>();
        private IEtatChasseur etatActuel;


        // Getter/Setter
        public string NomChasseur
        {
            get { return nomChasseur; }
            set
            {
                // Si nom trop long
                if (value.Trim().Length > Parametres.NBR_MAX_CHARACTERE)
                {
                    ErreurValidation = Parametres.ERREUR_NOM_TROP_LONG;
                }
                else if (value.Trim() == "")
                {
                    ErreurValidation = Parametres.ERREUR_NOM_VIDE;
                }
                else
                {
                    nomChasseur = value;
                    ErreurValidation = "";
                }
            }
        }

        public IEtatChasseur EtatActuel
        {
            get => etatActuel;
            set => etatActuel = value;
        }

        public int Score
        {
            get => score;
            set => score = value;
        }

        // Constructor
        public Chasseur() { }

        public Chasseur(string nom, byte positionX, byte positionY) : base(positionX, positionY)
        {
            this.NomChasseur = nom;
            this.curHP = maxHP;
            this.freezeTime = 1000f;
        }

        public Chasseur(string nom, byte positionX, byte positionY, byte largeurCarte, byte hauteurCarte) : base(positionX, positionY, largeurCarte, hauteurCarte)
        {
            this.NomChasseur = nom;
            this.curHP = maxHP;
            this.freezeTime = 1000f;
            this.curAP = 7;
            this.curDP = 4;
            this.etatActuel = EtatNormal.ObtenirInstance();
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

                // Ajoute les points de survie (effectuer n'importe quelle action donne des points de survie)
                this.score += Parametres.POINT_SURVIE;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void AjouterHistoriqueInfo(string action)
        {
            try
            {
                // Si le nombre de message dépasse le nombre max de ligne devant etre affichés
                if (historiqueInfo.Count >= Parametres.MAX_LIGNE_HISTORIQUE_INFO)
                {
                    // Retire la première action ajoutée
                    historiqueInfo.Dequeue();
                }
                // Ajoute la nouvelle action
                historiqueInfo.Enqueue(action);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public string Attaquer(Monstre cible)
        {
            try
            {
                byte degats = etatActuel.CalculerDommage(this);

                string degat = cible.PrendreDesDegats((byte)(degats + (this.inventaireEpee.Count() > 0 ? this.inventaireEpee.Peek().AP : 0)));

                return $"{this.nomChasseur} attaque le monstre. " + (cible.EstVivant() ? $"La cible perd {degat} HP. Il lui reste {cible.CurHP} HP           " : $"La cible est morte.                                ");
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }

        public void VerifierEpee()
        {
            try
            {
                // Si le joueur possedait une epee au moment de l'attaque
                if (this.inventaireEpee.Count() > 0)
                {
                    // Verifie l'etat de l'epee et retirer de l'inventaire si brise
                    if (this.inventaireEpee.Peek().VerifierEpee())
                    {
                        // Retire l'epee de l'inventaire
                        this.inventaireEpee.Dequeue();

                        // Ajoute l'action a info
                        this.AjouterHistoriqueInfo(Parametres.MESSAGE_EPEE_DETRUIT);
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void VerifierBouclier()
        {
            try
            {
                // Si le joueur possedait un bouclier au moment de l'attaque
                if (this.inventaireBouclier.Count() > 0)
                {
                    // Verifie l'etat du bouclier et retirer de l'inventaire si brise
                    if (this.inventaireBouclier.Peek().VerifierBouclier())
                    {
                        // Retire le bouclier de l'inventaire
                        this.inventaireBouclier.Dequeue();

                        // Ajoute l'action a info
                        this.AjouterHistoriqueInfo(Parametres.MESSAGE_BOUCLIER_DETRUIT);
                    }
                }
            }
            catch(Exception e) 
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void VerifierPic()
        {
            try
            {
                // Verifie l'etat du pic et retirer de l'inventaire si brise
                if (this.inventairePic.Peek().VerifierPic())
                {
                    // Retire le pic de l'inventaire
                    this.inventairePic.Dequeue();

                    // Ajoute l'action a info
                    this.AjouterHistoriqueInfo(Parametres.MESSAGE_PIC_DETRUIT);
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void DetruireMur(Carte carte, byte positionX, byte positionY, char axe, bool EstPositif)
        {
            try
            {
                if (axe == Parametres.AXE_Y)
                {
                    if (EstPositif) { ++positionY; }
                    else { --positionY; }
                }
                else
                {
                    if (EstPositif) { ++positionX; }
                    else { --positionX; }
                }
                // Retire le mur de la carte (tableau)
                carte.carte[positionY][positionX] = ' ';

                // Ajoute l'action a info
                this.AjouterHistoriqueInfo(Parametres.ACTION_DETRUIRE_MUR);

                // Ajoute des points
                this.Score += Parametres.POINT_DETRUIRE_MUR;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void RamasserPic(Carte carte)
        {
            try
            {
                foreach (Pic pic in Pic.bassinPics)
                {
                    if (this.PositionX == pic.positionX && this.PositionY == pic.positionY)
                    {
                        // Ajoute l'action au info en fonction de l'inventaire actuel
                        this.AjouterHistoriqueInfo(this.inventairePic.Count() == 0 ? Parametres.ACTION_PRENDRE_UN_PIC : Parametres.ACTION_PRENDRE_UN_NOUVEAU_PIC);

                        // Si n'avait pas deja un pic
                        if (this.inventairePic.Count() == 0)
                        {
                            // Ajoute un pic a l'inventaire
                            this.inventairePic.Enqueue(pic);
                        }

                        // Ajoute des points
                        this.Score += Parametres.POINT_RAMMASSER_OBJET;

                        // Retire le pic de la carte
                        carte.carte[pic.positionY][pic.positionX] = ' ';

                        // Retire le pic du bassin de pic
                        Pic.bassinPics.Remove(pic);

                        // Quitte le foreach
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void RamasserBouclier(Carte carte)
        {
            try
            {
                foreach (Bouclier bouclier in Bouclier.bassinBoucliers)
                {
                    if (this.PositionX == bouclier.positionX && this.PositionY == bouclier.positionY)
                    {
                        // Ajoute un pic a l'inventaire
                        this.inventaireBouclier.Enqueue(bouclier);

                        // Ajoute des points
                        this.Score += Parametres.POINT_RAMMASSER_OBJET;

                        // Ajoute l'action au info
                        this.AjouterHistoriqueInfo(Parametres.ACTION_PRENDRE_UN_BOUCLIER);

                        // Retire le pic de la carte
                        carte.carte[bouclier.positionY][bouclier.positionX] = ' ';

                        // Retire le pic du bassin de pic
                        Bouclier.bassinBoucliers.Remove(bouclier);

                        // Quitte le foreach
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void RamasserEpee(Carte carte)
        {
            try
            {
                foreach (Epee epee in Epee.bassinEpees)
                {
                    if (this.PositionX == epee.positionX && this.PositionY == epee.positionY)
                    {
                        // Ajoute un pic a l'inventaire
                        this.inventaireEpee.Enqueue(epee);

                        // Ajoute des points
                        this.Score += Parametres.POINT_RAMMASSER_OBJET;

                        // Ajoute l'action au info
                        this.AjouterHistoriqueInfo(Parametres.ACTION_PRENDRE_UNE_EPEE);

                        // Retire le pic de la carte
                        carte.carte[epee.positionY][epee.positionX] = ' ';

                        // Retire le pic du bassin de pic
                        Epee.bassinEpees.Remove(epee);

                        // Quitte le foreach
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void RamasserPotion(Carte carte)
        {
            try
            {
                foreach (Potion potion in Potion.bassinPotions)
                {
                    if (this.PositionX == potion.positionX && this.PositionY == potion.positionY)
                    {
                        // Ajoute des points
                        this.Score += Parametres.POINT_RAMMASSER_OBJET;

                        // Ajoute l'action au info
                        this.AjouterHistoriqueInfo(Parametres.ACTION_PRENDRE_UNE_POTION + potion.type + "              ");

                        // Modifie l'etat du joueur selon le type de potion
                        switch (potion.type)
                        {
                            // Si potion de force
                            case TypePotion.Force:
                                // Change l'etat pour force
                                this.etatActuel = EtatForce.ObtenirInstance();
                                this.curHP = maxHP;
                                this.AjouterHistoriqueInfo(Parametres.ETAT_FORT);
                                break;

                            // Si potion de vitesse
                            case TypePotion.Vitesse:
                                // Change l'etat pour vitesse
                                this.etatActuel = EtatVitesse.ObtenirInstance();
                                this.AjouterHistoriqueInfo(Parametres.ETAT_VITESSE);
                                this.freezeTime = etatActuel.CalculerVitesse(this);
                                break;

                            // Si potion d'invisibilite
                            case TypePotion.Invisibilite:
                                // Change l'etat pour invisible
                                this.etatActuel = EtatInvisible.ObtenirInstance();
                                this.AjouterHistoriqueInfo(Parametres.ETAT_INVISIBLE);
                                break;

                            // Si potion empoisonnee
                            case TypePotion.Empoisonnee:
                                // Change l'etat pour poison
                                this.etatActuel = EtatPoison.ObtenirInstance();
                                this.AjouterHistoriqueInfo(Parametres.ETAT_EMPOISONNE);
                                break;
                        }

                        // Retire le pic de la carte
                        carte.carte[potion.positionY][potion.positionX] = ' ';

                        // Retire le pic du bassin de pic
                        Potion.bassinPotions.Remove(potion);

                        // Quitte le foreach
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
