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
    public class EtatForce : IEtatChasseur
    {
        // Thread safe
        // Propriete
        private static EtatForce instance = null;


        // Singleton
        public static EtatForce ObtenirInstance()
        {
            try
            {
                if (instance == null)
                {
                    instance = new EtatForce();
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
                // Retourne les dommages angmentes du joueur
                return (byte)(joueur.CurAP * 2);
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
                // Retourne la defense augmentee du joueur
                return (byte)(joueur.CurDP * 2);
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
