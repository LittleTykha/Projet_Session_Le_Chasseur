/*
 * Project Name: Jeu_Graph
 * Student Name: Patrick Tremblay
 * Student ID:   2312796
 * Date:         Oct 27th 2023
 * Version:      1
 * Description:  Projet de Session : Jeu Graphique
*/

using DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeu_Graph
{
    public partial class FrmJeu : Form
    {
        // Constantes
        const string ENTER = "\r\n";


        // Proprietes publiques
        public Carte carte = new Carte();
        public Chasseur joueur = new Chasseur();
        public string carteChoisie = "";


        // Proprietes privees
        private Monstres monstres = new Monstres();
        private List<SpritePic> listePic = new List<SpritePic>();
        private Thread threadJoueur;
        private Thread threadMonstre;
        private PictureBox picJoueur;
        private PictureBox picObjectif;
        private byte niveau = 1;
        private bool EnTrainDeBouger = false;
        private bool partieEnCours = false;
        private bool aGagner = false;
        private delegate void DeplacerJoueurEntreLesThreads(int velociteX, int velociteY);
        private delegate void DeplacerMonstreEntreLesThreads(Monstre monstre);
        private delegate void AfficherInfoEntreLesThreads();
        private delegate void SupprimerMonstreEntreLesThreads(Monstre monstre);


        // Constructeur
        public FrmJeu()
        {
            InitializeComponent();
        }

        #region EVENEMENTS
        private void FrmJeu_Load(object sender, EventArgs e)
        {
            try
            {
                // Charge la carte choisie
                carte.ChargerCarte(carteChoisie, ref joueur, ref monstres);


                // Ajoute les pics a la liste de pics
                RemplirListePic();


                // Ajuster l'UI
                AjusterElementUI();


                // Afficher UI
                AfficherUI();


                // Dessine la carte choisie
                DessinerCarte(carte);


                // Raffraichir l'ecran pour etre certain que les pics soient dessines
                this.Refresh();


                // Debute la partie
                partieEnCours = true;


                // Assigne le freezetiem des monstres au timer
                timerMonstre.Interval = (int)Monstre.freezeTime;

                // Active le timer
                timerMonstre.Enabled = true;
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void FrmJeu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Si la partie est en vours
                if (partieEnCours)
                {
                    // Si le joueur n'est pas en train de bouger
                    if (!EnTrainDeBouger)
                    {
                        #region MOUVEMENT
                        // Deplacer le joueur en fonction de la touche et les limites de la carte
                        switch (e.KeyCode)
                        {
                            // Bas
                            case Keys.S:
                                //Si dans les limites
                                if (joueur.PositionY < joueur.LimitePositionY)
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

                                                        // Quitte la boucle
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        // Si est invisible
                                        else
                                        {
                                            // Deplace le joueur DANS la carte
                                            joueur.ChangerPosition(Parametres.AXE_Y, true);

                                            // Creer une thread secondaire pour deplacer le joueur
                                            threadJoueur = new Thread(new ThreadStart(this.DeplacerJoueurLentementVersLeBasDansThread));
                                            threadJoueur.IsBackground = true;
                                            threadJoueur.Start();

                                            // Ajoute l'action a info
                                            joueur.AjouterHistoriqueInfo(Parametres.ACTION_BAS);
                                        }

                                        // Applique le freezetime
                                        Attendre();
                                    }

                                    // Si pas de mur
                                    else if (carte.carte[joueur.PositionY + 1][joueur.PositionX] != Parametres.SYMBOLE_MUR)
                                    {
                                        // Deplace le joueur DANS la carte
                                        carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_VIDE;
                                        joueur.ChangerPosition(Parametres.AXE_Y, true);
                                        carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_JOUEUR;

                                        // Creer une thread secondaire pour deplacer le joueur
                                        threadJoueur = new Thread(new ThreadStart(this.DeplacerJoueurLentementVersLeBasDansThread));
                                        threadJoueur.IsBackground = true;
                                        threadJoueur.Start();

                                        // Ajoute l'action a info
                                        joueur.AjouterHistoriqueInfo(Parametres.ACTION_BAS);
                                    }

                                    // Si un mur et possede un pic
                                    else if (joueur.inventairePic.Count != 0)
                                    {
                                        // Retire le mur de la carte (tableau)
                                        joueur.DetruireMur(carte, joueur.PositionX, joueur.PositionY, Parametres.AXE_Y, true);

                                        // Supprime le mur du niveau
                                        SupprimerMur(joueur.PositionX, (byte)(joueur.PositionY + 1));

                                        // Verifie l'etat du pic et le retirer de l'inventaire si brise
                                        joueur.VerifierPic();

                                        // Applique le freezetime
                                        Attendre();
                                    }
                                }
                                break;

                            //Haut
                            case Keys.W:
                                //Si dans les limites
                                if (joueur.PositionY > 0)
                                {
                                    // Si le joueur entre en contact avec un monstre
                                    if (carte.carte[joueur.PositionY - 1][joueur.PositionX] == Parametres.SYMBOLE_MONSTRE)
                                    {
                                        // Si le joueur n'est pas invisible
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

                                                        // Quitte la boucle
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        // Si le joueur est visible
                                        else
                                        {
                                            // Deplace le joueur DANS la carte
                                            joueur.ChangerPosition(Parametres.AXE_Y, false);

                                            // Creer une thread secondaire pour deplacer le joueur
                                            threadJoueur = new Thread(new ThreadStart(this.DeplacerJoueurLentementVersLeHautDansThread));
                                            threadJoueur.IsBackground = true;
                                            threadJoueur.Start();

                                            // Ajoute l'action a info
                                            joueur.AjouterHistoriqueInfo(Parametres.ACTION_HAUT);
                                        }
                                        // Applique le freezetime
                                        Attendre();
                                    }

                                    // Si pas de mur
                                    else if (carte.carte[joueur.PositionY - 1][joueur.PositionX] != Parametres.SYMBOLE_MUR)
                                    {
                                        // Deplace le joueur DANS la carte
                                        carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_VIDE;
                                        joueur.ChangerPosition(Parametres.AXE_Y, false);
                                        carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_JOUEUR;

                                        // Creer une thread secondaire pour deplacer le joueur
                                        threadJoueur = new Thread(new ThreadStart(this.DeplacerJoueurLentementVersLeHautDansThread));
                                        threadJoueur.IsBackground = true;
                                        threadJoueur.Start();

                                        // Ajoute l'action a info
                                        joueur.AjouterHistoriqueInfo(Parametres.ACTION_HAUT);
                                    }

                                    // Si un mur et possede un pic
                                    else if (joueur.inventairePic.Count != 0)
                                    {
                                        // Retire le mur de la carte
                                        joueur.DetruireMur(carte, joueur.PositionX, joueur.PositionY, Parametres.AXE_Y, false);

                                        // Supprime le mur du niveau
                                        SupprimerMur(joueur.PositionX, (byte)(joueur.PositionY - 1));

                                        // Verifie l'etat du pic et retirer de l'inventaire si brise
                                        joueur.VerifierPic();

                                        // Applique le freezetime
                                        Attendre();
                                    }
                                }
                                break;

                            // Gauche
                            case Keys.A:
                                //Si dans les limites
                                if (joueur.PositionX > 0)
                                {
                                    // Si le joueur entre en contact avec un monstre
                                    if (carte.carte[joueur.PositionY][joueur.PositionX - 1] == Parametres.SYMBOLE_MONSTRE)
                                    {
                                        // Si le joueur n'est pas invisible
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

                                                    // Quitte la boucle
                                                    break;
                                                }
                                            }
                                        }
                                        // Si le joueur est invisible
                                        else
                                        {
                                            // Deplace le joueur DANS la carte
                                            joueur.ChangerPosition(Parametres.AXE_X, false);

                                            // Creer une thread secondaire pour deplacer le joueur
                                            threadJoueur = new Thread(new ThreadStart(this.DeplacerJoueurLentementVersLaGaucheDansThread));
                                            threadJoueur.IsBackground = true;
                                            threadJoueur.Start();

                                            // Ajoute l'action a info
                                            joueur.AjouterHistoriqueInfo(Parametres.ACTION_GAUCHE);
                                        }

                                        // Applique le freezetime
                                        Attendre();
                                    }

                                    // Si pas de mur
                                    else if (carte.carte[joueur.PositionY][joueur.PositionX - 1] != Parametres.SYMBOLE_MUR)
                                    {
                                        // Deplace le joueur DANS la carte
                                        carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_VIDE;
                                        joueur.ChangerPosition(Parametres.AXE_X, false);
                                        carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_JOUEUR;

                                        // Creer une thread secondaire pour deplacer le joueur
                                        threadJoueur = new Thread(new ThreadStart(this.DeplacerJoueurLentementVersLaGaucheDansThread));
                                        threadJoueur.IsBackground = true;
                                        threadJoueur.Start();

                                        // Ajoute l'action a info
                                        joueur.AjouterHistoriqueInfo(Parametres.ACTION_GAUCHE);
                                    }

                                    // Si un mur et possede un pic
                                    else if (joueur.inventairePic.Count != 0)
                                    {
                                        // Retire le mur de la carte
                                        joueur.DetruireMur(carte, joueur.PositionX, joueur.PositionY, Parametres.AXE_X, false);

                                        // Supprime le mur du niveau
                                        SupprimerMur((byte)(joueur.PositionX - 1), joueur.PositionY);

                                        // Verifie l'etat du pic et retirer de l'inventaire si brise
                                        joueur.VerifierPic();

                                        // Applique le freezetime
                                        Attendre();
                                    }
                                }
                                break;

                            // Droite
                            case Keys.D:
                                //Si dans les limites
                                if (joueur.PositionX < carte.carte[joueur.PositionY].GetLength(0) - 1)
                                {
                                    // Si le joueur entre en contact avec un monstre
                                    if (carte.carte[joueur.PositionY][joueur.PositionX + 1] == Parametres.SYMBOLE_MONSTRE)
                                    {
                                        // Si le joueur n'est pas invisible
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
                                                    //aAttaque = true;

                                                    if (monstre.EstVivant())
                                                    {
                                                        // Le monstre attaque
                                                        LeJoueurDefend(monstre);
                                                    }
                                                    else
                                                    {
                                                        // Supprime le monstre de la carte (tableau)
                                                        SupprimerMonstre(monstre);

                                                        // Quitte la boucle
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        // Si le joueur est invisible
                                        else
                                        {
                                            // Deplace le joueur DANS la carte
                                            joueur.ChangerPosition(Parametres.AXE_X, true);

                                            // Creer une thread secondaire pour deplacer le joueur
                                            threadJoueur = new Thread(new ThreadStart(this.DeplacerJoueurLentementVersLaDroiteDansThread));
                                            threadJoueur.IsBackground = true;
                                            threadJoueur.Start();

                                            // Ajoute l'action a info
                                            joueur.AjouterHistoriqueInfo(Parametres.ACTION_DROITE);
                                        }
                                        // Applique le freezetime
                                        Attendre();
                                    }

                                    // Si pas de mur
                                    else if (carte.carte[joueur.PositionY][joueur.PositionX + 1] != Parametres.SYMBOLE_MUR)
                                    {
                                        // Deplace le joueur DANS la carte
                                        carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_VIDE;
                                        joueur.ChangerPosition(Parametres.AXE_X, true);
                                        carte.carte[joueur.PositionY][joueur.PositionX] = Parametres.SYMBOLE_JOUEUR;

                                        // Creer une thread secondaire pour deplacer le joueur
                                        threadJoueur = new Thread(new ThreadStart(this.DeplacerJoueurLentementVersLaDroiteDansThread));
                                        threadJoueur.IsBackground = true;
                                        threadJoueur.Start();

                                        // Ajoute l'action a info
                                        joueur.AjouterHistoriqueInfo(Parametres.ACTION_DROITE);
                                    }

                                    // Si un mur et possede un pic
                                    else if (joueur.inventairePic.Count != 0)
                                    {
                                        // Retire le mur de la carte
                                        joueur.DetruireMur(carte, joueur.PositionX, joueur.PositionY, Parametres.AXE_X, true);

                                        // Supprime le mur du niveau
                                        SupprimerMur((byte)(joueur.PositionX + 1), joueur.PositionY);

                                        // Verifie l'etat du pic et retirer de l'inventaire si brise
                                        joueur.VerifierPic();

                                        // Applique le freezetime
                                        Attendre();
                                    }
                                }
                                break;
                        }
                        #endregion

                        #region POST_MOUVEMENT
                        // Si le joueur atteint un pic
                        VerifierSiAtteintPic();


                        // Si le joueur atteint un bouclier
                        VerifierSiAtteintBouclier();


                        // Si le joueur atteint une epee
                        VerifierSiAtteintEpee();


                        // Si le joueur atteint une potion
                        VerifierSiAtteintPotion();


                        // Si le joueur atteint l'objectif
                        if (joueur.PositionX == carte.positionObjectifX && joueur.PositionY == carte.positionObjectifY)
                        {
                            // Gagne la partie
                            aGagner = true;

                            // Affiche l'UI de fin de partie selon que le joueur est gagnant ou non
                            AfficherUIFinDePartie(aGagner);

                            // Ajoute des points
                            joueur.Score += Parametres.POINT_OBJECTIF;

                            // Arrete le timer Momnstre
                            timerMonstre.Stop();
                        }


                        // Si le joueur meurt
                        if (!joueur.EstVivant())
                        {
                            // Supprime le joueur du niveau
                            pnlCarte.Controls.Remove(picJoueur);

                            // Affiche l'UI de fin de partie selon que le joueur est gagnant ou non
                            AfficherUIFinDePartie(aGagner);
                        }

                        // Actualise l'UI a la fin du "tour"
                        AfficherUI();


                        // Afficher les actions du "tour"
                        AfficherAction();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btnNiveauSuivant_Click(object sender, EventArgs e)
        {
            try
            {
                // Prepare le passage au niveau suivant
                PasserAuNiveauSuivant();


                // Ajoute les pics a la liste de pics
                RemplirListePic();


                // Ajuster l'UI
                AjusterElementUI();


                // Afficher UI
                AfficherUI();


                //Masquer l'UI de fin de partie
                MasquerUIFinDePartie();


                // Dessine la carte choisie
                DessinerCarte(carte);


                // Vide le contenu du textboc Info
                tbInfoAction.Clear();


                // Raffraichir l'ecran pour etre certain que les pics soient dessines
                this.Refresh();


                // Debute la partie
                partieEnCours = true;


                // Re-initialise aGagne
                aGagner = false;
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btnRecommencer_Click(object sender, EventArgs e)
        {
            try
            {
                // Retire tous les picturebox de la fenetres pour qu'ils ne s'empilent pas d'un niveau a l'autre
                pnlCarte.Controls.Clear();

                // Recharge la carte
                carte.ChargerCarte(carteChoisie, ref joueur, ref monstres);

                // Ajoute les pics a la liste de pics
                RemplirListePic();


                // Ajuster l'UI
                AjusterElementUI();


                // Afficher UI
                AfficherUI();


                //Masquer l'UI de fin de partie
                MasquerUIFinDePartie();


                // Dessine la carte choisie
                DessinerCarte(carte);


                // Vide le contenu du textboc Info
                tbInfoAction.Clear();


                // Raffraichir l'ecran pour etre certain que les pics soient dessines
                this.Refresh();


                // Debute la partie
                partieEnCours = true;
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void timerMonstre_Tick(object sender, EventArgs e)
        {
            try
            {
                threadMonstre = new Thread(new ThreadStart(this.DeplacerMonstreLentementDansThread));
                threadMonstre.IsBackground = true;
                threadMonstre.Start();
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void pnlCarte_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // Boucle dans la liste de pic
                foreach (SpritePic pic in listePic)
                {
                    // Dessine le pic
                    pic.DessinerPic(e.Graphics);
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void FrmJeu_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // Enregistre le score
                Score.AjouterScore(new Pointage(joueur.NomChasseur, joueur.Score));


                // Si une threadJoueur est active
                if (threadJoueur != null)
                {
                    // Annuler la thread
                    threadJoueur.Abort();
                }

                // Si une threadMonstre est active
                if (threadMonstre != null)
                {
                    // Annuler la thread
                    threadMonstre.Abort();
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

        #region METHODES
        private void DessinerCarte(Carte carte)
        {
            try
            {
                // Boucler dans la 1er dim du tableau -> Ligne
                for (int y = 0; y < carte.carte.GetLength(0); y++)
                {
                    // 2em dim -> colone
                    for (int x = 0; x < carte.carte[y].Length; x++)
                    {
                        // Selon l'element de la carte
                        switch (carte.carte[y][x])
                        {
                            // Si l'element de la carte est le joueur
                            case Parametres.SYMBOLE_JOUEUR:

                                // Cree un picture box pour le joueur
                                picJoueur = new PictureBox
                                {
                                    Name = "picJoueur",
                                    Size = new Size(Parametres.TAILLE_BLOC, Parametres.TAILLE_BLOC),
                                    Location = new Point(x * Parametres.TAILLE_BLOC, y * Parametres.TAILLE_BLOC),
                                    Image = Properties.Resources.Joueur,
                                    SizeMode = PictureBoxSizeMode.StretchImage
                                };

                                // Ajoute le joueur dans la fenetre de jeu
                                pnlCarte.Controls.Add(picJoueur);
                                break;

                            // Si l'element de la carte est un monstre
                            case Parametres.SYMBOLE_MONSTRE:

                                // Cree un picture box pour le monstre
                                PictureBox monstreTemp = new PictureBox
                                {
                                    //Name = monstres.bassinMonstres.ElementAt(indexMonstre++).Nom,
                                    Size = new Size(Parametres.TAILLE_BLOC, Parametres.TAILLE_BLOC),
                                    Location = new Point(x * Parametres.TAILLE_BLOC, y * Parametres.TAILLE_BLOC),
                                    Image = Properties.Resources.Monstre,
                                    BackColor = Color.Transparent,
                                    SizeMode = PictureBoxSizeMode.StretchImage
                                };

                                // Boucle dans le bassin de monstre
                                foreach (Monstre monstre in monstres.bassinMonstres)
                                {
                                    // Si le picturebox est a la meme position qu'un monstre dans la carte (tableau)
                                    if (monstreTemp.Location.X == monstre.PositionX * Parametres.TAILLE_BLOC && monstreTemp.Location.Y == monstre.PositionY * Parametres.TAILLE_BLOC)
                                    {
                                        // Assigne le nom du monstre au picture box
                                        monstreTemp.Name = monstre.Nom;
                                    }
                                }

                                // Ajoute le monstre dans la fenetre de jeu
                                pnlCarte.Controls.Add(monstreTemp);
                                break;

                            // Si l'element de la carte est l'objectif
                            case Parametres.SYMBOLE_OBJECTIF:

                                // Cree un picture box pour l'objectif
                                picObjectif = new PictureBox
                                {
                                    Name = "picObjectif",
                                    Size = new Size(Parametres.TAILLE_BLOC, Parametres.TAILLE_BLOC),
                                    Location = new Point(x * Parametres.TAILLE_BLOC, y * Parametres.TAILLE_BLOC),
                                    Image = Properties.Resources.Objectif,
                                    SizeMode = PictureBoxSizeMode.StretchImage
                                };

                                // Ajoute l'objectif dans la fenetre de jeu
                                pnlCarte.Controls.Add(picObjectif);
                                break;

                            // Si l'element de la carte est une epee
                            case Parametres.SYMBOLE_EPEE:

                                // Cree un picture box pour l'epee
                                PictureBox epeeTemp = new PictureBox
                                {
                                    Name = "picEpee" + x + "x" + y,
                                    Size = new Size(Parametres.TAILLE_BLOC, Parametres.TAILLE_BLOC),
                                    Location = new Point(x * Parametres.TAILLE_BLOC, y * Parametres.TAILLE_BLOC),
                                    Image = Properties.Resources.Epee,
                                    SizeMode = PictureBoxSizeMode.StretchImage
                                };

                                // Ajoute l'epee dans la fenetre de jeu
                                pnlCarte.Controls.Add(epeeTemp);
                                break;

                            // Si l'element de la carte est un bouclier
                            case Parametres.SYMBOLE_BOUCLIER:

                                // Cree un picture box pour le bouclier
                                PictureBox bouclierTemp = new PictureBox
                                {
                                    Name = "picBouclier" + x + "x" + y,
                                    Size = new Size(Parametres.TAILLE_BLOC, Parametres.TAILLE_BLOC),
                                    Location = new Point(x * Parametres.TAILLE_BLOC, y * Parametres.TAILLE_BLOC),
                                    Image = Properties.Resources.Bouclier,
                                    SizeMode = PictureBoxSizeMode.StretchImage
                                };

                                // Ajoute le bouclier dans la fenetre de jeu
                                pnlCarte.Controls.Add(bouclierTemp);
                                break;

                            // Si l'element de la carte est une potion
                            case Parametres.SYMBOLE_POTION:

                                // Cree un picture box pour la potion
                                PictureBox potionTemp = new PictureBox
                                {
                                    Name = "picPotion" + x + "x" + y,
                                    Size = new Size(Parametres.TAILLE_BLOC, Parametres.TAILLE_BLOC),
                                    Location = new Point(x * Parametres.TAILLE_BLOC, y * Parametres.TAILLE_BLOC),
                                    Image = Properties.Resources.Potion,
                                    SizeMode = PictureBoxSizeMode.StretchImage
                                };

                                // Ajoute la potion dans la fenetre de jeu
                                pnlCarte.Controls.Add(potionTemp);
                                break;

                            // Si l'element de la carte est un mur
                            case Parametres.SYMBOLE_MUR:

                                // Cree un picture box pour le mur
                                PictureBox murTemp = new PictureBox
                                {
                                    Name = "picMur" + x + "x" + y,
                                    Size = new Size(Parametres.TAILLE_BLOC, Parametres.TAILLE_BLOC),
                                    Location = new Point(x * Parametres.TAILLE_BLOC, y * Parametres.TAILLE_BLOC),
                                    Image = Properties.Resources.Mur,
                                    SizeMode = PictureBoxSizeMode.StretchImage
                                };

                                // Ajoute le mur dans la fenetre de jeu
                                pnlCarte.Controls.Add(murTemp);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void LeJoueurAttaque(Monstre monstre)
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

        public void LeJoueurDefend(Monstre monstre)
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

        private void SupprimerMonstre(Monstre monstre)
        {
            try
            {
                // Cherche un picture box correspondant au nom du monstre tue
                PictureBox monstreASupprimer = (PictureBox)pnlCarte.Controls.Find(monstre.Nom, false).FirstOrDefault();

                // Supprime le monstre du niveau
                pnlCarte.Controls.Remove(monstreASupprimer);

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

        private void SupprimerMur(byte posX, byte posY)
        {
            try
            {
                // Generer nom du mur
                string nomMur = "picMur" + posX + "x" + posY;

                // Chercher le mur dans la liste de tous les control
                PictureBox picMurADetruire = (PictureBox)pnlCarte.Controls.Find(nomMur, false).FirstOrDefault();

                // Detuire le mur
                pnlCarte.Controls.Remove(picMurADetruire);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void RemplirListePic()
        {
            try
            {
                // Si la liste est deja chargee
                if (listePic.Count > 0)
                {
                    // Vide la liste de pics
                    listePic.Clear();
                }

                // Boucle dans la carte 1er dimension
                for (int y = 0; y < carte.carte.GetLength(0); y++)
                {
                    // 2em dim -> colone
                    for (int x = 0; x < carte.carte[y].Length; x++)
                    {
                        // Si l'element de la carte est un pic
                        if (carte.carte[y][x] == Parametres.SYMBOLE_PIC)
                        {
                            // Creer un pic
                            listePic.Add(new SpritePic(new Point(x * Parametres.TAILLE_BLOC, y * Parametres.TAILLE_BLOC), Color.Black));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void VerifierSiAtteintPic()
        {
            try
            {
                for (int i = 0; i < Pic.bassinPics.Count(); i++)
                {
                    if (joueur.PositionX == Pic.bassinPics.ElementAt(i).positionX && joueur.PositionY == Pic.bassinPics.ElementAt(i).positionY)
                    {
                        // Le joueur ramasse le pic
                        joueur.RamasserPic(carte);

                        // On efface le pic
                        listePic.RemoveAt(i);

                        // Rafraichir la carte
                        this.Refresh();

                        // Quitte le foreach pour evite le "out of index"
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void VerifierSiAtteintBouclier()
        {
            try
            {
                foreach (Bouclier bouclier in Bouclier.bassinBoucliers)
                {
                    if (joueur.PositionX == bouclier.positionX && joueur.PositionY == bouclier.positionY)
                    {
                        string nomBouclier = "picBouclier" + joueur.PositionX + "x" + joueur.PositionY;
                        PictureBox bouclierAPrendre = (PictureBox)pnlCarte.Controls.Find(nomBouclier, false).FirstOrDefault();

                        // Le joueur ramasse le pic
                        joueur.RamasserBouclier(carte);

                        // On retire le bouclier du paneau de carte
                        pnlCarte.Controls.Remove(bouclierAPrendre);

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void VerifierSiAtteintEpee()
        {
            try
            {
                foreach (Epee epee in Epee.bassinEpees)
                {
                    if (joueur.PositionX == epee.positionX && joueur.PositionY == epee.positionY)
                    {
                        string nomEpee = "picEpee" + joueur.PositionX + "x" + joueur.PositionY;
                        PictureBox epeeAPrendre = (PictureBox)pnlCarte.Controls.Find(nomEpee, false).FirstOrDefault();

                        // Le joueur ramasse le pic
                        joueur.RamasserEpee(carte);

                        // On retire le bouclier du paneau de carte
                        pnlCarte.Controls.Remove(epeeAPrendre);

                        break;

                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void VerifierSiAtteintPotion()
        {
            try
            {
                foreach (Potion potion in Potion.bassinPotions)
                {
                    if (joueur.PositionX == potion.positionX && joueur.PositionY == potion.positionY)
                    {
                        string nomPotion = "picPotion" + joueur.PositionX + "x" + joueur.PositionY;
                        PictureBox potionAPrendre = (PictureBox)pnlCarte.Controls.Find(nomPotion, false).FirstOrDefault();

                        // Le joueur ramasse le pic
                        joueur.RamasserPotion(carte);

                        // On retire le bouclier du paneau de carte
                        pnlCarte.Controls.Remove(potionAPrendre);

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void AjusterElementUI()
        {
            try
            {
                // Change le texte du label win
                lblWin.Text = $"Félicitation, {joueur.NomChasseur}!";

                // Change le texte du label lose
                lblLose.Text = Parametres.PERDRE_PARTIE;

                // Ajuste la position du paneau de score avec celui de la carte
                gbInfoJoueur.Location = new Point(0, carte.carte.GetLength(0) * Parametres.TAILLE_BLOC);

                // Ajuster la position du paneau dèinfo en fonction du paneau de score et de la carte
                gbInfo.Location = new Point(gbInfoJoueur.Right, carte.carte.GetLength(0) * Parametres.TAILLE_BLOC);

                // Ajuster la largeur du paneau info
                gbInfo.Width = (carte.carte[carte.HauteurCarte - 1].GetLength(0) * Parametres.TAILLE_BLOC) - gbInfoJoueur.Width;

                // Ajuster la largeur du textbox info selon celle du paneau info
                tbInfoAction.Width = gbInfo.Width - 10;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void AfficherUI()
        {
            try
            {
                // Modifier le nom de la fenetre jeu
                this.Text = Parametres.TITRE_PROJET + " - " + joueur.NomChasseur + " - " + carteChoisie;

                // Afficher le nom de la carte
                lblCarteNom.Text = carteChoisie;

                // Afficher le niveau
                lblNiveauChiffre.Text = niveau.ToString();

                // Afficher le nom du joueur
                lblJoueurNom.Text = joueur.NomChasseur;

                // Afficher les HP dans la barre de HP
                pbHP.Value = joueur.CurHP;

                // Afficher les HP en chiffre
                lblHPChiffre.Text = joueur.CurHP.ToString();

                // Affiche les HP en rouge si 5 et moins
                if (joueur.CurHP <= 5)
                {
                    //pbHP. = Color.Red;
                    lblHPChiffre.ForeColor = Color.Red;
                }
                else { lblHPChiffre.ForeColor = Color.Black; }

                // Afficher le score
                lblScoreChiffre.Text = joueur.Score.ToString();
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void AfficherAction()
        {
            try
            {
                // Boucle a l'envers dans historiqueInfo
                for (int i = joueur.historiqueInfo.Count; i > 0; i--)
                {
                    // Affiche l'action et le retire de la queue -> AppendText force le textbox a focus sur la derniere ligne ajoutee
                    tbInfoAction.AppendText(joueur.historiqueInfo.Dequeue() + ENTER);
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void AfficherUIFinDePartie(bool aGagner)
        {
            try
            {
                // La partie est toujours terminee lorsque FinDePartie() est appele
                partieEnCours = false;

                // Affiche le message Win si le joueur est gagnant
                lblWin.Visible = aGagner;

                // Affiche le bouton niveau suivant si le joueur est gagnant
                btnNiveauSuivant.Visible = aGagner;

                // Active le bonton niveau suivant si le joueur est gagnant
                btnNiveauSuivant.Enabled = aGagner;

                // Affiche le message lose si le joueur est perdant
                lblLose.Visible = !aGagner;

                // Affiche le bouton recommencer si le joueur est perdant
                btnRecommencer.Visible = !aGagner;

                // Active le bouton recommencer si le joueur est perdant
                btnRecommencer.Enabled = !aGagner;

                // Le textbox info est toujours masque lorsque FinDePartie() est appele
                tbInfoAction.Visible = false;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void MasquerUIFinDePartie()
        {
            try
            {
                // Masque le message Win
                lblWin.Visible = false;

                // Masque le bouton niveau suivant
                btnNiveauSuivant.Visible = false;

                // Desactive le bonton niveau suivant
                btnNiveauSuivant.Enabled = false;

                // Masque le message lose
                lblLose.Visible = false;

                // Masque le bouton recommencer
                btnRecommencer.Visible = false;

                // Desactive le bouton recommencer
                btnRecommencer.Enabled = false;

                // Affiche le textbox info
                tbInfoAction.Visible = true;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void PasserAuNiveauSuivant()
        {
            try
            {
                // Incremente le niveau
                niveau++;


                // Retire tous les picturebox de la fenetres pour qu'ils ne s'empilent pas d'un niveau a l'autre
                pnlCarte.Controls.Clear();


                // Soigne le joueur
                joueur.CurHP += (byte)Math.Round((decimal)(joueur.MaxHP - joueur.CurHP) / 2);


                // Conserve les HP dans une variable temp
                byte tempHP = joueur.CurHP;

                // Conserve l'inventaire dans une variable temp
                Queue<Pic> picTemp = joueur.inventairePic;
                Queue<Epee> epeeTemp = joueur.inventaireEpee;
                Queue<Bouclier> bouclierTemp = joueur.inventaireBouclier;


                // Conserve le score du joueur dans une variable temporaire
                int scoreTemp = joueur.Score;


                // Recharge la carte
                carte.ChargerCarte(carteChoisie, ref joueur, ref monstres);


                // Redonne au joueur ses HP
                joueur.CurHP = tempHP;


                // Redonne au joueur l'inventaire
                joueur.inventairePic = picTemp;
                joueur.inventaireEpee = epeeTemp;
                joueur.inventaireBouclier = bouclierTemp;


                // Redonne le score
                joueur.Score = scoreTemp;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerJoueurDeUnFrame(int velociteX, int velociteY)
        {
            try
            {
                // Deplacer le picture box (fonctionne en int)
                picJoueur.Left += velociteX;
                picJoueur.Top += velociteY;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerJoueurLentementVersLeBasDansThread()
        {
            try
            {
                int velociteX = 0;
                int velociteY = 0;
                int tailleBlocEcran = Parametres.TAILLE_BLOC;

                //Direction: jouer avec x/x et +/-
                velociteY = (int)(tailleBlocEcran * 0.02);
                EnTrainDeBouger = true;

                float topFloat = picJoueur.Top;
                for (int i = 1; i <= 50; i++)
                {
                    Thread.Sleep((int)joueur.FreezeTime / 50); //io sleep damns la thread sec

                    // Invoquer le delegate pour communiquer avec thread principale
                    Invoke(new DeplacerJoueurEntreLesThreads(DeplacerJoueurDeUnFrame), new object[] { velociteX, velociteY });
                }
                EnTrainDeBouger = false;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerJoueurLentementVersLeHautDansThread()
        {
            try
            {
                int velociteX = 0;
                int velociteY = 0;
                int tailleBlocEcran = Parametres.TAILLE_BLOC;

                //Direction: jouer avec x/x et +/-
                velociteY = (int)(tailleBlocEcran * -0.02);
                EnTrainDeBouger = true;

                float topFloat = picJoueur.Top;
                for (int i = 1; i <= 50; i++)
                {
                    Thread.Sleep((int)joueur.FreezeTime / 50); //io sleep damns la thread sec

                    // Invoquer le delegate pour communiquer avec thread principale
                    Invoke(new DeplacerJoueurEntreLesThreads(DeplacerJoueurDeUnFrame), new object[] { velociteX, velociteY });
                }
                EnTrainDeBouger = false;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerJoueurLentementVersLaGaucheDansThread()
        {
            try
            {
                int velociteX = 0;
                int velociteY = 0;
                int tailleBlocEcran = Parametres.TAILLE_BLOC;

                //Direction: jouer avec x/x et +/-
                velociteX = (int)(tailleBlocEcran * -0.02);
                EnTrainDeBouger = true;

                float topFloat = picJoueur.Top;
                for (int i = 1; i <= 50; i++)
                {
                    Thread.Sleep((int)joueur.FreezeTime / 50); //io sleep damns la thread sec

                    // Invoquer le delegate pour communiquer avec thread principale
                    Invoke(new DeplacerJoueurEntreLesThreads(DeplacerJoueurDeUnFrame), new object[] { velociteX, velociteY });
                }
                EnTrainDeBouger = false;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerJoueurLentementVersLaDroiteDansThread()
        {
            try
            {
                int velociteX = 0;
                int velociteY = 0;
                int tailleBlocEcran = Parametres.TAILLE_BLOC;

                //Direction: jouer avec x/x et +/-
                velociteX = (int)(tailleBlocEcran * 0.02);
                EnTrainDeBouger = true;

                float topFloat = picJoueur.Top;
                for (int i = 1; i <= 50; i++)
                {
                    Thread.Sleep((int)joueur.FreezeTime / 50); //io sleep damns la thread sec

                    // Invoquer le delegate pour communiquer avec thread principale
                    Invoke(new DeplacerJoueurEntreLesThreads(DeplacerJoueurDeUnFrame), new object[] { velociteX, velociteY });
                }
                EnTrainDeBouger = false;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void FigerJoueur()
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

        private void Attendre()
        {
            try
            {
                // Creer une thread secondaire pour s'assure de ne pas pouvoir "spammer" une touche pour attaquer les monstres en boucles (ou bouger immediatement apres avoir detruit un mur)
                threadJoueur = new Thread(new ThreadStart(FigerJoueur));
                threadJoueur.IsBackground = true;
                threadJoueur.Start();
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerMonstreLentementDansThread()
        {
            try
            {
                foreach (Monstre monstre in monstres.bassinMonstres)
                {
                    monstre.LancerDe();
                    monstre.PixelRestantPourDeplacement = Parametres.TAILLE_BLOC;

                    switch (monstre.de)
                    {
                        // Haut
                        case 0:
                            // Si le monstre entre en contact avec le joueur
                            if (carte.carte[monstre.PositionY - 1][monstre.PositionX] == Parametres.SYMBOLE_JOUEUR)
                            {
                                // Si n'est pas invisible
                                if (!(joueur.EtatActuel is EtatInvisible))
                                {
                                    // Le monstre attaque
                                    LeJoueurDefend(monstre);

                                    // Si joueur est vivant
                                    if (joueur.EstVivant())
                                    {
                                        // Le joueur attaque
                                        LeJoueurAttaque(monstre);
                                    }
                                    // Si le joueur est mort
                                    else
                                    {
                                        // Supprime le joueur du niveau
                                        pnlCarte.Controls.Remove(picJoueur);

                                        // Affiche l'UI de fin de partie selon que le joueur est gagnant ou non
                                        AfficherUIFinDePartie(aGagner);
                                    }

                                    // Affiche l'action dans l'UI
                                    Invoke(new AfficherInfoEntreLesThreads(AfficherAction), new object[] { });

                                    // Si le monstre meurt
                                    if (!monstre.EstVivant())
                                    {
                                        // Supprime le monstre
                                        Invoke(new SupprimerMonstreEntreLesThreads(SupprimerMonstre), new object[] { monstre });
                                    }
                                }
                                // Si le joueur est invisible
                                else
                                {
                                    // Dessine un symbole vide dans la carte
                                    carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;

                                    // Boucle en fonction de la taille d'un picture box
                                    for (int i = 1; i <= Parametres.TAILLE_BLOC; i++)
                                    {
                                        // Sleep
                                        Thread.Sleep(20);

                                        // Invoquer le delegate pour communiquer avec thread principale
                                        Invoke(new DeplacerMonstreEntreLesThreads(DeplacerMonstreLentementVersLeHautDansThread), new object[] { monstre });
                                    }

                                    // Change la position du monstre dans la carte
                                    monstre.ChangerPosition(Parametres.AXE_Y, false);

                                    // Dessine le symbole de monstre dans la carte a la nouvelle position
                                    carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;
                                }
                            }

                            // S'il n'y a pas de mur
                            else if (carte.carte[monstre.PositionY - 1][monstre.PositionX] != Parametres.SYMBOLE_MUR)
                            {
                                // Dessine un symbole vide dans la carte
                                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;

                                // Boucle en fonction de la taille d'un picture box
                                for (int i = 1; i <= Parametres.TAILLE_BLOC; i++)
                                {
                                    // Sleep
                                    Thread.Sleep(20);

                                    // Invoquer le delegate pour communiquer avec thread principale
                                    Invoke(new DeplacerMonstreEntreLesThreads(DeplacerMonstreLentementVersLeHautDansThread), new object[] { monstre });
                                }

                                // Change la position du monstre dans la carte
                                monstre.ChangerPosition(Parametres.AXE_Y, false);

                                // Dessine le symbole de monstre dans la carte a la nouvelle position
                                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;
                            }
                            break;

                        // Bas
                        case 1:
                            // Si le monstre entre en contact avec le joueur
                            if (carte.carte[monstre.PositionY + 1][monstre.PositionX] == Parametres.SYMBOLE_JOUEUR)
                            {
                                // Si n'est pas invisible
                                if (!(joueur.EtatActuel is EtatInvisible))
                                {
                                    // Le monstre attaque
                                    LeJoueurDefend(monstre);

                                    // Si joueur est vivant
                                    if (joueur.EstVivant())
                                    {
                                        // Le joueur attaque
                                        LeJoueurAttaque(monstre);
                                    }
                                    // Si le joueur est mort
                                    else
                                    {
                                        // Supprime le joueur du niveau
                                        pnlCarte.Controls.Remove(picJoueur);

                                        // Affiche l'UI de fin de partie selon que le joueur est gagnant ou non
                                        AfficherUIFinDePartie(aGagner);
                                    }

                                    // Affiche l'action dans l'UI
                                    Invoke(new AfficherInfoEntreLesThreads(AfficherAction), new object[] { });

                                    // Si le monstre meurt
                                    if (!monstre.EstVivant())
                                    {
                                        // Supprime le monstre
                                        Invoke(new SupprimerMonstreEntreLesThreads(SupprimerMonstre), new object[] { monstre });
                                    }
                                }
                                // Si le joueur est invisible
                                else
                                {
                                    // Dessine un symbole vide dans la carte
                                    carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;

                                    // Boucle en fonction de la taille d'un picture box
                                    for (int i = 1; i <= 50; i++)
                                    {
                                        // Sleep
                                        Thread.Sleep(20);

                                        // Invoquer le delegate pour communiquer avec thread principale
                                        Invoke(new DeplacerMonstreEntreLesThreads(DeplacerMonstreLentementVersLeBasDansThread), new object[] { monstre });
                                    }

                                    // Change la position du monstre dans la carte
                                    monstre.ChangerPosition(Parametres.AXE_Y, true);

                                    // Dessine le symbole de monstre dans la carte a la nouvelle position
                                    carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;
                                }
                            }

                            // Si'il n'y a pas de mur
                            else if (carte.carte[monstre.PositionY + 1][monstre.PositionX] != Parametres.SYMBOLE_MUR)
                            {
                                // Dessine un symbole vide dans la carte
                                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;

                                // Boucle en fonction de la taille d'un picture box
                                for (int i = 1; i <= 50; i++)
                                {
                                    // Sleep
                                    Thread.Sleep(20);

                                    // Invoquer le delegate pour communiquer avec thread principale
                                    Invoke(new DeplacerMonstreEntreLesThreads(DeplacerMonstreLentementVersLeBasDansThread), new object[] { monstre });
                                }

                                // Change la position du monstre dans la carte
                                monstre.ChangerPosition(Parametres.AXE_Y, true);

                                // Dessine le symbole de monstre dans la carte a la nouvelle position
                                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;
                            }
                            break;

                        // Gauche
                        case 2:
                            // Si le monstre entre en contact avec le joueur
                            if (carte.carte[monstre.PositionY][monstre.PositionX - 1] == Parametres.SYMBOLE_JOUEUR)
                            {
                                // Si n'est pas invisible
                                if (!(joueur.EtatActuel is EtatInvisible))
                                {
                                    // Le monstre attaque
                                    LeJoueurDefend(monstre);

                                    // Si joueur est vivant
                                    if (joueur.EstVivant())
                                    {
                                        // Le joueur attaque
                                        LeJoueurAttaque(monstre);
                                    }
                                    // Si le joueur est mort
                                    else
                                    {
                                        // Supprime le joueur du niveau
                                        pnlCarte.Controls.Remove(picJoueur);

                                        // Affiche l'UI de fin de partie selon que le joueur est gagnant ou non
                                        AfficherUIFinDePartie(aGagner);
                                    }

                                    // Affiche l'action dans l'UI
                                    Invoke(new AfficherInfoEntreLesThreads(AfficherAction), new object[] { });

                                    // Si le monstre meurt
                                    if (!monstre.EstVivant())
                                    {
                                        // Supprime le monstre
                                        Invoke(new SupprimerMonstreEntreLesThreads(SupprimerMonstre), new object[] { monstre });
                                    }
                                }
                                // Si le joueur est invisible
                                else
                                {
                                    // Dessine un symbole vide dans la carte
                                    carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;

                                    // Boucle en fonction de la taille d'un picture box
                                    for (int i = 1; i <= 50; i++)
                                    {
                                        // Sleep
                                        Thread.Sleep(20);

                                        // Invoquer le delegate pour communiquer avec thread principale
                                        Invoke(new DeplacerMonstreEntreLesThreads(DeplacerMonstreLentementVersLaGaucheDansThread), new object[] { monstre });
                                    }

                                    // Change la position du monstre dans la carte
                                    monstre.ChangerPosition(Parametres.AXE_X, false);

                                    // Dessine le symbole de monstre dans la carte a la nouvelle position
                                    carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;
                                }
                            }

                            // Si'il n'y a pas de mur
                            else if (carte.carte[monstre.PositionY][monstre.PositionX - 1] != Parametres.SYMBOLE_MUR)
                            {
                                // Dessine un symbole vide dans la carte
                                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;

                                // Boucle en fonction de la taille d'un picture box
                                for (int i = 1; i <= 50; i++)
                                {
                                    // Sleep
                                    Thread.Sleep(20);

                                    // Invoquer le delegate pour communiquer avec thread principale
                                    Invoke(new DeplacerMonstreEntreLesThreads(DeplacerMonstreLentementVersLaGaucheDansThread), new object[] { monstre });
                                }

                                // Change la position du monstre dans la carte
                                monstre.ChangerPosition(Parametres.AXE_X, false);

                                // Dessine le symbole de monstre dans la carte a la nouvelle position
                                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;
                            }
                            break;

                        // Droite
                        case 3:
                            // Si le monstre entre en contact avec le joueur
                            if (carte.carte[monstre.PositionY][monstre.PositionX + 1] == Parametres.SYMBOLE_JOUEUR)
                            {
                                // Si n'est pas invisible
                                if (!(joueur.EtatActuel is EtatInvisible))
                                {
                                    // Le monstre attaque
                                    LeJoueurDefend(monstre);

                                    // Si joueur est vivant
                                    if (joueur.EstVivant())
                                    {
                                        // Le joueur attaque
                                        LeJoueurAttaque(monstre);
                                    }
                                    // Si le joueur est mort
                                    else
                                    {
                                        // Supprime le joueur du niveau
                                        pnlCarte.Controls.Remove(picJoueur);

                                        // Affiche l'UI de fin de partie selon que le joueur est gagnant ou non
                                        AfficherUIFinDePartie(aGagner);
                                    }

                                    // Affiche l'action dans l'UI
                                    Invoke(new AfficherInfoEntreLesThreads(AfficherAction), new object[] { });

                                    // Si le monstre meurt
                                    if (!monstre.EstVivant())
                                    {
                                        // Supprime le monstre
                                        Invoke(new SupprimerMonstreEntreLesThreads(SupprimerMonstre), new object[] { monstre });
                                    }
                                }
                                // Si le joueur est invisible
                                else
                                {
                                    // Dessine un symbole vide dans la carte
                                    carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;

                                    // Boucle en fonction de la taille d'un picture box
                                    for (int i = 1; i <= 50; i++)
                                    {
                                        // Sleep
                                        Thread.Sleep(20);

                                        // Invoquer le delegate pour communiquer avec thread principale
                                        Invoke(new DeplacerMonstreEntreLesThreads(DeplacerMonstreLentementVersLaDroiteDansThread), new object[] { monstre });
                                    }

                                    // Change la position du monstre dans la carte
                                    monstre.ChangerPosition(Parametres.AXE_X, true);

                                    // Dessine le symbole de monstre dans la carte a la nouvelle position
                                    carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;
                                }
                            }

                            // Si'il n'y a pas de mur
                            else if (carte.carte[monstre.PositionY][monstre.PositionX + 1] != Parametres.SYMBOLE_MUR)
                            {
                                // Dessine un symbole vide dans la carte
                                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_VIDE;

                                // Boucle en fonction de la taille d'un picture box
                                for (int i = 1; i <= 50; i++)
                                {
                                    // Sleep
                                    Thread.Sleep(20);

                                    // Invoquer le delegate pour communiquer avec thread principale
                                    Invoke(new DeplacerMonstreEntreLesThreads(DeplacerMonstreLentementVersLaDroiteDansThread), new object[] { monstre });
                                }

                                // Change la position du monstre dans la carte
                                monstre.ChangerPosition(Parametres.AXE_X, true);

                                // Dessine le symbole de monstre dans la carte a la nouvelle position
                                carte.carte[monstre.PositionY][monstre.PositionX] = Parametres.SYMBOLE_MONSTRE;
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerMonstreLentementVersLaGaucheDansThread(Monstre monstre)
        {
            try
            {
                PictureBox monstreADeplacer;
                monstreADeplacer = (PictureBox)pnlCarte.Controls.Find(monstre.Nom, false).FirstOrDefault();
                monstreADeplacer.Left--;
                monstre.PixelRestantPourDeplacement--;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerMonstreLentementVersLaDroiteDansThread(Monstre monstre)
        {
            try
            {
                PictureBox monstreADeplacer;
                monstreADeplacer = (PictureBox)pnlCarte.Controls.Find(monstre.Nom, false).FirstOrDefault();
                monstreADeplacer.Left++;
                monstre.PixelRestantPourDeplacement--;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerMonstreLentementVersLeHautDansThread(Monstre monstre)
        {
            try
            {
                PictureBox monstreADeplacer;
                monstreADeplacer = (PictureBox)pnlCarte.Controls.Find(monstre.Nom, false).FirstOrDefault();
                monstreADeplacer.Top--;
                monstre.PixelRestantPourDeplacement--;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DeplacerMonstreLentementVersLeBasDansThread(Monstre monstre)
        {
            try
            {
                PictureBox monstreADeplacer;
                monstreADeplacer = (PictureBox)pnlCarte.Controls.Find(monstre.Nom, false).FirstOrDefault();
                monstreADeplacer.Top++;
                monstre.PixelRestantPourDeplacement--;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion
    }
}
