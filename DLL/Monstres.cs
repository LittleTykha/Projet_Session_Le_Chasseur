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
    public class Monstres
    {
        // Propriete
        public List<Monstre> bassinMonstres = new List<Monstre>();


        // Methodes
        public Monstre TrouverMonstre(int index)
        {
            try
            {
                return this.bassinMonstres.ElementAt(index);
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }
    }
}
