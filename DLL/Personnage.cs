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
    public abstract class Personnage
    {
        // Proprietes
        protected float freezeTime = 0;
        protected byte maxHP = 30;
        protected byte curHP = 0;
        protected byte positionX = 0;
        protected byte positionY = 0;
        protected byte limitePositionX = 0;
        protected byte limitePositionY = 0;
        protected byte maxAP = 7;
        protected byte maxDP = 4;
        protected byte curAP = 0;
        protected byte curDP = 0;

        // Getter / Setter
        #region GETTER/SETTER
        public string ErreurValidation { get; protected private set; }

        public byte CurHP
        {
            get { return curHP; }
            set
            {
                // Si plus grand que max hp
                if (value > maxHP)
                {
                    curHP = maxHP;
                }
                // Si plus bas ou egal que 0
                else if (value <= 0)
                {
                    curHP = 0;
                }
                // Si entre 0 et maxHp
                else
                {
                    curHP = value;
                }
            }
        }

        public float FreezeTime
        {
            get => freezeTime;
            set => freezeTime = value;
        }

        public byte MaxHP
        {
            get => maxHP;
        }

        public byte MaxAP
        {
            get => curAP;
        }

        public byte MaxDP
        {
            get => maxDP;
        }

        public byte CurAP
        {
            get => curAP;
            set
            {
                // Si plus grand que max
                if (value > maxAP)
                {
                    curAP = maxAP;
                }
                // Si plus bas ou egal que 0
                else if (value <= 0)
                {
                    curAP = 0;
                }
                // Si entre 0 et max
                else
                {
                    curAP = value;
                }
            }
        }

        public byte CurDP
        {
            get => curDP;
            set
            {
                // Si plus grand que max 
                if (value > maxDP)
                {
                    curDP = maxDP;
                }
                // Si plus bas ou egal que 0
                else if (value <= 0)
                {
                    curDP = 0;
                }
                // Si entre 0 et max
                else
                {
                    curDP = value;
                }
            }
        }

        public byte PositionX
        {
            get => positionX;
            set => positionX = value;
        }

        public byte PositionY
        {
            get => positionY;
            set => positionY = value;
        }

        public byte LimitePositionX
        {
            get => limitePositionX;
            set => limitePositionX = value;
        }

        public byte LimitePositionY
        {
            get => limitePositionY; 
            set => limitePositionY = value;
        }
        #endregion


        // Constructeur
        protected Personnage() { }

        protected Personnage(byte positionX, byte positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
        }

        protected Personnage(byte positionX, byte positionY, byte largeurCarte, byte hauteurCarte)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.limitePositionX = largeurCarte;
            this.limitePositionY = hauteurCarte;
        }



        // Methode
        public abstract bool EstDansLesLimitesDeLaCarte(byte limiteX, byte limiteY);

        public bool EstVivant()
        {
            try
            {
                if (this.curHP > 0)
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
                return true;
            }
        }

        
        public string PrendreDesDegats(ushort damage)
        {
            try
            {
                if ((byte)(damage - this.curDP) < 0)
                {
                    this.CurHP = 0;
                }
                else if (damage - this.curDP > this.curHP)
                {
                    this.CurHP = 0;
                }
                else
                {
                    this.CurHP -= (byte)(damage - this.curDP);
                }
                return (damage - this.curDP < 0 ? 0 : damage - this.curDP).ToString();
            }
            catch (Exception e)
            {
                GestionErreur.GererErreur(e, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }
    }
}
