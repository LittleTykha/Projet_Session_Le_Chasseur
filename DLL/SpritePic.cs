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
using System.Drawing;
using System.Linq.Expressions;

namespace DLL
{
    public class SpritePic
    {
        // Proprietes
        private Point centre;
        private int rayon;
        private Color maCouleur;


        // Getter / Setter
        public Color MaCouleur
        {
            get => maCouleur;
            set => maCouleur = value;
        }


        // Constructeur
        public SpritePic(Point newCentre, Color newCouleur)
        {
            this.centre = newCentre;
            this.maCouleur = newCouleur;
            this.rayon = Parametres.TAILLE_BLOC / 2;
        }


        // Methodes
        public Rectangle GetBounds()
        {
            try
            {
                int x = this.centre.X - this.rayon;
                int y = this.centre.Y - this.rayon;
                int width = 2 * this.rayon;
                int height = 2 * this.rayon;

                return new Rectangle(x, y, width, height);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return default;
            }
        }

        public void DessinerPic(Graphics g)
        {
            try
            {
                // Attribuer la zone pour dessiner le pic
                Rectangle tempRect = new Rectangle(this.centre.X, this.centre.Y + 8, Parametres.TAILLE_BLOC, Parametres.TAILLE_BLOC);

                // Attribuer des couleurs pour dessiner le pic
                Pen crayonBrun = new Pen(Color.SaddleBrown, 3.5f);
                Pen crayonOr = new Pen(Color.Gold, 3.5f);

                // Dessiner le dessus du pic
                g.DrawArc(crayonOr, tempRect, -35.0f, -110.0f);

                // Dessiner le manche du pic
                g.DrawLine(crayonBrun, this.centre.X + (Parametres.TAILLE_BLOC / 2), this.centre.Y + 10, this.centre.X + 25, this.centre.Y + 45);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
