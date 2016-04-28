using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Steganography
{
    public partial class Steganography : Form
    {
        #region properties
        private Bitmap bmp = null;
        private string extractedText = string.Empty;
        #endregion

        /// <summary>
        /// TODO: Default constructor
        /// </summary>
        public Steganography()
        {
            InitializeComponent();
        }

        /// <summary>
        /// TODO: Verbergen van data in een afbeelding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hideButton_Click(object sender, EventArgs e)
        {
            string _text = dataTextBox.Text;

            bmp = (Bitmap)imagePictureBox.Image;

            if (_text.Equals(""))
            {
                MessageBox.Show("The text you want to hide can't be empty", "Warning");
                return;
            }

            if (encryptCheckBox.Checked)
            {
                if (passwordTextBox.Text.Length < 6)
                {
                    MessageBox.Show("Please enter a password with at least 6 characters", "Warning");
                    return;
                }
                else
                {
                    _text = Crypto.EncryptStringAES(_text, passwordTextBox.Text);
                }
            }

            bmp = SteganographyHelper.embedText(_text, bmp);

            MessageBox.Show("Your text was hidden in the image successfully!", "Done");
            maakLeeg();

            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Png Image|*.png|Bitmap Image|*.bmp";

            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                switch (save_dialog.FilterIndex)
                {
                    case 0:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Png);
                        } break;
                    case 1:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Bmp);
                        } break;
                }

                
            }


        }

        /// <summary>
        /// TODO: Ophalen van verborgen data uit een afbeelding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void extractButton_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;

            string extractedText = SteganographyHelper.extractText(bmp);

            if (encryptCheckBox.Checked)
            {
                try
                {
                    extractedText = Crypto.DecryptStringAES(extractedText, passwordTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("Wrong password", "Error");

                    return;
                }
            }

            dataTextBox.Text = extractedText;
        }

        /// <summary>
        /// TODO: Het kiezen van een afbeelding waar van gebruik gemaakt word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files (*.jpeg; *.png; *.bmp)|*.jpg; *.png; *.bmp";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                imagePictureBox.Image = Image.FromFile(open_dialog.FileName);
                maakLeeg();
            }
        }

        /// <summary>
        /// TODO: empty fields
        /// </summary>
        private void maakLeeg()
        {
            dataTextBox.Text = "";
            passwordTextBox.Text = "";
            encryptCheckBox.Checked = false;
        }

        /// <summary>
        /// TODO: Close and return to menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitKnop_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}