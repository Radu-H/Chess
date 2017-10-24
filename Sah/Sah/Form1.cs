using Sah.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Sah
{


    public partial class Form1 : Form
    {
        Graphics tabla ;

        TextureBrush rama = new TextureBrush(Resources.Frame_Wood);
        TextureBrush alb = new TextureBrush(Resources.White_Marble);
        TextureBrush negru = new TextureBrush(Resources.Black_Marble);
        TextureBrush piesa_alb = new TextureBrush(Resources.White_Player);
        TextureBrush piesa_negru = new TextureBrush(Resources.Green_Player);

        struct tabsah
        {
            public int piesa, juc;
        };

        struct dominatie
        {
            public int j1, j2;
        };

        struct coordonate
        {
            public int x, y;
        };

        //piesa : 1- pion, 2-tura, 3-cal, 4-nebun, 5-rege, 6-regina
        //juc : 1-alb/jos, 2- negru/verde/sus

        tabsah[,] tab = new tabsah[8, 8];
        // tab[y,x] = celula de coord (x,y)

        dominatie[,] dom = new dominatie[8, 8];
        //fiecarui jucator i se atribuie o tabla de dominatie updatata la fiecare mutare
        //cand un rege ajunge in campul dominat de celalalt jucator se declanseaza modul SAH

        int[] select = new int[2];
        int[] dest = new int[2];
        string jucator1 = "", jucator2 = "";
        int mod = -1, juc = 1, nrsah1=-1, nrsah2=-1, ppiesa, prege;
        coordonate[] sah1= new coordonate[2];
        coordonate[] sah2 = new coordonate[2];
        /*
         * nrsah1, nrsah2 retin numarul pieselor care ameninta un rege
         * sah1, sah2 maxim 2 piese care ataca un rege
         * 
         * ppiesa, prege - posibilitatea de a muta o piesa sau regele in timpul sahului
         * 
         * unde mod:
         * =0 : mod de selectie
         * =1 : mod de mutare
         * =2 : mod SAH ( mod selectie special)
         * =3 : SAH-MAT
         * =4 : mod iesire SAH
        */
        List<coordonate> drumsah= new List<coordonate>();

        /*
         * CONCEPT SAH-MAT: dublu SAH => doar regele incearca sa iasa din sah, altfel e mat
         * SAH simplu => in plus si restul pieselor incearca sa captureze amenintarea sau sa blocheze dominatia acesteia 
         */
        private void update_dom()
        {
            nrsah1 = nrsah2 = -1;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    dom[i, j].j1 = dom[i, j].j2 = 0;

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    switch(tab[i,j].piesa)
                    {
                        case 1: dom_pion(i, j); break;
                        case 2: dom_tura(i, j); break;
                        case 3: dom_cal(i, j); break;
                        case 4: dom_nebun(i, j); break;
                        case 6: dom_tura(i, j); dom_nebun(i, j); break;
                            
                    }
        }


        void afis()
        {
            string x = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    x += dom[i, j].j1 + " ";
                x += "\n";
            }
            x += "\n\n\n\n\n";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    x += dom[i, j].j2 + " ";
                x += "\n";
            }

            MessageBox.Show(x);
        }


        private void dom_pion(int x, int y)
        {
            if (tab[x, y].juc == 1)
            {
                if (y > 0 && x > 0)
                {
                    dom[x - 1, y - 1].j1 = 1;
                    if(tab[x-1,y-1].piesa==5 &&tab[x-1,y-1].juc==2)
                        if(nrsah1<2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x ;
                            sah1[nrsah1].y = y ;
                        }
                }
                if (y < 7 && x > 0)
                {
                    dom[x - 1, y + 1].j1 = 1;
                    if (tab[x - 1, y + 1].piesa == 5 && tab[x - 1, y + 1].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
            }
            else
            {
                if (y > 0 && x < 7)
                {
                    dom[x + 1, y - 1].j2 = 1;
                    if (tab[x + 1, y - 1].piesa == 5 && tab[x + 1, y - 1].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }
                if (y < 7 && x < 7)
                {
                    dom[x + 1, y + 1].j2 = 1;
                    if (tab[x + 1, y + 1].piesa == 5 && tab[x + 1, y + 1].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }
            }
        }

        private void dom_tura(int x,int y)
        {
            int i = 1;
            if (x != 7)
            {
                while (tab[x + i, y].piesa == 0)
                {
                    if (tab[x, y].juc == 1)
                    {
                        dom[x + i, y].j1 = 1;
                        if (x + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        dom[x + i, y].j2 = 1;
                        if (x + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                }

                if (tab[x, y].juc == 1)
                {
                    if (tab[x + i, y].piesa != 0)
                    {
                        dom[x + i, y].j1 = 1;
                        if (tab[x + i, y].piesa == 5 && tab[x + i, y].juc == 2)
                            if (nrsah1 < 2)
                            {
                                nrsah1++;
                                sah1[nrsah1].x = x;
                                sah1[nrsah1].y = y;
                            }
                    }
                }
                else
                {
                    if (tab[x + i, y].piesa != 0)
                    {
                        dom[x + i, y].j2 = 1;
                        if (tab[x + i, y].piesa == 5 && tab[x + i, y].juc == 1)
                            if (nrsah2 < 2)
                            {
                                nrsah2++;
                                sah2[nrsah2].x = x;
                                sah2[nrsah2].y = y;
                            }
                    }
                }
            }

            i = 1;

            if (x != 0)
            {
                while (tab[x - i, y].piesa == 0)
                {
                    if (tab[x, y].juc == 1)
                    {
                        dom[x - i, y].j1 = 1;
                        if (x - i - 1 > -1)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        dom[x - i, y].j2 = 1;
                        if (x - i - 1 > -1)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                }

                if (tab[x, y].juc == 1)
                {
                    if (tab[x - i, y].piesa != 0)
                    {
                        dom[x - i, y].j1 = 1;
                        if (tab[x - i, y].piesa == 5 && tab[x - i, y].juc == 2)
                            if (nrsah1 < 2)
                            {
                                nrsah1++;
                                sah1[nrsah1].x = x;
                                sah1[nrsah1].y = y;
                            }
                    }
                }
                else
                {
                    if (tab[x - i, y].piesa != 0)
                    {
                        dom[x - i, y].j2 = 1;
                        if (tab[x - i, y].piesa == 5 && tab[x - i, y].juc == 1)
                            if (nrsah2 < 2)
                            {
                                nrsah2++;
                                sah2[nrsah2].x = x;
                                sah2[nrsah2].y = y;
                            }
                    }
                }
            }
            i = 1;

            if (y != 7)
            {
                while (tab[x, y + i].piesa == 0)
                {
                    if (tab[x, y].juc == 1)
                    {
                        dom[x, y + i].j1 = 1;
                        if (y + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        dom[x, y + i].j2 = 1;
                        if (y + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                }


                if (tab[x,y].juc == 1)
                {
                    if (tab[x, y + i].piesa != 0)
                    {
                        dom[x, y + i].j1 = 1;
                        if (tab[x, y + i].piesa == 5 && tab[x, y + i].juc == 2)
                            if (nrsah1 < 2)
                            {
                                nrsah1++;
                                sah1[nrsah1].x = x;
                                sah1[nrsah1].y = y;
                            }
                    }
                }
                else
                {
                    if (tab[x, y + i].piesa != 0)
                    {
                        dom[x, y + i].j2 = 1;
                        if (tab[x, y + i].piesa == 5 && tab[x, y + i].juc == 1)
                            if (nrsah2 < 2)
                            {
                                nrsah2++;
                                sah2[nrsah2].x = x;
                                sah2[nrsah2].y = y;
                            }
                    }
                }
            }
            i = 1;

            if (y != 0)
            {
                while (tab[x, y - i].piesa == 0)
                {
                    if (tab[x, y].juc == 1)
                    {
                        dom[x, y - i].j1 = 1;
                        if (y - i - 1 > -1)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        dom[x, y - i].j2 = 1;
                        if (y - i - 1 > -1)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                }

                if (tab[x,y].juc == 1)
                {
                    if (tab[x, y - i].piesa != 0)
                    {
                        dom[x, y - i].j1 = 1;
                        if (tab[x, y - i].piesa == 5 && tab[x, y - i].juc == 2)
                            if (nrsah1 < 2)
                            {
                                nrsah1++;
                                sah1[nrsah1].x = x;
                                sah1[nrsah1].y = y;
                            }
                    }
                }
                else
                {
                    if (tab[x, y - i].piesa != 0)
                    {
                        dom[x, y - i].j2 = 1;
                        if (tab[x, y - i].piesa == 5 && tab[x, y - i].juc == 1)
                            if (nrsah2 < 2)
                            {
                                nrsah2++;
                                sah2[nrsah2].x = x;
                                sah2[nrsah2].y = y;
                            }
                    }
                }
            }
        }

        private void dom_cal(int x, int y)
        {
            if (x > 1 && y < 7)
                if (tab[x, y].juc == 1)
                {
                    dom[x - 2, y + 1].j1 = 1;
                    if (tab[x - 2, y + 1].piesa == 5 && tab[x - 2, y + 1].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
                else
                {
                    dom[x - 2, y + 1].j2 = 1;
                     if (tab[x - 2, y + 1].piesa == 5 && tab[x - 2, y + 1].juc == 1)
                         if (nrsah2 < 2)
                         {
                             nrsah2++;
                             sah2[nrsah2].x = x;
                             sah2[nrsah2].y = y;
                         }
                }

            if (x > 0 && y < 6)
                if (tab[x, y].juc == 1)
                {
                    dom[x - 1, y + 2].j1 = 1;
                    if (tab[x - 1, y + 2].piesa == 5 && tab[x - 1, y + 2].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
                else
                {
                    dom[x - 1, y + 2].j2 = 1;
                    if (tab[x - 1, y + 2].piesa == 5 && tab[x - 1, y + 2].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }

            if (x < 7 && y < 6)
                if (tab[x, y].juc == 1)
                {
                    dom[x + 1, y + 2].j1 = 1;
                    if (tab[x + 1, y + 2].piesa == 5 && tab[x + 1, y + 2].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
                else
                {
                    dom[x + 1, y + 2].j2 = 1;
                    if (tab[x + 1, y + 2].piesa == 5 && tab[x + 1, y + 2].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }

            if (x < 6 && y < 7)
                if (tab[x, y].juc == 1)
                {
                    dom[x + 2, y + 1].j1 = 1;
                    if (tab[x + 2, y + 1].piesa == 5 && tab[x + 2, y + 1].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
                else
                {
                    dom[x + 2, y + 1].j2 = 1;
                    if (tab[x + 2, y + 1].piesa == 5 && tab[x + 2, y + 1].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }

            if (x < 6 && y > 0)
                if (tab[x, y].juc == 1)
                {
                    dom[x + 2, y - 1].j1 = 1;
                    if (tab[x + 2, y - 1].piesa == 5 && tab[x + 2, y - 1].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
                else
                {
                    dom[x + 2, y - 1].j2 = 1;
                    if (tab[x + 2, y - 1].piesa == 5 && tab[x + 2, y - 1].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }

            if (x < 7 && y > 1)
                if (tab[x, y].juc == 1)
                {
                    dom[x + 1, y - 2].j1 = 1;
                    if (tab[x + 1, y - 2].piesa == 5 && tab[x + 1, y - 2].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
                else
                {
                    dom[x + 1, y - 2].j2 = 1;
                    if (tab[x + 1, y - 2].piesa == 5 && tab[x + 1, y - 2].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }

            if (x > 0 && y > 1)
                if (tab[x, y].juc == 1)
                {
                    dom[x - 1, y - 2].j1 = 1;
                    if (tab[x - 1, y - 2].piesa == 5 && tab[x - 1, y - 2].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
                else
                {
                    dom[x - 1, y - 2].j2 = 1;
                    if (tab[x - 1, y - 2].piesa == 5 && tab[x - 1, y - 2].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }

            if (x > 1 && y > 0)
                if (tab[x, y].juc == 1)
                {
                    dom[x - 2, y - 1].j1 = 1;
                    if (tab[x - 2, y - 1].piesa == 5 && tab[x - 2, y - 1].juc == 2)
                        if (nrsah1 < 2)
                        {
                            nrsah1++;
                            sah1[nrsah1].x = x;
                            sah1[nrsah1].y = y;
                        }
                }
                else
                {
                    dom[x - 2, y - 1].j2 = 1;
                    if (tab[x - 2, y - 1].piesa == 5 && tab[x - 2, y - 1].juc == 1)
                        if (nrsah2 < 2)
                        {
                            nrsah2++;
                            sah2[nrsah2].x = x;
                            sah2[nrsah2].y = y;
                        }
                }
        }

        private void dom_nebun(int x, int y)
        {
            int i = 1;
            if (x != 7 && y != 7)
            {
                while (tab[x + i, y + i].piesa == 0)
                {
                    if (tab[x, y].juc == 1)
                    {
                        dom[x + i, y + i].j1 = 1;
                        if (x + i + 1 < 8 && y + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        dom[x + i, y + i].j2 = 1;
                        if (x + i + 1 < 8 && y + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                }

                if (tab[x, y].juc == 1)
                {
                    if (tab[x + i, y + i].piesa != 0)
                    {
                        dom[x + i, y + i].j1 = 1;
                        if (tab[x + i, y + i].piesa == 5 && tab[x + i, y + i].juc == 2)
                            if (nrsah1 < 2)
                            {
                                nrsah1++;
                                sah1[nrsah1].x = x;
                                sah1[nrsah1].y = y;
                            }
                    }
                }
                else
                {
                    if (tab[x + i, y + i].piesa != 0)
                    {
                        dom[x + i, y + i].j2 = 1;
                        if (tab[x + i, y + i].piesa == 5 && tab[x + i, y + i].juc == 1)
                            if (nrsah2 < 2)
                            {
                                nrsah2++;
                                sah2[nrsah2].x = x;
                                sah2[nrsah2].y = y;
                            }
                    }
                }
            }

            i = 1;

            if (x != 0 && y != 7)
            {
                while (tab[x - i, y + i].piesa == 0)
                {
                    if (tab[x, y].juc == 1)
                    {
                        dom[x - i, y + i].j1 = 1;
                        if (x - i - 1 > -1 && y + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        dom[x - i, y + i].j2 = 1;
                        if (x - i - 1 > -1 && y + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                }

                if (tab[x, y].juc == 1)
                {
                    if (tab[x - i, y + i].piesa != 0)
                    {
                        dom[x - i, y + i].j1 = 1;
                        if (tab[x - i, y + i].piesa == 5 && tab[x - i, y + i].juc == 2)
                            if (nrsah1 < 2)
                            {
                                nrsah1++;
                                sah1[nrsah1].x = x;
                                sah1[nrsah1].y = y;
                            }
                    }
                }
                else
                {
                    if (tab[x - i, y + i].piesa != 0)
                    {
                        dom[x - i, y + i].j2 = 1;
                        if (tab[x - i, y + i].piesa == 5 && tab[x - i, y + i].juc == 1)
                            if (nrsah2 < 2)
                            {
                                nrsah2++;
                                sah2[nrsah2].x = x;
                                sah2[nrsah2].y = y;
                            }
                    }
                }
            }

            i = 1;

            if (x != 0 && y != 0)
            {
                while (tab[x - i, y - i].piesa == 0)
                {
                    if (tab[x, y].juc == 1)
                    {
                        dom[x - i, y - i].j1 = 1;
                        if (y - i - 1 > -1 && x - i - 1 > -1)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        dom[x - i, y - i].j2 = 1;
                        if (y - i - 1 > -1 && x - i - 1 > -1)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                }

                if (tab[x, y].juc == 1)
                {
                    if (tab[x - i, y - i].piesa != 0)
                    {
                        dom[x - i, y - i].j1 = 1;
                        if (tab[x - i, y - i].piesa == 5 && tab[x - i, y - i].juc == 2)
                            if (nrsah1 < 2)
                            {
                                nrsah1++;
                                sah1[nrsah1].x = x;
                                sah1[nrsah1].y = y;
                            }
                    }
                }
                else
                {
                    if (tab[x - i, y - i].piesa != 0)
                    {
                        dom[x - i, y - i].j2 = 1;
                        if (tab[x - i, y - i].piesa == 5 && tab[x - i, y - i].juc == 1)
                            if (nrsah2 < 2)
                            {
                                nrsah2++;
                                sah2[nrsah2].x = x;
                                sah2[nrsah2].y = y;
                            }
                    }
                }
            }

            i = 1;

            if (x != 7 && y != 0)
            {
                while (tab[x + i, y - i].piesa == 0)
                {
                    if (tab[x, y].juc == 1)
                    {
                        dom[x + i, y - i].j1 = 1;
                        if (y - i - 1 > -1 && x + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        dom[x + i, y - i].j2 = 1;
                        if (y - i - 1 > -1 && x + i + 1 < 8)
                        {
                            i++;
                        }
                        else
                            break;
                    }
                }

                if (tab[x, y].juc == 1)
                {
                    if (tab[x + i, y - i].piesa != 0)
                    {
                        dom[x + i, y - i].j1 = 1;
                        if (tab[x + i, y - i].piesa == 5 && tab[x + i, y - i].juc == 2)
                            if (nrsah1 < 2)
                            {
                                nrsah1++;
                                sah1[nrsah1].x = x;
                                sah1[nrsah1].y = y;
                            }
                    }
                }
                else
                {
                    if (tab[x + i, y - i].piesa != 0)
                    {
                        dom[x + i, y - i].j2 = 1;
                        if (tab[x + i, y - i].piesa == 5 && tab[x + i, y - i].juc == 1)
                            if (nrsah2 < 2)
                            {
                                nrsah2++;
                                sah2[nrsah2].x = x;
                                sah2[nrsah2].y = y;
                            }
                    }
                }
            }
        }

        private void resetabla()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (i < 2)
                        tab[i, j].juc = 2;
                    else
                        if (i >= 6)
                            tab[i, j].juc = 1;
                        else
                            tab[i, j].juc = 0;
                
                    tab[i, j].piesa = 0;
                    if (i == 0 || i == 7)

                        if (j == 0 || j == 7)
                            tab[i, j].piesa = 2;
                        else
                            if (j == 1 || j == 6)
                                tab[i, j].piesa = 3;
                            else 
                        if (j == 2 || j == 5)
                            tab[i, j].piesa = 4;
                    
                    if(i==0||i==7)
                        if (j == 3) tab[i, j].piesa = 6;
                        else if (j == 4) tab[i, j].piesa = 5;

                    if (i == 1 || i == 6) tab[i, j].piesa = 1;
                }
            drawtab();
            update_dom();
        }

        private void rectang(Graphics a, TextureBrush b, int x, int y, int w, int h)
        {
            a.FillRectangle(b, new Rectangle(x, y, w, h));
        }

        private void pion (Graphics a,TextureBrush b,int x,int y)
        {
            a.FillPolygon(b, new Point[]{new Point(x+20,y+70),
                                        new Point(x+30,y+40),
                                        new Point(x+50,y+40),
                                        new Point(x+60,y+70)});

            a.FillPie(b, x + 25, y + 11, 30, 30, 0, 360);
        }

        private void cal (Graphics a, TextureBrush b, int x, int y)
        {
            a.FillPolygon(b, new Point[]{new Point(x+20,y+70),
                                         new Point(x+30,y+40),
                                         new Point(x+50,y+20),
                                         new Point(x+60,y+45),
                                         new Point(x+55,y+50),
                                         new Point(x+40,y+40),
                                         new Point(x+50,y+70)});

            a.FillPie(b, x + 20, y + 10, 40, 40, 130, 200);
        }

       private void nebun(Graphics a, TextureBrush b, int x, int y)
        {
            a.FillPolygon(b, new Point[]{new Point(x+25,y+70),
                                         new Point(x+35,y+35),
                                         new Point(x+45,y+35),
                                         new Point(x+55,y+70)});

            a.FillPie(b, x + 29, y + 14, 22, 22,350,330);

            a.FillPie(b,x+35,y+5,10,10,0,360);

            a.FillRectangle(b,x + 29, y + 36, 22, 5);
        }

       private void rege(Graphics a, TextureBrush b, int x, int y)
        {
            a.FillPolygon(b, new Point[]{new Point(x+22,y+70),
                                        new Point(x+32,y+32),
                                        new Point(x+27,y+27),
                                        new Point(x+53,y+27),
                                        new Point(x+48,y+32),
                                        new Point(x+58,y+70)});

            a.FillRectangle(b, x + 35, y + 5, 10, 22);

            a.FillRectangle(b, x + 25, y + 13, 30, 6);

            a.FillRectangle(b, x + 25, y + 40, 30, 6);
        }

       private void regina(Graphics a, TextureBrush b, int x, int y)
        {
            a.FillPolygon(b, new Point[] { new Point(x+20,y+70),
                                           new Point(x+30,y+50),
                                           new Point(x+50,y+50),
                                           new Point(x+60,y+70)});

            a.FillPolygon(b, new Point[] { new Point(x+23,y+50),
                                           new Point(x+35,y+40),
                                           new Point(x+45,y+40),
                                           new Point(x+56,y+50)});

            a.FillRectangle(b, x + 30, y + 35, 20, 5);

            a.FillRectangle(b, x + 35, y + 25, 10, 15);

            a.FillPolygon(b, new Point[] { new Point(x+30,y+25),
                                           new Point(x+25,y+18),
                                           new Point(x+55,y+18),
                                           new Point(x+50,y+25)});

            a.FillPie(b, x + 35, y + 8, 10, 10, 0, 360);
        }

       private void tura(Graphics a, TextureBrush b, int x, int y)
        {
            a.FillPolygon(b, new Point[]{new Point(x+20,y+10),
                                        new Point(x+28,y+10),
                                        new Point(x+28,y+20),
                                        new Point(x+36,y+20),
                                        new Point(x+36,y+10),
                                        new Point(x+44,y+10),
                                        new Point(x+44,y+20),
                                        new Point(x+52,y+20),
                                        new Point(x+52,y+10),
                                        new Point(x+60,y+10),
                                        new Point(x+60,y+30),
                                        new Point(x+20,y+30)});

            a.FillRectangle(b, x + 25, y + 30, 30, 40);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void buttons()
        {
           for(int j=1;j<=8;j++)
            for(int i =1;i<=8;i++)
            {
                TranspCtrl bt = new TranspCtrl();
                bt.Left = 180 + (i - 1) * 80;
                bt.Top = 30 + (j - 1) * 80;
                bt.Height = bt.Width = 80;
                bt.Name="C"+j.ToString()+i.ToString();
                this.Controls.Add(bt);
                bt.Click += new System.EventHandler(this.bt_Click);
            }
        }

        private int rety(string pct)
        {
            return int.Parse(pct.Substring(1, 1));
        }

        private int retx(string pct)
        {
            return int.Parse(pct.Substring(2, 1));
        }

        private void drawpiesa(TextureBrush textbr, int x, int y, int piesa)
        {
            if ((x + y) % 2 == 0)
                rectang(tabla, alb, 180 + (x - 1) * 80, 30 + (y - 1) * 80, 80, 80);
            else
                rectang(tabla, negru, 180 + (x - 1) * 80, 30 + (y - 1) * 80, 80, 80);

            switch(piesa)
            {
                case 1: pion(tabla, textbr, 180 + (x - 1) * 80, 30 + (y - 1) * 80); break;
                case 2: tura(tabla, textbr, 180 + (x - 1) * 80, 30 + (y - 1) * 80); break;
                case 3: cal(tabla, textbr, 180 + (x - 1) * 80, 30 + (y - 1) * 80); break;
                case 4: nebun(tabla, textbr, 180 + (x - 1) * 80, 30 + (y - 1) * 80); break;
                case 5: rege(tabla, textbr, 180 + (x - 1) * 80, 30 + (y - 1) * 80); break;
                case 6: regina(tabla, textbr, 180 + (x - 1) * 80, 30 + (y - 1) * 80); break;
            }
        }

        private bool valid_pion(int xi, int yi, int xf, int yf, int piesa)
        {
            if (juc == 1)
            {
                if (yf - yi == -2 && xf == xi && (tab[yi - 1, xi].piesa == 0 && tab[yf, xf].piesa == 0) && yi == 6)
                    return true;
                if (yf - yi == -1 && xf == xi && tab[yf, xf].piesa == 0)
                    return true;
                if (yf - yi == -1 && (xf - xi == -1 || xf - xi == 1) && tab[yf, xf].piesa != 0)
                    return true;
            }
            else
            {
                if (yf - yi == 2 && xf == xi && (tab[yi + 1, xi].piesa == 0 && tab[yf, xf].piesa == 0) && yi == 1)
                    return true;
                if (yf - yi == 1 && xf == xi && tab[yf, xf].piesa == 0)
                    return true;
                if (yf - yi == 1 && (xf - xi == -1 || xf - xi == 1) && tab[yf, xf].piesa != 0)
                    return true;
            }

            return false;
        }

        private bool valid_tura(int xi, int yi, int xf, int yf, int piesa)
        {
            if (!(xf == xi || yf == yi))
                return false;
            if (xf == xi && yf > yi)
                for (int i = yi + 1; i < yf; i++)
                    if (tab[i, xi].piesa != 0)
                        return false;
            if (xf == xi && yf < yi)
                for (int i = yi - 1; i > yf; i--)
                    if (tab[i, xi].piesa != 0)
                        return false;
            if (xf > xi && yf == yi)
                for (int j = xi + 1; j < xf; j++)
                    if (tab[yi, j].piesa != 0)
                        return false;
            if (xf < xi && yf == yi)
                for (int j = xi - 1; j > xf; j--)
                    if (tab[yi, j].piesa != 0)
                        return false;

            return true;
        }

        private bool valid_cal(int xi, int yi, int xf, int yf, int piesa)
        {
            if ((xf == xi + 1 && (yf == yi + 2 || yf == yi - 2)) || (xf == xi + 2 && (yf == yi + 1 || yf == yi - 1)) || (xf == xi - 1 && (yf == yi + 2 || yf == yi - 2)) || (xf == xi - 2 && (yf == yi + 1 || yf == yi - 1)))
                return true;
            return false;
        }

        private bool valid_nebun(int xi, int yi, int xf, int yf, int piesa)
        {
            if (!(Math.Abs(xi - xf) == Math.Abs(yi - yf)))
                return false;
            if (xf < xi && yf < yi)
                for (int i = 1; i < Math.Abs(xi - xf); i++)
                    if (tab[yi - i, xi - i].piesa != 0)
                        return false;
            if (xf > xi && yf < yi)
                for (int i = 1; i < Math.Abs(xf - xi); i++)
                    if (tab[yi - i, xi + i].piesa != 0)
                        return false;
            if (xf > xi && yf > yi)
                for (int i = 1; i < Math.Abs(xf - xi); i++)
                    if (tab[yi + i, xi + i].piesa != 0)
                        return false;
            if (xf < xi && yf > yi)
                for (int i = 1; i < Math.Abs(xf - xi); i++)
                    if (tab[yi + i, xi - i].piesa != 0)
                        return false;

                    return true;
        }

        private bool valid_rege(int xi, int yi, int xf, int yf, int piesa)
        {
            if (juc == 1)
            {
                if (dom[yf, xf].j2 == 1)
                    return false;
            }
            else if (dom[yf, xf].j1 == 1)
                return false;

            if (yf > 0)
                if (tab[yf - 1, xf].piesa == 5 && tab[yf - 1, xf].juc != juc)
                    return false;
            if (yf < 7)
                if (tab[yf + 1, xf].piesa == 5 && tab[yf + 1, xf].juc != juc)
                    return false;
            if (xf > 0)
                if (tab[yf, xf - 1].piesa == 5 && tab[yf, xf - 1].juc != juc)
                    return false;
            if (xf < 7)
                if (tab[yf, xf + 1].piesa == 5 && tab[yf, xf + 1].juc != juc)
                    return false;
            if (yf > 0 && xf > 0)
                if (tab[yf - 1, xf - 1].piesa == 5 && tab[yf - 1, xf - 1].juc != juc)
                    return false;
            if (yf > 0 && xf < 7)
                if (tab[yf - 1, xf + 1].piesa == 5 && tab[yf - 1, xf + 1].juc != juc)
                    return false;
            if (yf < 7 && xf < 7)
                if (tab[yf + 1, xf + 1].piesa == 5 && tab[yf + 1, xf + 1].juc != juc)
                    return false;
            if (yf < 7 && xf > 0)
                if (tab[yf + 1, xf - 1].piesa == 5 && tab[yf + 1, xf - 1].juc != juc)
                    return false;
            if (Math.Abs(xf - xi) <= 1 && Math.Abs(yf - yi) <= 1)
                return true;
            return false;
        }

        private bool mutare_valid(int yi, int xi, int yf, int xf, int piesa)
        {
            bool ok = true;

            switch(piesa)
            {
                case 1:
                    {
                        ok = valid_pion(yi, xi, yf, xf, piesa);
                        break;
                    }
                case 2:
                    {
                        ok = valid_tura(yi, xi, yf, xf, piesa);
                        break;
                    }
                case 3:
                    {
                        ok = valid_cal(yi, xi, yf, xf, piesa);
                        break;
                    }
                case 4:
                    {
                        ok = valid_nebun(yi, xi, yf, xf, piesa);
                        break;
                    }
                case 5:
                    {
                        ok = valid_rege(yi, xi, yf, xf, piesa);
                        break;
                    }
                case 6:
                    {
                        ok = valid_nebun(yi, xi, yf, xf, piesa) || valid_tura(yi, xi, yf, xf, piesa);
                        break;
                    }
            }

            return ok;
        }

        private bool verif_sah()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (tab[i, j].piesa == 5)
                        if (tab[i, j].juc == 1)
                        {
                            if (dom[i, j].j2 == 1)
                            {
                                MessageBox.Show(jucator1 + " is in CHECK.");
                                return true;
                            }
                        }
                        else
                        {
                            if (dom[i, j].j1 == 1)
                            {
                                MessageBox.Show(jucator2 + " is in CHECK.");
                                return true;
                            }
                        }

            return false;
        }

        private void mod_sah()
        {
            int x=-1, y=-1;
            prege = 1;
            ppiesa = 0;
            // prege / ppiese - pos de a muta regele/piesa
            if (juc == 1)
            {
                for (int i = 0; i < 8&&x==-1; i++)
                    for (int j = 0; j < 8&&x==-1; j++)
                        if (tab[i, j].piesa == 5 && tab[i, j].juc == 1)
                        {
                            x = i;
                            y = j;
                           
                        }
                #region REGE1-EVADARE
                if (x == 0)
                {
                    if (y == 0)
                    {
                        if ((dom[x + 1, y].j2 == 1 || tab[x + 1, y].juc == 1) && (dom[x + 1, y + 1].j2 == 1 || tab[x + 1, y + 1].juc == 1) && (dom[x, y + 1].j2 == 1 || tab[x, y + 1].juc == 1))
                            prege = 0;
                    }
                    else if (y == 7)
                    {
                        if ((dom[x + 1, y].j2 == 1 || tab[x + 1, y].juc == 1) && (dom[x + 1, y - 1].j2 == 1 || tab[x + 1, y - 1].juc == 1) && (dom[x, y - 1].j2 == 1 || tab[x, y - 1].juc == 1))
                            prege = 0;
                    }
                    else
                    {
                        if ((dom[x + 1, y].j2 == 1 || tab[x + 1, y].juc == 1) && (dom[x + 1, y + 1].j2 == 1 || tab[x + 1, y + 1].juc == 1) && (dom[x, y + 1].j2 == 1 || tab[x, y + 1].juc == 1) && (dom[x + 1, y - 1].j2 == 1 || tab[x + 1, y - 1].juc == 1) && (dom[x, y - 1].j2 == 1 || tab[x, y - 1].juc == 1))
                            prege = 0;
                    }
                }
                else if (x == 7)
                {
                    if (y == 0)
                    {
                        if ((dom[x - 1, y].j2 == 1 || tab[x - 1, y].juc == 1) && (dom[x - 1, y + 1].j2 == 1 || tab[x - 1, y + 1].juc == 1) && (dom[x, y + 1].j2 == 1 || tab[x, y + 1].juc == 1))
                            prege = 0;
                    }
                    else if (y == 7)
                    {
                        if ((dom[x - 1, y].j2 == 1 || tab[x - 1, y].juc == 1) && (dom[x - 1, y - 1].j2 == 1 || tab[x - 1, y - 1].juc == 1) && (dom[x, y - 1].j2 == 1 || tab[x, y - 1].juc == 1))
                            prege = 0;
                    }
                    else
                    {
                        if ((dom[x - 1, y].j2 == 1 || tab[x - 1, y].juc == 1) && (dom[x - 1, y + 1].j2 == 1 || tab[x - 1, y + 1].juc == 1) && (dom[x, y + 1].j2 == 1 || tab[x, y + 1].juc == 1) && (dom[x - 1, y - 1].j2 == 1 || tab[x - 1, y - 1].juc == 1) && (dom[x, y - 1].j2 == 1 || tab[x, y - 1].juc == 1))
                            prege = 0;
                    }
                }
                else if (y == 0)
                {
                    if ((dom[x - 1, y].j2 == 1 || tab[x - 1, y].juc == 1) && (dom[x - 1, y + 1].j2 == 1 || tab[x - 1, y + 1].juc == 1) && (dom[x, y + 1].j2 == 1 || tab[x, y + 1].juc == 1) && (dom[x + 1, y + 1].j2 == 1 || tab[x + 1, y + 1].juc == 1) && (dom[x + 1, y].j2 == 1 || tab[x + 1, y].juc == 1))
                        prege = 0;
                }
                else if (y == 7)
                {
                    if ((dom[x - 1, y].j2 == 1 || tab[x - 1, y].juc == 1) && (dom[x - 1, y - 1].j2 == 1 || tab[x - 1, y - 1].juc == 1) && (dom[x, y - 1].j2 == 1 || tab[x, y - 1].juc == 1) && (dom[x + 1, y - 1].j2 == 1 || tab[x + 1, y - 1].juc == 1) && (dom[x + 1, y].j2 == 1 || tab[x + 1, y].juc == 1))
                        prege = 0;
                }
                else
                {
                    if ((dom[x - 1, y].j2 == 1 || tab[x - 1, y].juc == 1) && (dom[x - 1, y + 1].j2 == 1 || tab[x - 1, y + 1].juc == 1) && (dom[x, y + 1].j2 == 1 || tab[x, y + 1].juc == 1) && (dom[x + 1, y + 1].j2 == 1 || tab[x + 1, y + 1].juc == 1) && (dom[x + 1, y].j2 == 1 || tab[x + 1, y].juc == 1) && (dom[x - 1, y - 1].j2 == 1 || tab[x - 1, y - 1].juc == 1) && (dom[x, y - 1].j2 == 1 || tab[x, y - 1].juc == 1) && (dom[x + 1, y - 1].j2 == 1 || tab[x + 1, y - 1].juc == 1))
                        prege = 0;
                }
                #endregion

                if (nrsah2 == 1)
                    if (prege == 0)
                    {
                        mod = 3;
                    }
                    else
                    {
                        mod = 2;
                    }


                if (nrsah2 == 0)
                {
                    coordonate a = new coordonate();
                    #region PIESA1-BLOCARE
                    switch (tab[sah2[0].x, sah2[0].y].piesa)
                    {
                        case 1: drumsah.Add(sah2[0]); break;
                        case 2:
                            {
                                if (x == sah2[0].x)
                                {
                                    a.x = x;
                                    if (y < sah2[0].y)
                                        for (int j = sah2[0].y; j > y; j--)
                                        {
                                            a.y = j;
                                            drumsah.Add(a);
                                        }
                                    if (y > sah2[0].y)
                                    {
                                        for (int j = sah2[0].y; j < y; j++)
                                        {
                                            a.y = j;
                                            drumsah.Add(a);
                                        }
                                    }
                                }
                                else if (y == sah2[0].y)
                                {
                                    a.y = y;
                                    if (x < sah2[0].x)
                                        for (int i = sah2[0].x; i > x; i--)
                                        {
                                            a.x = i;
                                            drumsah.Add(a);
                                        }
                                    if (x > sah2[0].x)
                                    {
                                        for (int i = sah2[0].x; i < x; i++)
                                        {
                                            a.x = i;
                                            drumsah.Add(a);
                                        }
                                    }
                                }
                                break;
                            }
                        case 3:
                            {
                                drumsah.Add(sah2[0]);
                                break;
                            }
                        case 4:
                            {

                                for (int i = 0; i < Math.Abs(x - sah2[0].x); i++)
                                {
                                    if (x < sah2[0].x)
                                        a.x = sah2[0].x - i;

                                    if (x > sah2[0].x)
                                        a.x = sah2[0].x + i;

                                    if (y < sah2[0].y)
                                        a.y = sah2[0].y - i;

                                    if (y > sah2[0].y)
                                        a.y = sah2[0].y + i;
                                    drumsah.Add(a);
                                }

                                break;
                            }
                        case 6:
                            {
                                if (x == sah2[0].x)
                                {
                                    a.x = x;
                                    if (y < sah2[0].y)
                                        for (int j = sah2[0].y; j > y; j--)
                                        {
                                            a.y = j;
                                            drumsah.Add(a);
                                        }
                                    if (y > sah2[0].y)
                                    {
                                        for (int j = sah2[0].y; j < y; j++)
                                        {
                                            a.y = j;
                                            drumsah.Add(a);
                                        }
                                    }
                                }
                                else if (y == sah2[0].y)
                                {
                                    a.y = y;
                                    if (x < sah2[0].x)
                                        for (int i = sah2[0].x; i > x; i--)
                                        {
                                            a.x = i;
                                            drumsah.Add(a);
                                        }
                                    if (x > sah2[0].x)
                                    {
                                        for (int i = sah2[0].x; i < x; i++)
                                        {
                                            a.x = i;
                                            drumsah.Add(a);
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < Math.Abs(x - sah2[0].x); i++)
                                    {
                                        if (x < sah2[0].x)
                                            a.x = sah2[0].x - i;

                                        if (x > sah2[0].x)
                                            a.x = sah2[0].x + i;

                                        if (y < sah2[0].y)
                                            a.y = sah2[0].y - i;

                                        if (y > sah2[0].y)
                                            a.y = sah2[0].y + i;
                                        drumsah.Add(a);
                                    }
                                }
                                break;
                            }
                    }
                    #endregion
                    // se cauta daca se poate muta o piesa intr-o celula prin care se ameninta regele
                    for (int i = 0; i < 8 && ppiesa == 0; i++)
                        for (int j = 0; j < 8 && ppiesa == 0; j++)
                            if (dom[i, j].j1 == 1 && drumsah.Contains(new coordonate { x = i, y = j }))
                                ppiesa = 1;

                    if (prege == 0 && ppiesa == 0)
                    {
                        mod = 3;
                    }
                    else
                    {
                        mod = 2;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 8 && x == -1; i++)
                    for (int j = 0; j < 8 && x == -1; j++)
                        if (tab[i, j].piesa == 5 && tab[i, j].juc == 2)
                        {
                            x = i;
                            y = j;
                           
                        }
                #region REGE2-EVADARE
                if (x == 0)
                {
                    if (y == 0)
                    {
                        if ((dom[x + 1, y].j1 == 1 || tab[x + 1, y].juc == 2) && (dom[x + 1, y + 1].j1 == 1 || tab[x + 1, y + 1].juc == 2) && (dom[x, y + 1].j1 == 1 || tab[x, y + 1].juc == 2))
                            prege = 0;
                    }
                    else if (y == 7)
                    {
                        if ((dom[x + 1, y].j1 == 1 || tab[x + 1, y].juc == 2) && (dom[x + 1, y - 1].j1 == 1 || tab[x + 1, y - 1].juc == 2) && (dom[x, y - 1].j1 == 1 || tab[x, y - 1].juc == 2))
                            prege = 0;
                    }
                    else
                    {
                        if ((dom[x + 1, y].j1 == 1 || tab[x + 1, y].juc == 2) && (dom[x + 1, y + 1].j1 == 1 || tab[x + 1, y + 1].juc == 2) && (dom[x, y + 1].j1 == 1 || tab[x, y + 1].juc == 2) && (dom[x + 1, y - 1].j1 == 1 || tab[x + 1, y - 1].juc == 2) && (dom[x, y - 1].j1 == 1 || tab[x, y - 1].juc == 2))
                            prege = 0;
                    }
                }
                else if (x == 7)
                {
                    if (y == 0)
                    {
                        if ((dom[x - 1, y].j1 == 1 || tab[x - 1, y].juc == 2) && (dom[x - 1, y + 1].j1 == 1 || tab[x - 1, y + 1].juc == 2) && (dom[x, y + 1].j1 == 1 || tab[x, y + 1].juc == 2))
                            prege = 0;
                    }
                    else if (y == 7)
                    {
                        if ((dom[x - 1, y].j1 == 1 || tab[x - 1, y].juc == 2) && (dom[x - 1, y - 1].j1 == 1 || tab[x - 1, y - 1].juc == 2) && (dom[x, y - 1].j1 == 1 || tab[x, y - 1].juc == 2))
                            prege = 0;
                    }
                    else
                    {
                        if ((dom[x - 1, y].j1 == 1 || tab[x - 1, y].juc == 2) && (dom[x - 1, y + 1].j1 == 1 || tab[x - 1, y + 1].juc == 2) && (dom[x, y + 1].j1 == 1 || tab[x, y + 1].juc == 2) && (dom[x - 1, y - 1].j1 == 1 || tab[x - 1, y - 1].juc == 2) && (dom[x, y - 1].j1 == 1 || tab[x, y - 1].juc == 2))
                            prege = 0;
                    }
                }
                else if (y == 0)
                {
                    if ((dom[x - 1, y].j1 == 1 || tab[x - 1, y].juc == 2) && (dom[x - 1, y + 1].j1 == 1 || tab[x - 1, y + 1].juc == 2) && (dom[x, y + 1].j1 == 1 || tab[x, y + 1].juc == 2) && (dom[x + 1, y + 1].j1 == 1 || tab[x + 1, y + 1].juc == 2) && (dom[x + 1, y].j1 == 1 || tab[x + 1, y].juc == 2))
                        prege = 0;
                }
                else if (y == 7)
                {
                    if ((dom[x - 1, y].j1 == 1 || tab[x - 1, y].juc == 2) && (dom[x - 1, y - 1].j1 == 1 || tab[x - 1, y - 1].juc == 2) && (dom[x, y - 1].j1 == 1 || tab[x, y - 1].juc == 2) && (dom[x + 1, y - 1].j1 == 1 || tab[x + 1, y - 1].juc == 2) && (dom[x + 1, y].j1 == 1 || tab[x + 1, y].juc == 2))
                        prege = 0;
                }
                else
                {
                    if ((dom[x - 1, y].j1 == 1 || tab[x - 1, y].juc == 2) && (dom[x - 1, y + 1].j1 == 1 || tab[x - 1, y + 1].juc == 2) && (dom[x, y + 1].j1 == 1 || tab[x, y + 1].juc == 2) && (dom[x + 1, y + 1].j1 == 1 || tab[x + 1, y + 1].juc == 2) && (dom[x + 1, y].j1 == 1 || tab[x + 1, y].juc == 2) && (dom[x - 1, y - 1].j1 == 1 || tab[x - 1, y - 1].juc == 2) && (dom[x, y - 1].j1 == 1 || tab[x, y - 1].juc == 2) && (dom[x + 1, y - 1].j1 == 1 || tab[x + 1, y - 1].juc == 2))
                        prege = 0;
                }
                #endregion

                if (nrsah2 == 1)
                    if (prege == 0)
                    {
                        mod = 3;
                    }
                    else
                    {
                        mod = 2;
                    }

                if (nrsah1 == 0)
                {
                    coordonate a = new coordonate();
                    #region PIESA2-BLOCARE
                    switch (tab[sah1[0].x, sah1[0].y].piesa)
                    {
                        case 1: drumsah.Add(sah1[0]); break;
                        case 2:
                            {
                                if (x == sah1[0].x)
                                {
                                    a.x = x;
                                    if (y < sah1[0].y)
                                        for (int j = sah1[0].y; j > y; j--)
                                        {
                                            a.y = j;
                                            drumsah.Add(a);
                                        }
                                    if (y > sah1[0].y)
                                    {
                                        for (int j = sah1[0].y; j < y; j++)
                                        {
                                            a.y = j;
                                            drumsah.Add(a);
                                        }
                                    }
                                }
                                else if (y == sah1[0].y)
                                {
                                    a.y = y;
                                    if (x < sah1[0].x)
                                        for (int i = sah1[0].x; i > x; i--)
                                        {
                                            a.x = i;
                                            drumsah.Add(a);
                                        }
                                    if (x > sah1[0].x)
                                    {
                                        for (int i = sah1[0].x; i < x; i++)
                                        {
                                            a.x = i;
                                            drumsah.Add(a);
                                        }
                                    }
                                }
                                break;
                            }
                        case 3:
                            {
                                drumsah.Add(sah1[0]);
                                break;
                            }
                        case 4:
                            {

                                for (int i = 0; i < Math.Abs(x - sah1[0].x); i++)
                                {
                                    if (x < sah1[0].x)
                                        a.x = sah1[0].x - i;

                                    if (x > sah1[0].x)
                                        a.x = sah1[0].x + i;

                                    if (y < sah1[0].y)
                                        a.y = sah1[0].y - i;

                                    if (y > sah1[0].y)
                                        a.y = sah1[0].y + i;
                                    drumsah.Add(a);
                                }

                                break;
                            }
                        case 6:
                            {
                                if (x == sah1[0].x)
                                {
                                    a.x = x;
                                    if (y < sah1[0].y)
                                        for (int j = sah1[0].y; j > y; j--)
                                        {
                                            a.y = j;
                                            drumsah.Add(a);
                                        }
                                    if (y > sah1[0].y)
                                    {
                                        for (int j = sah1[0].y; j < y; j++)
                                        {
                                            a.y = j;
                                            drumsah.Add(a);
                                        }
                                    }
                                }
                                else if (y == sah1[0].y)
                                {
                                    a.y = y;
                                    if (x < sah1[0].x)
                                        for (int i = sah1[0].x; i > x; i--)
                                        {
                                            a.x = i;
                                            drumsah.Add(a);
                                        }
                                    if (x > sah1[0].x)
                                    {
                                        for (int i = sah1[0].x; i < x; i++)
                                        {
                                            a.x = i;
                                            drumsah.Add(a);
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < Math.Abs(x - sah1[0].x); i++)
                                    {
                                        if (x < sah1[0].x)
                                            a.x = sah1[0].x - i;

                                        if (x > sah1[0].x)
                                            a.x = sah1[0].x + i;

                                        if (y < sah1[0].y)
                                            a.y = sah1[0].y - i;

                                        if (y > sah1[0].y)
                                            a.y = sah1[0].y + i;
                                        drumsah.Add(a);
                                    }
                                }
                                break;
                            }
                    }
                    #endregion

                    for (int i = 0; i < 8 && ppiesa == 0; i++)
                        for (int j = 0; j < 8 && ppiesa == 0; j++)
                            if (dom[i, j].j2 == 1 && drumsah.Contains(new coordonate { x = i, y = j }))
                                ppiesa = 1;

                    if (prege == 0 && ppiesa == 0)
                    {
                        mod = 3;
                    }
                    else
                    {
                        mod = 2;
                    }

                }
            }
        }

        void copymat(ref tabsah [,]a, tabsah [,]b)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    a[i, j].piesa = b[i, j].piesa;
                   
                        a[i, j].juc = b[i, j].juc;
                }
        }

        bool vrfmut()
        {
            tabsah[,] auxtab = new tabsah[8, 8];
            int x = -1, y = -1;
            bool bl = false;
            copymat(ref auxtab, tab);
            if(juc==1)
            {
                tab[dest[1] - 1, dest[0] - 1].piesa = tab[select[1] - 1, select[0] - 1].piesa;
                tab[select[1] - 1, select[0] - 1].piesa = 0;

                tab[dest[1] - 1, dest[0] - 1].juc = tab[select[1] - 1, select[0] - 1].juc;
                tab[select[1] - 1, select[0] - 1].juc = 0;

                update_dom();

                for (int i = 0; i < 8&&x==-1; i++)
                    for (int j = 0; j < 8&&x==-1; j++)
                        if (tab[i, j].piesa == 5 && tab[i, j].juc == 1)
                        {
                            x = i;
                            y = j;
                        }

                if (dom[x, y].j2 == 0)
                    bl = true;

                tab = auxtab;
                update_dom();
            }
            if(juc==2)
            {
                 
                        tab[dest[1] - 1, dest[0] - 1].piesa = tab[select[1] - 1, select[0] - 1].piesa;
                        tab[select[1] - 1, select[0] - 1].piesa = 0;

                        tab[dest[1] - 1, dest[0] - 1].juc = tab[select[1] - 1, select[0] - 1].juc;
                        tab[select[1] - 1, select[0] - 1].juc = 0;

                        update_dom();

                        for (int i = 0; i < 8 && x == -1; i++)
                            for (int j = 0; j < 8 && x == -1; j++)
                                if (tab[i, j].piesa == 5 && tab[i, j].juc == 2)
                                {
                                    x = i;
                                    y = j;
                                }

                        if (dom[x, y].j1 == 0)
                            bl= true;

                        tab = auxtab;
                        update_dom();
                 
            }

            return bl;
        }

        private bool endSah()
        {
            tabsah[,] auxtab = new tabsah[8, 8];
            bool bl = false;
            copymat(ref auxtab, tab);
            if (juc == 1)
            {

                if (nrsah2 == 1)
                {
                    if (tab[select[1] - 1, select[0] - 1].piesa == 5 && dom[dest[1] - 1, dest[0] - 1].j2 == 0)
                    {
                        tab[dest[1] - 1, dest[0] - 1].piesa = tab[select[1] - 1, select[0] - 1].piesa;
                        tab[select[1] - 1, select[0] - 1].piesa = 0;

                        tab[dest[1] - 1, dest[0] - 1].juc = tab[select[1] - 1, select[0] - 1].juc;
                        tab[select[1] - 1, select[0] - 1].juc = 0;

                        update_dom();

                        if (dom[dest[1] - 1, dest[0] - 1].j2 == 0)
                           bl= true;

                        tab = auxtab;
                        update_dom();
                    }
                }
                else if (nrsah2 == 0)
                {
                    if ((tab[select[1] - 1, select[0] - 1].piesa == 5 && dom[dest[1] - 1, dest[0] - 1].j2 == 0) || drumsah.Contains(new coordonate { x = dest[1] - 1, y = dest[0] - 1 }))
                    {
                        bl = vrfmut();
                    }
                }
            }
            if (juc == 2)
            {
                if (nrsah1 == 1)
                {
                    if (tab[select[1] - 1, select[0] - 1].piesa == 5 && dom[dest[1] - 1, dest[0] - 1].j1 == 0)
                    {
                        tab[dest[1] - 1, dest[0] - 1].piesa = tab[select[1] - 1, select[0] - 1].piesa;
                        tab[select[1] - 1, select[0] - 1].piesa = 0;

                        tab[dest[1] - 1, dest[0] - 1].juc = tab[select[1] - 1, select[0] - 1].juc;
                        tab[select[1] - 1, select[0] - 1].juc = 0;

                        update_dom();

                        if (dom[dest[1] - 1, dest[0] - 1].j1 == 0)
                            bl= true;

                        tab = auxtab;
                        update_dom();

                    }
                }
                else if (nrsah1 == 0)
                {

                    if (tab[select[1] - 1, select[0] - 1].piesa == 5 && dom[dest[1] - 1, dest[0] - 1].j1 == 0 || drumsah.Contains(new coordonate { x = dest[1] - 1, y = dest[0] - 1 }))
                    {
                        bl = vrfmut();
                    }
                    
                }
            }
            return bl;
        }

        private void bt_Click(object sender, EventArgs e)
        {
            TranspCtrl CtrlClicked = sender as TranspCtrl;


            int a = retx(CtrlClicked.Name), b = rety(CtrlClicked.Name);

            if (mod == 0 && tab[b - 1, a - 1].juc == juc)
            {
                select[0] = retx(CtrlClicked.Name);
                select[1] = rety(CtrlClicked.Name);
                mod = (mod + 1) % 2;
            }
            else
                if (mod == 1 && ((tab[b - 1, a - 1].piesa == 0) || (tab[b - 1, a - 1].juc == juc % 2 + 1)) && tab[b - 1, a - 1].piesa != 5)
                {
                    dest[0] = retx(CtrlClicked.Name);
                    dest[1] = rety(CtrlClicked.Name);

                    if (mutare_valid(select[0] - 1, select[1] - 1, dest[0] - 1, dest[1] - 1, tab[select[1] - 1, select[0] - 1].piesa)&&vrfmut())
                    {
                        if (juc == 1)
                            drawpiesa(piesa_alb, dest[0], dest[1], tab[select[1] - 1, select[0] - 1].piesa);
                        else
                            drawpiesa(piesa_negru, dest[0], dest[1], tab[select[1] - 1, select[0] - 1].piesa);

                        if ((select[0] + select[1]) % 2 == 0)
                            rectang(tabla, alb, 180 + (select[0] - 1) * 80, 30 + (select[1] - 1) * 80, 80, 80);
                        else
                            rectang(tabla, negru, 180 + (select[0] - 1) * 80, 30 + (select[1] - 1) * 80, 80, 80);

                        tab[dest[1] - 1, dest[0] - 1].piesa = tab[select[1] - 1, select[0] - 1].piesa;
                        tab[select[1] - 1, select[0] - 1].piesa = 0;

                        tab[dest[1] - 1, dest[0] - 1].juc = tab[select[1] - 1, select[0] - 1].juc;
                        tab[select[1] - 1, select[0] - 1].juc = 0;


                        update_dom();
                        
                        juc = juc % 2 + 1;

                        if (juc == 1)
                        {
                            jc1.BackColor = SystemColors.Highlight;
                            jc2.BackColor = SystemColors.Control;
                        }
                        else
                        {
                            jc1.BackColor = SystemColors.Control;
                            jc2.BackColor = SystemColors.Highlight;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Move!");
                    }

                    mod = (mod + 1) % 2;

                    if (verif_sah())
                    {
                        drumsah.Clear();
                        mod_sah();
                    }
                }
                else if (mod == 2)
                {
                    select[0] = retx(CtrlClicked.Name);
                    select[1] = rety(CtrlClicked.Name);

                    mod = 4;
                }
                else if (mod == 4 && ((tab[b - 1, a - 1].piesa == 0) || (tab[b - 1, a - 1].juc == juc % 2 + 1)) && tab[b - 1, a - 1].piesa != 5)
                {
                    dest[0] = retx(CtrlClicked.Name);
                    dest[1] = rety(CtrlClicked.Name);
                   
                    if (mutare_valid(select[0] - 1, select[1] - 1, dest[0] - 1, dest[1] - 1, tab[select[1] - 1, select[0] - 1].piesa) && endSah())
                    {
                        if (juc == 1)
                            drawpiesa(piesa_alb, dest[0], dest[1], tab[select[1] - 1, select[0] - 1].piesa);
                        else
                            drawpiesa(piesa_negru, dest[0], dest[1], tab[select[1] - 1, select[0] - 1].piesa);

                        if ((select[0] + select[1]) % 2 == 0)
                            rectang(tabla, alb, 180 + (select[0] - 1) * 80, 30 + (select[1] - 1) * 80, 80, 80);
                        else
                            rectang(tabla, negru, 180 + (select[0] - 1) * 80, 30 + (select[1] - 1) * 80, 80, 80);

                        tab[dest[1] - 1, dest[0] - 1].piesa = tab[select[1] - 1, select[0] - 1].piesa;
                        tab[select[1] - 1, select[0] - 1].piesa = 0;

                        tab[dest[1] - 1, dest[0] - 1].juc = tab[select[1] - 1, select[0] - 1].juc;
                        tab[select[1] - 1, select[0] - 1].juc = 0;

                        drumsah.Clear();
                        nrsah1 = nrsah2 = -1;
                        sah1[0] = sah1[1] = sah2[0] = sah2[1] = new coordonate { x = 0, y = 0 };
                        mod = 0;

                        update_dom();

                        juc = juc % 2 + 1;

                        if (juc == 1)
                        {
                            jc1.BackColor = SystemColors.Highlight;
                            jc2.BackColor = SystemColors.Control;
                        }
                        else
                        {
                            jc1.BackColor = SystemColors.Control;
                            jc2.BackColor = SystemColors.Highlight;
                        }
                        if (verif_sah())
                        {
                            drumsah.Clear();
                            mod_sah();
                        }
                    }
                    else
                    {
                            MessageBox.Show("Invalid Move!");
                       
                            mod = 2;
                    }
                }

            if (mod == 3)
            {
                juc = juc % 2 + 1;
                if (juc == 1)
                    MessageBox.Show("Check-Mate!\n" + jucator1 + " won! Congratulations!");
                else
                    MessageBox.Show("Check-Mate!\n" + jucator2 + " won! Congratulations!");
                gameOver();
            }
        }

        private void drawtab()
        {
            rectang(tabla, rama, 150, 0, 700, 700);

            for (int i = 1; i <= 8; i++)
                for (int j = 1; j <= 8; j++)
                    if ((i + j) % 2 == 0)
                        rectang(tabla, alb, 180 + (i - 1) * 80, 30 + (j - 1) * 80, 80, 80);
                    else
                        rectang(tabla, negru, 180 + (i - 1) * 80, 30 + (j - 1) * 80, 80, 80);

            for (int i = 1; i <= 8; i++)
            {
                pion(tabla, piesa_negru, 180 + (i - 1) * 80, 110);
                pion(tabla, piesa_alb, 180 + (i - 1) * 80, 510);
            }

            for (int i = 1; i <= 8; i++)
                switch (i)
                {
                    case 1: tura(tabla, piesa_negru, 180 + (i - 1) * 80, 30); tura(tabla, piesa_alb, 180 + (i - 1) * 80, 590); break;
                    case 8: tura(tabla, piesa_negru, 180 + (i - 1) * 80, 30); tura(tabla, piesa_alb, 180 + (i - 1) * 80, 590); break;
                    case 2: cal(tabla, piesa_negru, 180 + (i - 1) * 80, 30); cal(tabla, piesa_alb, 180 + (i - 1) * 80, 590); break;
                    case 7: cal(tabla, piesa_negru, 180 + (i - 1) * 80, 30); cal(tabla, piesa_alb, 180 + (i - 1) * 80, 590); break;
                    case 3: nebun(tabla, piesa_negru, 180 + (i - 1) * 80, 30); nebun(tabla, piesa_alb, 180 + (i - 1) * 80, 590); break;
                    case 6: nebun(tabla, piesa_negru, 180 + (i - 1) * 80, 30); nebun(tabla, piesa_alb, 180 + (i - 1) * 80, 590); break;
                    case 4: regina(tabla, piesa_negru, 180 + (i - 1) * 80, 30); regina(tabla, piesa_alb, 180 + (i - 1) * 80, 590); break;
                    case 5: rege(tabla, piesa_negru, 180 + (i - 1) * 80, 30); rege(tabla, piesa_alb, 180 + (i - 1) * 80, 590); break;
                }
        }

        private void refreshTab()
        {
            rectang(tabla, rama, 150, 0, 700, 700);

            for (int i = 1; i <= 8; i++)
                for (int j = 1; j <= 8; j++)
                    if ((i + j) % 2 == 0)
                        rectang(tabla, alb, 180 + (i - 1) * 80, 30 + (j - 1) * 80, 80, 80);
                    else
                        rectang(tabla, negru, 180 + (i - 1) * 80, 30 + (j - 1) * 80, 80, 80);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (tab[i, j].juc == 1)
                        switch (tab[i, j].piesa)
                        {
                            case 1: pion(tabla, piesa_alb, 180 + j * 80, 30 + i * 80); break;
                            case 2: tura(tabla, piesa_alb, 180 + j * 80, 30 + i * 80); break;
                            case 3: cal(tabla, piesa_alb, 180 + j * 80, 30 + i * 80); break;
                            case 4: nebun(tabla, piesa_alb, 180 + j * 80, 30 + i * 80); break;
                            case 5: rege(tabla, piesa_alb, 180 + j * 80, 30 + i * 80); break;
                            case 6: regina(tabla, piesa_alb, 180 + j * 80, 30 + i * 80); break;
                        }
                    else
                        switch (tab[i, j].piesa)
                        {
                            case 1: pion(tabla, piesa_negru, 180 + j * 80, 30 + i * 80); break;
                            case 2: tura(tabla, piesa_negru, 180 + j * 80, 30 + i * 80); break;
                            case 3: cal(tabla, piesa_negru, 180 + j * 80, 30 + i * 80); break;
                            case 4: nebun(tabla, piesa_negru, 180 + j * 80, 30 + i * 80); break;
                            case 5: rege(tabla, piesa_negru, 180 + j * 80, 30 + i * 80); break;
                            case 6: regina(tabla, piesa_negru, 180 + j * 80, 30 + i * 80); break;
                        }
        }

        private void gameOver()
        {
            resetabla();
            jucator1 = jucator2 = "";
            mod=nrsah1=nrsah2= -1;
            dest[0]=dest[1] = select[0]=select[1] = 0;
            juc = 1;
            sah1.Initialize();
            sah2.Initialize();
            drumsah.Clear();
            jc1.Text = jc2.Text = "";
            jc1.Visible = jc2.Visible = false;
            juc1.Text = juc2.Text = "";
            tabla.Clear(SystemColors.Control);
            juc1.Visible = juc1_lb.Visible = juc2.Visible = juc2_lb.Visible = start_button.Visible = true;

        }

        private void start_Click(object sender, EventArgs e)
        {
            if (juc1.Text.Length == 0 || juc2.Text.Length == 0)
                MessageBox.Show("Input the names of the two players!");
            else
            {
                juc1.Visible = juc1_lb.Visible = juc2.Visible = juc2_lb.Visible = false;
                mod = 0;
                jc1.Visible = jc2.Visible = true;
                jucator1 = jc1.Text = juc1.Text;
                jucator2 = jc2.Text = juc2.Text;
                jc1.BackColor = SystemColors.Highlight;
                start_button.Visible = false;

                tabla = this.CreateGraphics();
                tabla.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                tabla.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                buttons();

                resetabla();
            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            if (mod != -1)
                if (!isFormFullyVis(this))
                    refreshTab();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (mod != -1)
            {
                refreshTab();
            }
        }

        bool isPointVisOnScreen(Point p)
        {
            foreach(Screen s in Screen.AllScreens)
            {
                if (p.X < s.Bounds.Right && p.X > s.Bounds.Left && p.Y > s.Bounds.Top && p.Y < s.Bounds.Bottom)
                    return true;
            }
            return false;
        }

        bool isFormFullyVis(Form f)
        {
            return isPointVisOnScreen(new Point(f.Left, f.Top)) && isPointVisOnScreen(new Point(f.Right, f.Top)) && isPointVisOnScreen(new Point(f.Right, f.Bottom)) && isPointVisOnScreen(new Point(f.Left, f.Bottom));
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (mod != -1)
            {
                refreshTab();
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (mod != -1)
            {
                if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized || WindowState == FormWindowState.Minimized)
                    refreshTab();
            }
        }

    }

    public class TranspCtrl : Control
    {
        public bool drag = false;
        public bool enab = false;
        private int m_opacity = 100;

        private int alpha;
        public TranspCtrl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = Color.Transparent;
        }

        public int Opacity
        {
            get
            {
                if (m_opacity > 100)
                {
                    m_opacity = 100;
                }
                else if (m_opacity < 1)
                {
                    m_opacity = 1;
                }
                return this.m_opacity;
            }
            set
            {
                this.m_opacity = value;
                if (this.Parent != null)
                {
                    Parent.Invalidate(this.Bounds, true);
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            Color frmColor = this.Parent.BackColor;
            Brush bckColor = default(Brush);

            alpha = (m_opacity * 255) / 100;

            if (drag)
            {
                Color dragBckColor = default(Color);

                if (BackColor != Color.Transparent)
                {
                    int Rb = BackColor.R * alpha / 255 + frmColor.R * (255 - alpha) / 255;
                    int Gb = BackColor.G * alpha / 255 + frmColor.G * (255 - alpha) / 255;
                    int Bb = BackColor.B * alpha / 255 + frmColor.B * (255 - alpha) / 255;
                    dragBckColor = Color.FromArgb(Rb, Gb, Bb);
                }
                else
                {
                    dragBckColor = frmColor;
                }

                alpha = 255;
                bckColor = new SolidBrush(Color.FromArgb(alpha, dragBckColor));
            }
            else
            {
                bckColor = new SolidBrush(Color.FromArgb(alpha, this.BackColor));
            }

            if (this.BackColor != Color.Transparent | drag)
            {
                g.FillRectangle(bckColor, bounds);
            }

            bckColor.Dispose();
            g.Dispose();
            base.OnPaint(e);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            if (this.Parent != null)
            {
                Parent.Invalidate(this.Bounds, true);
            }
            base.OnBackColorChanged(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnParentBackColorChanged(e);
        }
    }
}
