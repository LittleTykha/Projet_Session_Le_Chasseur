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
    public static class GestionErreur
    {
        // Variables globales
        private static StreamWriter monStreaWriter;

        // Methodes
        public static void GererErreur(Exception e, string sNomFonction)
        {
            try
            {
                // Affiche ce message si le DEBUG_MODE est true dans la console (Uniquement visible dans le Output si Windows Forms)
                Console.WriteLine(Parametres.DEBUG_MODE ? e.Message : "");

                // Consigner l'erreur dans un fichier texte
                monStreaWriter = new StreamWriter(Parametres.ERROR_LOG, true);
                monStreaWriter.WriteLine($"{DateTime.Now} - Une erreur s'est produite dans la fonction {sNomFonction} - {e.Message}");
                monStreaWriter.Close();
            }
            catch
            {
                GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
