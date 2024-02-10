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
    public interface IEtatChasseur
    {
        byte CalculerDommage(Chasseur joueur);

        byte CalculerDefense(Chasseur joueur);

        float CalculerVitesse(Chasseur joueur);
    }
}
