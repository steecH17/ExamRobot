using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamRobot
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        //игровая карта
        List<List<int>> levelList = new List<List<int>>() {
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
        Point playerCoords;//координаты управляемого обьекта
        int countSteps = 0;//количество выполненных действий из лист бокса
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(panelLevel.ClientSize.Width, panelLevel.ClientSize.Height);
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, panelLevel, new object[] { true });
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            listBox.Items.Add("Влево");
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            listBox.Items.Add("Вверх");
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            listBox.Items.Add("Вправо");
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            listBox.Items.Add("Вниз");
        }

        private void buttonGeneratedLevel_Click(object sender, EventArgs e)
        {
            
            levelList = new List<List<int>>() {
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};

            countSteps = 0;
            GeneratedLevel();//генерация карты
            panelLevel.Invalidate();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void panelLevel_Paint(object sender, PaintEventArgs e)
        {
            bmp = new Bitmap(panelLevel.ClientSize.Width, panelLevel.ClientSize.Height);

            DrawPlayingField();//отрисовка карты(препятсвий и самого игрока)
            RenderMesh();//отрисовка сетки

            e.Graphics.DrawImageUnscaled(bmp, 0, 0);
        }

        private void RenderMesh()
        {
            Graphics graphics = Graphics.FromImage(bmp);
            int width = panelLevel.ClientSize.Width;
            int height = panelLevel.ClientSize.Height;
            //MessageBox.Show(width.ToString(), height.ToString());
            int stepX = width / 10;
            int stepY = height / 10;
            Point p1, p2;
            //Vertical
            for (int x = 0; x <= stepX * 10; x += stepX)
            {
                p1 = new Point(x, 0);
                p2 = new Point(x, stepY * 10);
                graphics.DrawLine(Pens.Black, p1, p2);
            }
            //Horizontal
            for (int y = 0; y <= stepY * 10; y += stepY)
            {
                p1 = new Point(0, y);
                p2 = new Point(stepX * 10, y);
                graphics.DrawLine(Pens.Black, p1, p2);
            }
        }

        private void DrawPlayingField()
        {
            Graphics graphics = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.DarkGray);
            SolidBrush brushPlayer = new SolidBrush(Color.Red);

            int width = panelLevel.ClientSize.Width;
            int height = panelLevel.ClientSize.Height;
            int stepX = width / 10;
            int stepY = height / 10;
            for (int i = 0; i < levelList.Count; i++)
            {
                for (int j = 0; j < levelList[i].Count; j++)
                {
                    if (levelList[i][j] == 1)
                    {
                        graphics.FillRectangle(brush, stepX * j, stepY * i, stepX, stepY);//отрисовка препятсвий
                    }
                    else if (levelList[i][j] == 2)
                    {
                        graphics.FillEllipse(brushPlayer, stepX * j, stepY * i, stepX, stepY);//отрисовка игрока
                    }
                }
            }


        }

        private void GeneratedLevel()
        {
            int countBlocks = random.Next(13, 21);
            int x, y;

            for(int i = 0; i < countBlocks;i++)
            {
                x = random.Next(0, 10);
                y = random.Next(0, 10);
                levelList[y][x] = 1;//ставим препятсвия на карту
            }
            x = random.Next(0, 10);
            y = random.Next(0, 10);
            levelList[y][x] = 2;//ставим на карту управляемый обьект
            playerCoords = new Point(x, y);//запоминаем координаты игрока на карте
        }

        private void GameOver()
        {
            timer.Stop();
            MessageBox.Show("Управляемы обьект врезался! Игра окончена!");
            levelList = new List<List<int>>() {
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};

            countSteps = 0;
            listBox.Items.Clear();
            GeneratedLevel();
            panelLevel.Invalidate();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            string action = listBox.Items[countSteps].ToString();//берем какое действие надо выполнить
            countSteps++;//увеличиваем счетчик, чтобы на следующем тике выполнять следующее по списку действие
            if (countSteps == listBox.Items.Count) timer.Stop();// если выполнили все действия выключаем таймер
            switch (action)
            {
                case "Влево":
                    {
                        if (playerCoords.X - 1 >= 0 && levelList[playerCoords.Y][playerCoords.X - 1] == 0)
                        {
                            levelList[playerCoords.Y][playerCoords.X] = 0;//убираем игрока с предыдщего места
                            playerCoords.X -= 1;
                            levelList[playerCoords.Y][playerCoords.X] = 2;//перемещаем на следующее
                        }
                        else GameOver();//значит игра окончена(проигрыш)
                        break;
                    }

                case "Вправо":
                    {
                        if (playerCoords.X + 1 <= 9 && levelList[playerCoords.Y][playerCoords.X + 1] == 0)
                        {
                            levelList[playerCoords.Y][playerCoords.X] = 0;
                            playerCoords.X += 1;
                            levelList[playerCoords.Y][playerCoords.X] = 2;
                        }
                        else GameOver();
                        break;
                    }
                case "Вверх":
                    {
                        if (playerCoords.Y - 1 >= 0 && levelList[playerCoords.Y - 1][playerCoords.X] == 0)
                        {
                            levelList[playerCoords.Y][playerCoords.X] = 0;
                            playerCoords.Y -= 1;
                            levelList[playerCoords.Y][playerCoords.X] = 2;
                        }
                        else GameOver();
                        break;
                    }
                case "Вниз":
                    {
                        if (playerCoords.Y + 1 <= 9 && levelList[playerCoords.Y + 1][playerCoords.X] == 0)
                        {
                            levelList[playerCoords.Y][playerCoords.X] = 0;
                            playerCoords.Y += 1;
                            levelList[playerCoords.Y][playerCoords.X] = 2;
                        }
                        else GameOver();
                        break;
                    }
                default: break;
            }

            panelLevel.Invalidate();
        }
    }
}
