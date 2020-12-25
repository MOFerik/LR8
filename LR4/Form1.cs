using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LR4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.panel1.BackColor = System.Drawing.Color.White;
            stor.Notify += updateTree;
            treeView1.Nodes.Add("Корень");
        }    

        public class CCircle // Класс фигур
        {
            public List<CCircle> arr = new List<CCircle>();
            public int x;
            public int y;
            public int r = 60;
            public bool flag;
            public int figure;
            public int lineX1;
            public int lineX2;
            public int lineY1;
            public int lineY2;
            public Color clr = Color.Black;

            public virtual int IfGroup()
            {
                return 0;
            }

            public virtual void Save()
            {
                File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt", this.IfGroup() + "\n");
                File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt", this.figure + "\n");
                if (this.figure == 3)
                {
                    File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt", this.lineX1 + "\n");
                    File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.lineX2 + "\n");
                    File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.lineY1 + "\n");
                    File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.lineY2 + "\n");
                }
                else
                {
                    File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.x + "\n");
                    File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.y + "\n");
                    File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.r + "\n");
                }
                File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.clr.ToArgb().ToString() + "\n");
                File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.flag + "\n");
            }

            public virtual void Load(StreamReader reader)
            {
                this.figure = Convert.ToInt32(reader.ReadLine());
                if (this.figure == 3)
                {
                    this.lineX1 = Convert.ToInt32(reader.ReadLine());
                    this.lineX2 = Convert.ToInt32(reader.ReadLine());
                    this.lineY1 = Convert.ToInt32(reader.ReadLine());
                    this.lineY2 = Convert.ToInt32(reader.ReadLine());
                }
                else
                {
                    this.x = Convert.ToInt32(reader.ReadLine());
                    this.y = Convert.ToInt32(reader.ReadLine());
                    this.r = Convert.ToInt32(reader.ReadLine());
                }
                this.clr = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                this.flag = (reader.ReadLine() == "True");
                return;
            }
        }

        public class Glue : CCircle
        {
            public delegate void glMoved();
            public event glMoved glMove;
            public int figure = 5;
            
        }

        public void AddGlued(CCircle glue, CCircle circ)
        {
            glue.arr.Add(circ);
        }

        public void DeleteGlued(CCircle glue, CCircle circ)
        {
            glue.arr.Remove(circ);
        }

        public void MoveGlued(CCircle circ, int dx, int dy)
        {
            if (circ.IfGroup() == 0)
            {
                Draw(circ, Color.White);
                circ.x += dx;
                circ.y += dy;
                Draw(circ, circ.clr);
            }
            else
            {
                for (int i = 0; i < circ.arr.Count; i++)
                {
                    MoveGlued(circ.arr[i], dx, dy);
                }
            }
        }

        public void GlueTo(int x, int y, int r, CCircle glue)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j] != glue)
                {
                    if (stor.arr[j].IfGroup() == 0)
                    {
                        if (stor.arr[j].figure != 3)
                        {
                            if ((Math.Abs(x - stor.arr[j].x) < r / 2 + stor.arr[j].r / 2) && (Math.Abs(y - stor.arr[j].y) < r / 2 + stor.arr[j].r / 2))
                            {
                                int found = 0;
                                for (int q = 0; q < glue.arr.Count; q++)
                                {
                                    if (glue.arr[q] == stor.arr[j])
                                        found++;
                                }
                                if (found == 0)
                                    AddGlued(glue, stor.arr[j]);
                            }
                        }
                    }
                    else
                    {
                        bool found = false;
                        found = stor.CheckGroup(x - glue.r / 2, y - glue.r/2, stor.arr[j]);
                        if (found)
                        {
                            AddGlued(glue, stor.arr[j]);
                            break;
                        }
                        found = stor.CheckGroup(x + glue.r / 2, y + glue.r/2, stor.arr[j]);
                        if (found)
                        {
                            AddGlued(glue, stor.arr[j]);
                            break;
                        }
                        found = stor.CheckGroup(x - glue.r/2, y + glue.r / 2, stor.arr[j]);
                        if (found)
                        {
                            AddGlued(glue, stor.arr[j]);
                            break;
                        }
                        found = stor.CheckGroup(x + glue.r/2, y - glue.r / 2, stor.arr[j]);
                        if (found)
                        {
                            AddGlued(glue, stor.arr[j]);
                            break;
                        }
                        found = stor.CheckGroup(x, y - glue.r / 2, stor.arr[j]);
                        if (found)
                        {
                            AddGlued(glue, stor.arr[j]);
                            break;
                        }
                        found = stor.CheckGroup(x, y + glue.r / 2, stor.arr[j]);
                        if (found)
                        {
                            AddGlued(glue, stor.arr[j]);
                            break;
                        }
                        found = stor.CheckGroup(x - glue.r / 2, y, stor.arr[j]);
                        if (found)
                        {
                            AddGlued(glue, stor.arr[j]);
                            break;
                        }
                        found = stor.CheckGroup(x + glue.r / 2, y, stor.arr[j]);
                        if (found)
                        {
                            AddGlued(glue, stor.arr[j]);
                            break;
                        }
                    }
                }
            }
        }

        public class CircleStorage // Класс-хранилище фигур
        {
            public delegate void AddedNode();
            public event AddedNode Notify;
            int div(int a, int b)
            {
                if (b != 0)
                    return a / b;
                else
                    return 0;
            }

            public List<CCircle> arr = new List<CCircle>();

            public int readyLine = -1;

            public bool ctrlPress = false;

            bool select;
            public bool Check(int x, int y) // Функция проверки нажатия на объект
            {
                List<int> selected = new List<int>();
                select = false;
                for (int j = 0; j < this.arr.Count; j++)
                {
                    if (this.arr[j].IfGroup() == 0)
                    {
                        if (this.arr[j].figure != 3)
                        {
                            if (x > this.arr[j].x - this.arr[j].r / 2 && x < this.arr[j].x + this.arr[j].r / 2 && y > this.arr[j].y - this.arr[j].r / 2 && y < this.arr[j].y + this.arr[j].r / 2)
                            {
                                selected.Add(j);
                                select = true;
                            }
                        }
                        else
                        {
                            if (!(x > this.arr[j].lineX1 && x > this.arr[j].lineX2) && !(y > this.arr[j].lineY1 && y > this.arr[j].lineY2) && !(x < this.arr[j].lineX1 && x < this.arr[j].lineX2) && !(y < this.arr[j].lineY1 && y < this.arr[j].lineY2) && div((x - this.arr[j].lineX1), (y - this.arr[j].lineY1)) == div((this.arr[j].lineX2 - this.arr[j].lineX1), (this.arr[j].lineY2 - this.arr[j].lineY1)))
                            {
                                selected.Add(j);
                                select = true;
                            }
                        }
                    }
                    else
                    {
                        bool found = CheckGroup(x, y, this.arr[j]);
                        if (found)
                        {
                            selected.Add(j);
                            select = true;
                        }
                    }
                }
                if (ModifierKeys.HasFlag(Keys.Control) != true)
                {
                    for (int j = 0; j < this.arr.Count; j++)
                    {
                        this.arr[j].flag = false;
                    }
                }
                for (int q = 0; q < selected.Count; q++)
                {
                    this.arr[selected[q]].flag = true;
                }
                Notify.Invoke();
                return select;
            }

            public bool CheckGroup(int x, int y, CCircle group)
            {
                for (int q = 0; q < group.arr.Count; q++)
                {
                    if (group.arr[q].IfGroup() == 0)
                    {
                        if (group.arr[q].figure != 3)
                        {
                            if (x > group.arr[q].x - group.arr[q].r / 2 && x < group.arr[q].x + group.arr[q].r / 2 && y > group.arr[q].y - group.arr[q].r / 2 && y < group.arr[q].y + group.arr[q].r / 2)
                            {

                                return true;
                            }
                        }
                        else
                        {
                            if (!(x > group.arr[q].lineX1 && x > group.arr[q].lineX2) && !(y > group.arr[q].lineY1 && y > group.arr[q].lineY2) && !(x < group.arr[q].lineX1 && x < group.arr[q].lineX2) && !(y < group.arr[q].lineY1 && y < group.arr[q].lineY2) && div((x - group.arr[q].lineX1), (y - group.arr[q].lineY1)) == div((group.arr[q].lineX2 - group.arr[q].lineX1), (group.arr[q].lineY2 - group.arr[q].lineY1)))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        bool found = CheckGroup(x, y, group.arr[q]);
                        if (found)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public void AddStor(CCircle circ) // Добавление созданного объекта в хранилище
            {
                arr.Add(circ);
                Notify.Invoke();
            }

            public void DelStor(CCircle circ)
            {
                arr.Remove(circ);
                Notify.Invoke();
            }

            public void SaveStor()
            {
                File.WriteAllText(@"C:\Users\deadp\Desktop\save.txt", this.arr.Count + "\n");
                for (int i = 0; i < this.arr.Count; i++)
                {
                    this.arr[i].Save();
                }
            }

            public void LoadStor()
            {
                FileStream ifi = new FileStream(@"C:\Users\deadp\Desktop\save.txt", FileMode.Open);
                StreamReader reader = new StreamReader(ifi);
                int count = Convert.ToInt32(reader.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    if (Convert.ToInt32(reader.ReadLine()) == 0)
                    {
                        CCircle circ = new CCircle();
                        circ.Load(reader);
                        this.AddStor(circ);
                    }
                    else
                    {
                        GroupedFigures group = new GroupedFigures();
                        group.Load(reader);
                        this.AddStor(group);
                    }
                }
                reader.Close();
            }
        }

        public class GroupedFigures : CCircle
        {
            public void AddGroup(CCircle circ) // Добавление созданного объекта в хранилище
            {
                arr.Add(circ);
            }

            public override int IfGroup()
            {
                return 1;
            }

            public override void Save()
            {
                File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.IfGroup() + "\n");
                File.AppendAllText(@"C:\Users\deadp\Desktop\save.txt",this.arr.Count + "\n");
                for (int i = 0; i < this.arr.Count; i++)
                {
                    this.arr[i].Save();
                }
            }

            public override void Load(StreamReader reader)
            {
                int count = Convert.ToInt32(reader.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    if (Convert.ToInt32(reader.ReadLine()) == 0)
                    {
                        CCircle circ = new CCircle();
                        circ.Load(reader);
                        this.arr.Add(circ);
                    }
                    else
                    {
                        GroupedFigures group = new GroupedFigures();
                        group.Load(reader);
                        this.arr.Add(group);
                    }
                }
            }
        }

        CircleStorage stor = new CircleStorage();

        public void Draw(CCircle circle, Color clr)
        {
            Pen mPen = new Pen(clr, 3);
            SolidBrush brush = new SolidBrush(clr);
            Rectangle rect = new Rectangle(circle.x - circle.r / 2, circle.y - circle.r / 2, circle.r, circle.r);
            Rectangle dot = new Rectangle(circle.x, circle.y, 2, 2);
            switch (circle.figure)
            {
                case 0:
                    this.panel1.CreateGraphics().DrawEllipse(mPen, rect);
                    break;
                case 1:
                    this.panel1.CreateGraphics().DrawRectangle(mPen, rect);
                    break;
                case 2:
                    this.panel1.CreateGraphics().DrawPolygon(mPen, new PointF[] { new PointF(circle.x - circle.r / 2, circle.y + circle.r / 2), new PointF(circle.x + circle.r / 2, circle.y + circle.r / 2), new PointF(circle.x, circle.y - circle.r / 2) });
                    break;
                case 3:
                    if (stor.readyLine >= 0)
                        panel1.CreateGraphics().FillEllipse(brush, dot);
                    else
                    {
                        this.panel1.CreateGraphics().DrawLine(mPen, circle.lineX1, circle.lineY1, circle.lineX2, circle.lineY2);
                        panel1.CreateGraphics().FillEllipse(brush, circle.lineX1, circle.lineY1, 2, 2);
                        panel1.CreateGraphics().FillEllipse(brush, circle.lineX2, circle.lineY2, 2, 2);
                    }
                    break;
                case 4:
                    panel1.CreateGraphics().FillEllipse(brush, dot);
                    break;
                case 5:
                    panel1.CreateGraphics().DrawEllipse(mPen, rect);
                    break;
            }
        }

        public void DrawGroup(CCircle group, Color clr)
        {
            for (int i = 0; i < group.arr.Count; i++)
            {
                if (group.arr[i].IfGroup() == 0)
                {
                    if (clr != Color.Red && clr != Color.White)
                    {
                        clr = group.arr[i].clr;
                    }
                    Pen mPen = new Pen(clr, 3);
                    SolidBrush brush = new SolidBrush(clr);
                    Rectangle rect = new Rectangle(group.arr[i].x - group.arr[i].r / 2, group.arr[i].y - group.arr[i].r / 2, group.arr[i].r, group.arr[i].r);
                    Rectangle dot = new Rectangle(group.arr[i].x, group.arr[i].y, 2, 2);
                    switch (group.arr[i].figure)
                    {
                        case 0:
                            this.panel1.CreateGraphics().DrawEllipse(mPen, rect);
                            break;
                        case 1:
                            this.panel1.CreateGraphics().DrawRectangle(mPen, rect);
                            break;
                        case 2:
                            this.panel1.CreateGraphics().DrawPolygon(mPen, new PointF[] { new PointF(group.arr[i].x - group.arr[i].r / 2, group.arr[i].y + group.arr[i].r / 2), new PointF(group.arr[i].x + group.arr[i].r / 2, group.arr[i].y + group.arr[i].r / 2), new PointF(group.arr[i].x, group.arr[i].y - group.arr[i].r / 2) });
                            break;
                        case 3:
                            if (stor.readyLine >= 0)
                                panel1.CreateGraphics().FillEllipse(brush, dot);
                            else
                            {
                                this.panel1.CreateGraphics().DrawLine(mPen, group.arr[i].lineX1, group.arr[i].lineY1, group.arr[i].lineX2, group.arr[i].lineY2);
                                panel1.CreateGraphics().FillEllipse(brush, group.arr[i].lineX1, group.arr[i].lineY1, 2, 2);
                                panel1.CreateGraphics().FillEllipse(brush, group.arr[i].lineX2, group.arr[i].lineY2, 2, 2);
                            }
                            break;
                        case 4:
                            panel1.CreateGraphics().FillEllipse(brush, dot);
                            break;
                        case 5:
                            this.panel1.CreateGraphics().DrawEllipse(mPen, rect);
                            break;
                    }
                }
                else
                {
                    DrawGroup(group.arr[i], clr);
                }
            }
        }

        Pen aPen = new Pen(Color.Red, 3);
        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (stor.readyLine >= 0 && stor.arr[stor.readyLine] == null)
            {
                stor.readyLine = -1;
            }
            if (stor.Check(e.X, e.Y)) // При нажатии на объект
            {
                for (int j = 0; j < stor.arr.Count; j++) // Отрисовка объектов с учетом выделения
                {
                        if (stor.arr[j].flag)
                        {
                            if (stor.arr[j].IfGroup() == 0)
                                Draw(stor.arr[j], Color.Red);
                            else
                                DrawGroup(stor.arr[j], Color.Red);
                        }
                        else
                        {
                            if (stor.arr[j].IfGroup() == 0)
                                Draw(stor.arr[j], stor.arr[j].clr);
                            else
                                DrawGroup(stor.arr[j], stor.arr[j].clr);
                        }
                }
            }
            else // При нажатии на холст
            {
                CCircle circ = new CCircle(); // Создание нового объекта
                circ.x = e.X;
                circ.y = e.Y;
                circ.r = Convert.ToInt32(numericUpDown1.Value);
                circ.clr = colorDialog1.Color;
                stor.AddStor(circ);
                if (listBox1.SelectedIndex == 3 && stor.readyLine >= 0)
                {
                    stor.arr[stor.arr.Count - 1].figure = 3;
                    Pen wPen = new Pen(Color.White, 3);
                    Rectangle dot = new Rectangle(stor.arr[stor.readyLine].x, stor.arr[stor.readyLine].y, 2, 2);
                    panel1.CreateGraphics().FillEllipse(Brushes.White, dot);
                    this.panel1.CreateGraphics().DrawLine(aPen, circ.x, circ.y, stor.arr[stor.readyLine].x, stor.arr[stor.readyLine].y);
                    stor.arr[stor.arr.Count - 1].lineX1 = stor.arr[stor.arr.Count - 1].x;
                    stor.arr[stor.arr.Count - 1].lineY1 = stor.arr[stor.arr.Count - 1].y;
                    stor.arr[stor.arr.Count - 1].lineX2 = stor.arr[stor.readyLine].x;
                    stor.arr[stor.arr.Count - 1].lineY2 = stor.arr[stor.readyLine].y;

                    stor.readyLine = -1;
                }
                else
                {
                    for (int j = 0; j < stor.arr.Count; j++)
                    {

                            if (j != (stor.arr.Count - 1)) // Снятие выделения с других объектов и отрисовка их
                            {
                                stor.arr[j].flag = false;
                                if (stor.arr[j].IfGroup() == 0)
                                    Draw(stor.arr[j], stor.arr[j].clr);
                                else
                                    DrawGroup(stor.arr[j], stor.arr[j].clr);
                            }
                            else // Выделение нового объекта и его отрисовка
                            {
                                stor.arr[j].flag = true;
                                Rectangle rect = new Rectangle(stor.arr[j].x - stor.arr[j].r / 2, stor.arr[j].y - stor.arr[j].r / 2, stor.arr[j].r, stor.arr[j].r);
                                Rectangle dot = new Rectangle(stor.arr[j].x, stor.arr[j].y, 2, 2);
                                switch (listBox1.SelectedIndex)
                                {
                                    case 0:
                                        this.panel1.CreateGraphics().DrawEllipse(aPen, rect);
                                        circ.figure = 0;
                                        break;
                                    case 1:
                                        this.panel1.CreateGraphics().DrawRectangle(aPen, rect);
                                        circ.figure = 1;
                                        break;
                                    case 2:
                                        this.panel1.CreateGraphics().DrawPolygon(aPen, new PointF[] { new PointF(stor.arr[j].x - stor.arr[j].r / 2, stor.arr[j].y + stor.arr[j].r / 2), new PointF(stor.arr[j].x + stor.arr[j].r / 2, stor.arr[j].y + stor.arr[j].r / 2), new PointF(stor.arr[j].x, stor.arr[j].y - stor.arr[j].r / 2) });
                                        circ.figure = 2;
                                        break;
                                    case 3:
                                        panel1.CreateGraphics().FillEllipse(Brushes.Red, dot);
                                        stor.readyLine = j;
                                        stor.arr[j].r = 6;
                                        circ.figure = 3;
                                        break;
                                    case 4:
                                        panel1.CreateGraphics().FillEllipse(Brushes.Red, dot);
                                        circ.figure = 4;
                                        stor.arr[j].r = 6;
                                        break;
                                    default:
                                        this.panel1.CreateGraphics().DrawEllipse(aPen, rect);
                                        circ.figure = 5;
                                    break;
                                }
                            }
                    }
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == true)
                {
                    for (int q = 0; q < stor.arr.Count; q++)
                    {
                        if(stor.arr[q].figure == 5)
                            DeleteGlued(stor.arr[q], stor.arr[j]);
                    }
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], Color.White);
                    else
                        DrawGroup(stor.arr[j], Color.White);
                    stor.DelStor(stor.arr[j]);
                    j--;
                }
            }
            for (int j = 0; j < stor.arr.Count; j++)
            {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], stor.arr[j].clr);
                    else
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
            }
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == true)
                {
                    if (stor.arr[j].IfGroup() == 0)
                    {
                        Draw(stor.arr[j], Color.White);
                        stor.arr[j].r = Convert.ToInt32(numericUpDown1.Value);
                        Draw(stor.arr[j], Color.Red);
                    }
                    else
                    {
                        DrawGroup(stor.arr[j], Color.White);
                        for (int i = 0; i < stor.arr[j].arr.Count; i++)
                        {
                            stor.arr[j].arr[i].r = Convert.ToInt32(numericUpDown1.Value);
                        }
                        DrawGroup(stor.arr[j], Color.Red);
                    }
                }
            }
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == false)
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], stor.arr[j].clr);
                    else
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel2.BackColor = colorDialog1.Color;
                for (int j = 0; j < stor.arr.Count; j++)
                {
                    if (stor.arr[j].flag == true)
                    {
                        if (stor.arr[j].IfGroup() == 0)
                        {
                            stor.arr[j].clr = colorDialog1.Color;
                            Draw(stor.arr[j], stor.arr[j].clr);
                        }
                        else
                        {
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                stor.arr[j].arr[i].clr = colorDialog1.Color;
                            }
                            DrawGroup(stor.arr[j], stor.arr[j].clr);
                        }
                    }
                }
            }
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == true)
                {
                    if (stor.arr[j].IfGroup() == 0)
                    {
                        if ((stor.arr[j].figure == 5) && (stor.arr[j].y - (stor.arr[j].r / 2) > 0))
                        {
                            Draw(stor.arr[j], Color.White);
                            stor.arr[j].y--;
                            Draw(stor.arr[j], Color.Red);
                            GlueTo(stor.arr[j].x, stor.arr[j].y, stor.arr[j].r, stor.arr[j]);
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                MoveGlued(stor.arr[j].arr[i], 0, -1);
                            }
                        }
                        else if (stor.arr[j].figure == 3)
                        {
                            if ((stor.readyLine == -1) && (stor.arr[j].lineY1 > 0) && (stor.arr[j].lineY2 > 0))
                            {
                                Draw(stor.arr[j], Color.White);
                                stor.arr[j].lineY1--;
                                stor.arr[j].lineY2--;
                                Draw(stor.arr[j], Color.Red);
                            }
                        }
                        else if (stor.arr[j].y - (stor.arr[j].r / 2) > 0)
                        {
                            Draw(stor.arr[j], Color.White);
                            stor.arr[j].y--;
                            Draw(stor.arr[j], Color.Red);
                        }
                    }
                    else
                    {
                        int able = 1;
                        for (int i = 0; i < stor.arr[j].arr.Count; i++)
                        {
                            if (stor.arr[j].arr[i].figure == 3)
                            {
                                if ((stor.readyLine == -1) && !((stor.arr[j].arr[i].lineY1 > 0) && (stor.arr[j].arr[i].lineY2 > 0)))
                                    able = 0;
                            }
                            else if (!(stor.arr[j].arr[i].y - (stor.arr[j].arr[i].r / 2) > 0))
                                able = 0;
                        }

                        if (able == 1)
                        {
                            DrawGroup(stor.arr[j], Color.White);
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                if (stor.arr[j].arr[i].figure == 3)
                                {
                                    stor.arr[j].arr[i].lineY1--;
                                    stor.arr[j].arr[i].lineY2--;
                                }
                                else
                                    stor.arr[j].arr[i].y--;
                            }
                            DrawGroup(stor.arr[j], Color.Red);
                        }
                    }
                }
            }
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == false)
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], stor.arr[j].clr);
                    else
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == true)
                {
                    if (stor.arr[j].IfGroup() == 0)
                    {
                        if ((stor.arr[j].figure == 5) && (stor.arr[j].y + (stor.arr[j].r / 2) < panel1.Height - 4))
                        {
                            Draw(stor.arr[j], Color.White);
                            stor.arr[j].y++;
                            Draw(stor.arr[j], Color.Red);
                            GlueTo(stor.arr[j].x, stor.arr[j].y, stor.arr[j].r, stor.arr[j]);
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                MoveGlued(stor.arr[j].arr[i], 0, 1);
                            }
                        }
                        if (stor.arr[j].figure == 3)
                        {
                            if ((stor.readyLine == -1) && (stor.arr[j].lineY1 < panel1.Height - 4) && (stor.arr[j].lineY2 < panel1.Height - 4))
                            {
                                Draw(stor.arr[j], Color.White);
                                stor.arr[j].lineY1++;
                                stor.arr[j].lineY2++;
                                Draw(stor.arr[j], Color.Red);
                            }
                        }
                        else if (stor.arr[j].y + (stor.arr[j].r / 2) < panel1.Height - 4)
                        {
                            Draw(stor.arr[j], Color.White);
                            stor.arr[j].y++;
                            Draw(stor.arr[j], Color.Red);
                        }
                    }
                    else
                    {
                        int able = 1;
                        for (int i = 0; i < stor.arr[j].arr.Count; i++)
                        {
                            if (stor.arr[j].arr[i].figure == 3)
                            {
                                if ((stor.readyLine == -1) && !((stor.arr[j].arr[i].lineY1 < panel1.Height - 4) && (stor.arr[j].arr[i].lineY2 < panel1.Height - 4)))
                                    able = 0;
                            }
                            else if (!(stor.arr[j].arr[i].y + (stor.arr[j].arr[i].r / 2) < panel1.Height - 4))
                                able = 0;
                        }

                        if (able == 1)
                        {
                            DrawGroup(stor.arr[j], Color.White);
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                if (stor.arr[j].arr[i].figure == 3)
                                {
                                    stor.arr[j].arr[i].lineY1++;
                                    stor.arr[j].arr[i].lineY2++;
                                }
                                else
                                    stor.arr[j].arr[i].y++;
                            }
                            DrawGroup(stor.arr[j], Color.Red);
                        }
                    }
                }
            }
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == false)
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], stor.arr[j].clr);
                    else
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == true)
                {
                    if (stor.arr[j].IfGroup() == 0)
                    {
                        if ((stor.arr[j].figure == 5) && (stor.arr[j].x - (stor.arr[j].r / 2) > 0))
                        {
                            Draw(stor.arr[j], Color.White);
                            stor.arr[j].x--;
                            Draw(stor.arr[j], Color.Red);
                            GlueTo(stor.arr[j].x, stor.arr[j].y, stor.arr[j].r, stor.arr[j]);
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                MoveGlued(stor.arr[j].arr[i], -1, 0);
                            }
                        }
                        if (stor.arr[j].figure == 3)
                        {
                            if ((stor.readyLine == -1) && (stor.arr[j].lineX1 > 0) && (stor.arr[j].lineX2 > 0))
                            {
                                Draw(stor.arr[j], Color.White);
                                stor.arr[j].lineX2--;
                                stor.arr[j].lineX1--;
                                Draw(stor.arr[j], Color.Red);
                            }
                        }
                        else if (stor.arr[j].x - (stor.arr[j].r / 2) > 0)
                        {
                            Draw(stor.arr[j], Color.White);
                            stor.arr[j].x--;
                            Draw(stor.arr[j], Color.Red);
                        }
                    }
                    else
                    {
                        int able = 1;
                        for (int i = 0; i < stor.arr[j].arr.Count; i++)
                        {
                            if (stor.arr[j].arr[i].figure == 3)
                            {
                                if ((stor.readyLine == -1) && !((stor.arr[j].arr[i].lineX1 > 0) && (stor.arr[j].arr[i].lineX2 > 0)))
                                    able = 0;
                            }
                            else if (!(stor.arr[j].arr[i].x - (stor.arr[j].arr[i].r / 2) > 0))
                                able = 0;
                        }

                        if (able == 1)
                        {
                            DrawGroup(stor.arr[j], Color.White);
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                if (stor.arr[j].arr[i].figure == 3)
                                {
                                    stor.arr[j].arr[i].lineX1--;
                                    stor.arr[j].arr[i].lineX2--;
                                }
                                else
                                    stor.arr[j].arr[i].x--;
                            }
                            DrawGroup(stor.arr[j], Color.Red);
                        }
                    }
                }
            }
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == false)
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], stor.arr[j].clr);
                    else
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
                }
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == true)
                {
                    if (stor.arr[j].IfGroup() == 0)
                    {
                        if ((stor.arr[j].figure == 5) && (stor.arr[j].x + (stor.arr[j].r / 2) < panel1.Height - 44))
                        {
                            Draw(stor.arr[j], Color.White);
                            stor.arr[j].x++;
                            Draw(stor.arr[j], Color.Red);
                            GlueTo(stor.arr[j].x, stor.arr[j].y, stor.arr[j].r, stor.arr[j]);
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                MoveGlued(stor.arr[j].arr[i], 1, 0);
                            }
                        }
                        if (stor.arr[j].figure == 3)
                        {
                            if ((stor.readyLine == -1) && (stor.arr[j].lineX1 < panel1.Height - 44) && (stor.arr[j].lineX2 < panel1.Height - 44))
                            {
                                Draw(stor.arr[j], Color.White);
                                stor.arr[j].lineX2++;
                                stor.arr[j].lineX1++;
                                Draw(stor.arr[j], Color.Red);
                            }
                        }
                        else if (stor.arr[j].x + (stor.arr[j].r / 2) < panel1.Height - 44)
                        {
                            Draw(stor.arr[j], Color.White);
                            stor.arr[j].x++;
                            Draw(stor.arr[j], Color.Red);
                        }
                    }
                    else
                    {
                        int able = 1;
                        for (int i = 0; i < stor.arr[j].arr.Count; i++)
                        {
                            if (stor.arr[j].arr[i].figure == 3)
                            {
                                if ((stor.readyLine == -1) && !((stor.arr[j].arr[i].lineX1 < panel1.Height - 44) && (stor.arr[j].arr[i].lineX2 < panel1.Height - 44)))
                                    able = 0;
                            }
                            else if (!(stor.arr[j].arr[i].x + (stor.arr[j].arr[i].r / 2) < panel1.Height - 44))
                                able = 0;
                        }

                        if (able == 1)
                        {
                            DrawGroup(stor.arr[j], Color.White);
                            for (int i = 0; i < stor.arr[j].arr.Count; i++)
                            {
                                if (stor.arr[j].arr[i].figure == 3)
                                {
                                    stor.arr[j].arr[i].lineX1++;
                                    stor.arr[j].arr[i].lineX2++;
                                }
                                else
                                    stor.arr[j].arr[i].x++;
                            }
                            DrawGroup(stor.arr[j], Color.Red);
                        }
                    }
                }
            }
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == false)
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], stor.arr[j].clr);
                    else
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
                }
            }
        }

        unsafe public TreeNode treeGroup(CCircle circ, int j, int *count)
        {
            TreeNode trNd = new TreeNode();
            if (circ.IfGroup() == 0)
            {
                trNd.Text = Convert.ToString(j);
                if (circ.flag)
                    treeView1.SelectedNode = trNd;
                return trNd;
            }
            else
            {
                j = *count;
                trNd.Text = "Группа " + Convert.ToString(j);
                for (int i = 0; i < circ.arr.Count; i++)
                {
                    trNd.Nodes.Add(treeGroup(circ.arr[i], 1 + j + i, count));
                }
                *count += circ.arr.Count + 1;

                if (circ.flag)
                    treeView1.SelectedNode = trNd;
                return trNd;
            }
        }

        unsafe public void updateTree()
        {
            for (int i = 0; i < stor.arr.Count; i++)
            {
                treeView1.Nodes[0].Nodes.Clear();
            }
            int count = 0;
            for (int i = 0; i < stor.arr.Count; i++)
            {
                treeView1.Nodes[0].Nodes.Add(treeGroup(stor.arr[i], i, &count));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag)
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], Color.Red);
                    else
                        DrawGroup(stor.arr[j], Color.Red);
                }
                else
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], stor.arr[j].clr);
                    else
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
                }
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            GroupedFigures group = new GroupedFigures();
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag)
                {
                    group.AddGroup(stor.arr[j]);
                    stor.DelStor(stor.arr[j]);
                    j--;
                }
            }
            stor.AddStor(group);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag == true)
                {
                    if (stor.arr[j].IfGroup() == 1)
                    {
                        for (int i = 0; i < stor.arr[j].arr.Count; i++)
                        {
                            stor.arr.Add(stor.arr[j].arr[i]);
                            stor.arr[stor.arr.Count() - 1].flag = false;
                        }
                        stor.DelStor(stor.arr[j]);
                        j--;
                    }
                }
            }
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            stor.SaveStor();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            for(int i = 0; i <stor.arr.Count; i++)
            {
                stor.arr[i].flag = true;
            }
            Button1_Click(sender, e);
            stor.LoadStor();
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag)
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], Color.Red);
                    else
                        DrawGroup(stor.arr[j], Color.Red);
                }
                else
                {
                    if (stor.arr[j].IfGroup() == 0)
                        Draw(stor.arr[j], stor.arr[j].clr);
                    else
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
                }
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int selInd = treeView1.SelectedNode.Index;
            if (ModifierKeys.HasFlag(Keys.Control) != true)
            {
                for (int j = 0; j < stor.arr.Count; j++)
                {
                    stor.arr[j].flag = false;
                }
            }
            stor.arr[selInd].flag = true;
            for (int j = 0; j < stor.arr.Count; j++)
            {
                if (stor.arr[j].flag)
                {
                    if (stor.arr[j].IfGroup() == 1)
                        DrawGroup(stor.arr[j], Color.Red);
                    else
                        Draw(stor.arr[j], Color.Red);
                }
                else
                {
                    if (stor.arr[j].IfGroup() == 1)
                        DrawGroup(stor.arr[j], stor.arr[j].clr);
                    else
                        Draw(stor.arr[j], stor.arr[j].clr);
                }
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                int selInd = treeView1.SelectedNode.Index;
                if (ModifierKeys.HasFlag(Keys.Control) != true)
                {
                    for (int j = 0; j < stor.arr.Count; j++)
                    {
                        stor.arr[j].flag = false;
                    }
                }
                stor.arr[selInd].flag = true;
                for (int j = 0; j < stor.arr.Count; j++)
                {
                    if (stor.arr[j].flag)
                    {
                        if (stor.arr[j].IfGroup() == 1)
                            DrawGroup(stor.arr[j], Color.Red);
                        else
                            Draw(stor.arr[j], Color.Red);
                    }
                    else
                    {
                        if (stor.arr[j].IfGroup() == 1)
                            DrawGroup(stor.arr[j], stor.arr[j].clr);
                        else
                            Draw(stor.arr[j], stor.arr[j].clr);
                    }
                }
            }
        }
    }
}
