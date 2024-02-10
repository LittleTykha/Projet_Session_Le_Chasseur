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
    public class EtatInvisible : IEtatChasseur
    {
        // Thread safe
        // Propriete
        private static EtatInvisible instance = null;


        // Singleton
        public static EtatInvisible ObtenirInstance()
        {
            try
            {
                if (instance == null)
                {
                    instance = new EtatInvisible();
                }
                return instance;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public byte CalculerDommage(Chasseur joueur)
        {
            try
            {
                // Retourne les dommages normaux du joueur
                return joueur.CurAP;

            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name); 
                return joueur.CurAP;
            }
        }

        public byte CalculerDefense(Chasseur joueur)
        {
            try
            {
                // Retourne la defense normale du joueur
                return joueur.CurDP;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return joueur.CurDP;
            }
        }

        public float CalculerVitesse(Chasseur joueur)
        {
            try
            {
                // Retourne la vitesse normale du chasseur
                return joueur.FreezeTime;
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return joueur.FreezeTime;
            }
        }
    }
}
