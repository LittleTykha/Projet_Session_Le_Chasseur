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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public static class Score
    {
        // Proprietes
        //private static StreamReader streamReader;
        private static StreamWriter streamWriter;
        private static List<Pointage> listeScore = new List<Pointage>() { };
        static FileInfo info = new FileInfo(Parametres.FICHIER_SCORE);

        // Methodes
        public static void AjouterScore(Pointage pointage)
        {
            // Vide la liste (peut etre remplie si le jeu n'est pas completement fermer entre deux niveaux)
            listeScore.Clear();

            // Si le fichierscore.txt n'est pas vide
            if (info.Length != 0)
            {
                // Boucle dans le fichier score.txt pour trouver les pointages deja existants
                foreach (string stringligneFichier in File.ReadLines(Parametres.FICHIER_SCORE))
                {
                    // Tableau temporaire pour conserver la separation du nom et du score
                    string[] temp = stringligneFichier.Split(' ');

                    // Structure temporaire pour conserver le pointage
                    Pointage pointageTemp = new Pointage(temp[0], Convert.ToInt32(temp[1]));

                    // Ajouter le pointage a la liste de pointage
                    listeScore.Add(pointageTemp);
                }
            }

            // Ajoute le nouveau pointage a la liste de pointage
            listeScore.Add(pointage);

            
            // Tri les pointages fonction du score
            listeScore = listeScore.OrderByDescending(x => x.point).ToList();


            // Si la liste de pointage est superieure a 10 entrees
            if (listeScore.Count() > 10)
            {
                // Efface le dernier score (le plus petit)
                listeScore.Remove(listeScore.Last());
            }


            // Efface le contenu du fichier score.txt pour eviter d'ecrire en double
            File.WriteAllText(Parametres.FICHIER_SCORE, string.Empty);


            // Boucle dans la liste triee de pointage
            foreach (Pointage p in listeScore)
            {
                // Sauvegarde le poitage dans le fichier score.txt
                streamWriter = new StreamWriter(Parametres.FICHIER_SCORE, true);
                streamWriter.WriteLine($"{p.nom} {p.point}");
                streamWriter.Close();
            }
        }
    }

    public struct Pointage
    {
        public string nom;
        public int point;

        public Pointage(string nom, int point)
        {
            this.nom = nom;
            this.point = point;
        }
    }
}
