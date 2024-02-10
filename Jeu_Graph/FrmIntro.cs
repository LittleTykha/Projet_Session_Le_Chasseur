/*
 * Project Name: Jeu_Graph
 * Student Name: Patrick Tremblay
 * Student ID:   2312796
 * Date:         Oct 27th 2023
 * Version:      1
 * Description:  Projet de Session : Jeu Graphique
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLL;

namespace Jeu_Graph
{
    public partial class FrmIntro : Form
    {
        // Constantes
        const string ENTER = "\r\n";


        // Proprietes
        Carte carte = new Carte();
        Chasseur joueur = new Chasseur();
        Monstres monstres = new Monstres();
        bool erreurTrouve = false;


        // Methodes
        public FrmIntro()
        {
            InitializeComponent();
        }


        // Evenements
        private void FrmIntro_Load(object sender, EventArgs e)
        {
            try
            {
                // Change le textes des elements
                lblNomJoueur.Text = Parametres.MESSAGE_CHOIX_NOM;
                lblChoixCarte.Text = Parametres.MESSAGE_SELECTION_NIVEAU_2;
                lbIntro.Text = Parametres.MESSAGE_DESC_JEU;


                // Boucle dans la liste de cartes
                for (int i = 0; i < carte.ListeCarte.Length; i++)
                {
                    // Charge le combobox avec les noms de carte disponibles
                    cbChoixCarte.Items.Add(carte.ListeCarte[i]);
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void FrmIntro_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // Auitte l'application
                Application.Exit();
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Efface les precedents messages d'erreur pour faciliter la lecture
                tbInfo.Text = Parametres.STRING_VIDE;


                // Attribution du nom au joueur
                joueur.NomChasseur = tbNomJoueur.Text;


                // Chargement de la carte pour verifier les erreur
                carte.ChargerCarte(cbChoixCarte.SelectedItem.ToString(), ref joueur, ref monstres);


                // Si le nom est invalide
                if (joueur.ErreurValidation != "")
                {
                    ErreurTrouvee(joueur.ErreurValidation + ENTER);
                }


                // Si la carte est invalide
                if (carte.ErreurValidation != "")
                {
                    ErreurTrouvee(carte.ErreurValidation + ENTER);
                }


                // Si aucune carte est selectionnee
                if (cbChoixCarte.SelectedIndex == -1)
                {
                    ErreurTrouvee(Parametres.MESSAGE_ERREUR_AUCUNE_SELECTION + ENTER);
                }


                // Si aucune erreur n'est trouvee
                if (!erreurTrouve)
                {
                    // Cree une nouvelle fenetre de jeu
                    FrmJeu fenetreJeu = new FrmJeu();


                    // Initialise les proprietes de la fenetre jeu
                    fenetreJeu.joueur.NomChasseur = this.joueur.NomChasseur;
                    fenetreJeu.carte = this.carte;


                    // Assigne la carte choisie
                    fenetreJeu.carteChoisie = cbChoixCarte.SelectedItem.ToString();


                    // Ouvre la fenetre jeu en mode Modal
                    fenetreJeu.ShowDialog();
                }

                // Reset erreur trouvees pour les prochains clics
                erreurTrouve = false;
            }
            catch (Exception ex)
            {
                GestionErreur.GererErreur(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        // Methodes
        public void ErreurTrouvee(string message)
        {
            try
            {
                // Erreur trouvee est TRUE
                erreurTrouve = true;

                // Affiche un message d'erreur
                tbInfo.Text += message;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
