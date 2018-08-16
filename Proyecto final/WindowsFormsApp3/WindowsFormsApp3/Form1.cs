using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //
        //Creacion de Barras de colores
        //
        static int CNT = 4;
        public static List<TrackBar> tbs = new List<TrackBar>();
        Color[] clr = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow };
        string[] lb = new string[] { "Rojo", "Verde", "Azul", "Amarillo" };
        Image img2, img_org;

        private static Image Llenar_Leyer(PictureBox PicBox, Image img, Color[] colr)
        {
            
            PicBox.Image = img;
            Bitmap bmp_img = new Bitmap(PicBox.Image);
            Graphics ghp = Graphics.FromImage(bmp_img);
            LinearGradientBrush LineaBrush;
            for (int i = 0; i < CNT; i++)
            {
                LineaBrush = new LinearGradientBrush(new Rectangle(0, 0, bmp_img.Width, bmp_img.Height), Color.FromArgb(tbs[i].Value, colr[i]), Color.FromArgb(tbs[i].Value, colr[i]), LinearGradientMode.BackwardDiagonal);
                ghp.FillRectangle(LineaBrush, new Rectangle(0, 0, bmp_img.Width, bmp_img.Height));
            }
            return (Image)bmp_img;
        }

        private void Imagen_PredeterminadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < CNT; i++)
            {
                tbs[i].Value = 0;
            }
            pictureBox1.Image = img2 = img_org;
        }

        private void GuardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Guardar fotos//
            saveFileDialog1.Filter = "JPEG File (*.jpg)|*.jpg|Bitmap File (*.bmp)|*.bmp|PNG File(*.png)|*.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                switch (Path.GetExtension(saveFileDialog1.FileName))
                {
                    case ".jpg":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Jpeg); break;
                    case ".bmp":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Bmp); break;
                    case ".png":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Png); break;
                }
            }
        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Salir
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            add_tracks();
        }
        void add_tracks()
        {
            //Procedimiento para las barras de colores dar location y size
            for (int i = 0; i < CNT; i++)
            {
                TrackBar tb = new TrackBar();
                tb.Size = new Size(114, 45);
                tb.Location = new Point(i * 114 + 214, 545);
                tb.Enabled = false;
                tb.Minimum = 0;
                tb.Maximum = 255;
                tb.Value = 0;
                tb.TickStyle = TickStyle.None;
                tb.ValueChanged += new EventHandler(tb_ValueChanged);
                tbs.Add(tb);

                Label l = new Label();
                l.Text = lb[i];
                l.Location = new Point(i * 114 + 260, 567);
                this.Controls.Add(l);
            }
            this.Controls.AddRange(tbs.ToArray());
            this.Width += (CNT - 4) * 110;
            label1.Left = (int)((pictureBox1.Width - label1.Width) / 2);
        }

        private void AbrilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Procedimiento Abrir Imagen 
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    label1.Visible = false;
                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    img_org = img2 = pictureBox1.Image;
                    for (int i = 0; i < CNT; i++)
                    {
                        tbs[i].Enabled = true;
                    }
                }
            }
            catch
            {
                pictureBox1.Image = null;
                label1.Visible = true;
                for (int i = 0; i < CNT; i++)
                {
                    tbs[i].Enabled = false;
                }
            }
        }

        private void Estabilización_de_colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Barras de colores vuelven a cero
            img2 = pictureBox1.Image;
            for (int i = 0; i < CNT; i++)
            {
                tbs[i].Value = 0;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        void tb_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < CNT; i++)
            {
                toolTip1.SetToolTip(tbs[i], tbs[i].Value.ToString());
            }
            pictureBox1.Image = Llenar_Leyer(pictureBox1, img2, clr);
        }
    }

}
