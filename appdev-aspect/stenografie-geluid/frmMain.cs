using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace SteganoWave
{
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtSrcFile;
		private System.Windows.Forms.Button btnSrcFile;
		private System.Windows.Forms.Button btnDstFile;
		private System.Windows.Forms.TextBox txtDstFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnHide;
		private System.Windows.Forms.TabControl tabCtl;
		private System.Windows.Forms.TabPage tabHide;
		private System.Windows.Forms.TabPage tabExtract;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Button btnExtract;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnKeyFile;
		private System.Windows.Forms.TextBox txtKeyFile;
		private System.Windows.Forms.TextBox txtExtractedMessage;
		private System.Windows.Forms.RadioButton rdoSrcFile;
		private System.Windows.Forms.RadioButton rdoRecord;
		private System.Windows.Forms.RadioButton rdoMsgFile;
		private System.Windows.Forms.TextBox txtMsgFile;
		private System.Windows.Forms.RadioButton rdoMsgText;
		private System.Windows.Forms.RadioButton rdoMessageDisplay;
		private System.Windows.Forms.RadioButton rdoMessageDstFile;
		private System.Windows.Forms.TextBox txtMessageDstFile;
		private System.Windows.Forms.Button btnMsgFile1;
		private System.Windows.Forms.Button btnMessageDstFile;
		private System.Windows.Forms.ErrorProvider errorProvider; 
		private System.ComponentModel.Container components = null;

		public frmMain(){
			InitializeComponent();
		}

		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtSrcFile = new System.Windows.Forms.TextBox();
			this.btnSrcFile = new System.Windows.Forms.Button();
			this.btnDstFile = new System.Windows.Forms.Button();
			this.txtDstFile = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnHide = new System.Windows.Forms.Button();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.tabCtl = new System.Windows.Forms.TabControl();
			this.tabHide = new System.Windows.Forms.TabPage();
			this.rdoMsgText = new System.Windows.Forms.RadioButton();
			this.btnMsgFile1 = new System.Windows.Forms.Button();
			this.txtMsgFile = new System.Windows.Forms.TextBox();
			this.rdoMsgFile = new System.Windows.Forms.RadioButton();
			this.tabExtract = new System.Windows.Forms.TabPage();
			this.btnMessageDstFile = new System.Windows.Forms.Button();
			this.txtMessageDstFile = new System.Windows.Forms.TextBox();
			this.rdoMessageDisplay = new System.Windows.Forms.RadioButton();
			this.rdoMessageDstFile = new System.Windows.Forms.RadioButton();
			this.txtExtractedMessage = new System.Windows.Forms.TextBox();
			this.btnExtract = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.btnKeyFile = new System.Windows.Forms.Button();
			this.txtKeyFile = new System.Windows.Forms.TextBox();
			this.rdoSrcFile = new System.Windows.Forms.RadioButton();
			this.rdoRecord = new System.Windows.Forms.RadioButton();
			this.errorProvider = new System.Windows.Forms.ErrorProvider();
			this.tabCtl.SuspendLayout();
			this.tabHide.SuspendLayout();
			this.tabExtract.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtSrcFile
			// 
			this.errorProvider.SetIconAlignment(this.txtSrcFile, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtSrcFile.Location = new System.Drawing.Point(145, 21);
			this.txtSrcFile.Name = "txtSrcFile";
			this.txtSrcFile.Size = new System.Drawing.Size(382, 22);
			this.txtSrcFile.TabIndex = 0;
			this.txtSrcFile.Text = "";
			this.txtSrcFile.TextChanged += new System.EventHandler(this.textField_TextChanged);
			// 
			// btnSrcFile
			// 
			this.btnSrcFile.Location = new System.Drawing.Point(529, 21);
			this.btnSrcFile.Name = "btnSrcFile";
			this.btnSrcFile.Size = new System.Drawing.Size(102, 28);
			this.btnSrcFile.TabIndex = 1;
			this.btnSrcFile.Text = "Browse...";
			this.btnSrcFile.Click += new System.EventHandler(this.btnSrcFile_Click);
			// 
			// btnDstFile
			// 
			this.btnDstFile.Location = new System.Drawing.Point(594, 21);
			this.btnDstFile.Name = "btnDstFile";
			this.btnDstFile.Size = new System.Drawing.Size(102, 28);
			this.btnDstFile.TabIndex = 1;
			this.btnDstFile.Text = "Browse...";
			this.btnDstFile.Click += new System.EventHandler(this.btnDstFile_Click);
			// 
			// txtDstFile
			// 
			this.errorProvider.SetIconAlignment(this.txtDstFile, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtDstFile.Location = new System.Drawing.Point(195, 21);
			this.txtDstFile.Name = "txtDstFile";
			this.txtDstFile.Size = new System.Drawing.Size(382, 22);
			this.txtDstFile.TabIndex = 0;
			this.txtDstFile.Text = "";
			this.txtDstFile.TextChanged += new System.EventHandler(this.textField_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(51, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(123, 31);
			this.label2.TabIndex = 2;
			this.label2.Text = "Save Result as";
			// 
			// btnHide
			// 
			this.btnHide.Location = new System.Drawing.Point(297, 237);
			this.btnHide.Name = "btnHide";
			this.btnHide.Size = new System.Drawing.Size(164, 30);
			this.btnHide.TabIndex = 3;
			this.btnHide.Text = "Hide Message";
			this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
			// 
			// txtMessage
			// 
			this.errorProvider.SetIconAlignment(this.txtMessage, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtMessage.Location = new System.Drawing.Point(195, 99);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(378, 128);
			this.txtMessage.TabIndex = 4;
			this.txtMessage.Text = "";
			this.txtMessage.TextChanged += new System.EventHandler(this.textField_TextChanged);
			this.txtMessage.Enter += new System.EventHandler(this.txtMessage_Enter);
			// 
			// tabCtl
			// 
			this.tabCtl.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.tabHide,
																				 this.tabExtract});
			this.tabCtl.Location = new System.Drawing.Point(10, 172);
			this.tabCtl.Name = "tabCtl";
			this.tabCtl.SelectedIndex = 0;
			this.tabCtl.Size = new System.Drawing.Size(727, 315);
			this.tabCtl.TabIndex = 5;
			this.tabCtl.SelectedIndexChanged += new System.EventHandler(this.tabCtl_SelectedIndexChanged);
			// 
			// tabHide
			// 
			this.tabHide.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.rdoMsgText,
																				  this.btnMsgFile1,
																				  this.txtMsgFile,
																				  this.rdoMsgFile,
																				  this.txtMessage,
																				  this.btnHide,
																				  this.label2,
																				  this.btnDstFile,
																				  this.txtDstFile});
			this.tabHide.Location = new System.Drawing.Point(4, 25);
			this.tabHide.Name = "tabHide";
			this.tabHide.Size = new System.Drawing.Size(719, 286);
			this.tabHide.TabIndex = 0;
			this.tabHide.Text = "Hide";
			// 
			// rdoMsgText
			// 
			this.rdoMsgText.Checked = true;
			this.rdoMsgText.Location = new System.Drawing.Point(31, 99);
			this.rdoMsgText.Name = "rdoMsgText";
			this.rdoMsgText.Size = new System.Drawing.Size(123, 29);
			this.rdoMsgText.TabIndex = 8;
			this.rdoMsgText.TabStop = true;
			this.rdoMsgText.Text = "Hide Text";
			this.rdoMsgText.CheckedChanged += new System.EventHandler(this.rdoContent_CheckedChanged);
			// 
			// btnMsgFile1
			// 
			this.btnMsgFile1.Location = new System.Drawing.Point(594, 59);
			this.btnMsgFile1.Name = "btnMsgFile1";
			this.btnMsgFile1.Size = new System.Drawing.Size(102, 30);
			this.btnMsgFile1.TabIndex = 7;
			this.btnMsgFile1.Text = "Browse...";
			this.btnMsgFile1.Click += new System.EventHandler(this.btnMsgFile_Click);
			// 
			// txtMsgFile
			// 
			this.errorProvider.SetIconAlignment(this.txtMsgFile, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtMsgFile.Location = new System.Drawing.Point(195, 59);
			this.txtMsgFile.Name = "txtMsgFile";
			this.txtMsgFile.Size = new System.Drawing.Size(382, 22);
			this.txtMsgFile.TabIndex = 6;
			this.txtMsgFile.Text = "";
			this.txtMsgFile.TextChanged += new System.EventHandler(this.textField_TextChanged);
			this.txtMsgFile.Enter += new System.EventHandler(this.txtMsgFile_Enter);
			// 
			// rdoMsgFile
			// 
			this.rdoMsgFile.Location = new System.Drawing.Point(31, 59);
			this.rdoMsgFile.Name = "rdoMsgFile";
			this.rdoMsgFile.Size = new System.Drawing.Size(174, 30);
			this.rdoMsgFile.TabIndex = 5;
			this.rdoMsgFile.Text = "Hide Content of File";
			this.rdoMsgFile.CheckedChanged += new System.EventHandler(this.rdoContent_CheckedChanged);
			// 
			// tabExtract
			// 
			this.tabExtract.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.btnMessageDstFile,
																					 this.txtMessageDstFile,
																					 this.rdoMessageDisplay,
																					 this.rdoMessageDstFile,
																					 this.txtExtractedMessage,
																					 this.btnExtract});
			this.tabExtract.Location = new System.Drawing.Point(4, 25);
			this.tabExtract.Name = "tabExtract";
			this.tabExtract.Size = new System.Drawing.Size(719, 286);
			this.tabExtract.TabIndex = 1;
			this.tabExtract.Text = "Extract";
			// 
			// btnMessageDstFile
			// 
			this.btnMessageDstFile.Location = new System.Drawing.Point(573, 20);
			this.btnMessageDstFile.Name = "btnMessageDstFile";
			this.btnMessageDstFile.Size = new System.Drawing.Size(103, 28);
			this.btnMessageDstFile.TabIndex = 12;
			this.btnMessageDstFile.Text = "Browse...";
			this.btnMessageDstFile.Click += new System.EventHandler(this.btnMessageDstFile_Click);
			// 
			// txtMessageDstFile
			// 
			this.errorProvider.SetIconAlignment(this.txtMessageDstFile, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtMessageDstFile.Location = new System.Drawing.Point(184, 20);
			this.txtMessageDstFile.Name = "txtMessageDstFile";
			this.txtMessageDstFile.Size = new System.Drawing.Size(383, 22);
			this.txtMessageDstFile.TabIndex = 11;
			this.txtMessageDstFile.Text = "";
			this.txtMessageDstFile.TextChanged += new System.EventHandler(this.textField_TextChanged);
			this.txtMessageDstFile.Enter += new System.EventHandler(this.txtMessageDstFile_Enter);
			// 
			// rdoMessageDisplay
			// 
			this.rdoMessageDisplay.Checked = true;
			this.rdoMessageDisplay.Location = new System.Drawing.Point(31, 59);
			this.rdoMessageDisplay.Name = "rdoMessageDisplay";
			this.rdoMessageDisplay.Size = new System.Drawing.Size(143, 30);
			this.rdoMessageDisplay.TabIndex = 10;
			this.rdoMessageDisplay.TabStop = true;
			this.rdoMessageDisplay.Text = "Display Message";
			// 
			// rdoMessageDstFile
			// 
			this.rdoMessageDstFile.Location = new System.Drawing.Point(31, 16);
			this.rdoMessageDstFile.Name = "rdoMessageDstFile";
			this.rdoMessageDstFile.Size = new System.Drawing.Size(153, 29);
			this.rdoMessageDstFile.TabIndex = 9;
			this.rdoMessageDstFile.Text = "Save Message as";
			// 
			// txtExtractedMessage
			// 
			this.errorProvider.SetIconAlignment(this.txtExtractedMessage, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtExtractedMessage.Location = new System.Drawing.Point(184, 69);
			this.txtExtractedMessage.Multiline = true;
			this.txtExtractedMessage.Name = "txtExtractedMessage";
			this.txtExtractedMessage.Size = new System.Drawing.Size(379, 119);
			this.txtExtractedMessage.TabIndex = 7;
			this.txtExtractedMessage.Text = "";
			this.txtExtractedMessage.TextChanged += new System.EventHandler(this.textField_TextChanged);
			this.txtExtractedMessage.Enter += new System.EventHandler(this.txtExtractedMessage_Enter);
			// 
			// btnExtract
			// 
			this.btnExtract.Location = new System.Drawing.Point(297, 237);
			this.btnExtract.Name = "btnExtract";
			this.btnExtract.Size = new System.Drawing.Size(164, 30);
			this.btnExtract.TabIndex = 6;
			this.btnExtract.Text = "Extract Message";
			this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(68, 112);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(77, 31);
			this.label5.TabIndex = 2;
			this.label5.Text = "Key File";
			// 
			// btnKeyFile
			// 
			this.btnKeyFile.Location = new System.Drawing.Point(529, 112);
			this.btnKeyFile.Name = "btnKeyFile";
			this.btnKeyFile.Size = new System.Drawing.Size(102, 27);
			this.btnKeyFile.TabIndex = 1;
			this.btnKeyFile.Text = "Browse...";
			this.btnKeyFile.Click += new System.EventHandler(this.btnKeyFile_Click);
			// 
			// txtKeyFile
			// 
			this.errorProvider.SetIconAlignment(this.txtKeyFile, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtKeyFile.Location = new System.Drawing.Point(145, 112);
			this.txtKeyFile.Name = "txtKeyFile";
			this.txtKeyFile.Size = new System.Drawing.Size(382, 22);
			this.txtKeyFile.TabIndex = 0;
			this.txtKeyFile.Text = "";
			this.txtKeyFile.TextChanged += new System.EventHandler(this.textField_TextChanged);
			// 
			// rdoSrcFile
			// 
			this.rdoSrcFile.Location = new System.Drawing.Point(51, 16);
			this.rdoSrcFile.Name = "rdoSrcFile";
			this.rdoSrcFile.Size = new System.Drawing.Size(94, 26);
			this.rdoSrcFile.TabIndex = 6;
			this.rdoSrcFile.Text = "Wave File";
			this.rdoSrcFile.CheckedChanged += new System.EventHandler(this.rdoCarrierWave_CheckedChanged);
			// 
			// rdoRecord
			// 
			this.rdoRecord.Checked = true;
			this.rdoRecord.Location = new System.Drawing.Point(51, 60);
			this.rdoRecord.Name = "rdoRecord";
			this.rdoRecord.Size = new System.Drawing.Size(222, 25);
			this.rdoRecord.TabIndex = 6;
			this.rdoRecord.TabStop = true;
			this.rdoRecord.Text = "Record a Wave on the fly";
			this.rdoRecord.CheckedChanged += new System.EventHandler(this.rdoCarrierWave_CheckedChanged);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(752, 503);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.rdoSrcFile,
																		  this.tabCtl,
																		  this.btnSrcFile,
																		  this.txtSrcFile,
																		  this.txtKeyFile,
																		  this.label5,
																		  this.btnKeyFile,
																		  this.rdoRecord});
			this.Name = "frmMain";
			this.Text = "SteganoWave";
			this.tabCtl.ResumeLayout(false);
			this.tabHide.ResumeLayout(false);
			this.tabExtract.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void btnSrcFile_Click(object sender, System.EventArgs e) {
			//select the clean carrier file
			OpenFileDialog dlg = new OpenFileDialog();
			GetFileName(dlg, txtSrcFile, true);
		}

		private void btnDstFile_Click(object sender, System.EventArgs e) {
			//select a filename to save the carrier file
			SaveFileDialog dlg = new SaveFileDialog();
			GetFileName(dlg, txtDstFile, true);
		}

		private void btnKeyFile_Click(object sender, System.EventArgs e) {
			//select a key
			OpenFileDialog dlg = new OpenFileDialog();
			GetFileName(dlg, txtKeyFile, false);
		}

		
		private void btnMsgFile_Click(object sender, System.EventArgs e) {
			//select a secret message
			OpenFileDialog dlg = new OpenFileDialog();
			GetFileName(dlg, txtMsgFile, false);
			rdoMsgFile.Checked = true;
		}
		
		private void btnMessageDstFile_Click(object sender, System.EventArgs e) {
			//select a filename to save the extracted message
			SaveFileDialog dlg = new SaveFileDialog();
			GetFileName(dlg, txtMessageDstFile, false);
			rdoMessageDstFile.Checked = true;
		}

		private void btnHide_Click(object sender, System.EventArgs e) {
			//hide the message inside the carrier wave


			if(rdoSrcFile.Checked && (txtSrcFile.Text.Length == 0)){
				errorProvider.SetError(txtSrcFile, "You forgot to choose a carrier file.");
			}
			else if(txtKeyFile.Text.Length == 0){
				errorProvider.SetError(txtKeyFile, "You forgot to choose a key file.");
			}
			else if(txtDstFile.Text.Length == 0){
				errorProvider.SetError(txtDstFile, "The resulting carrier file must be saved somewhere.");
			}
			else if(rdoMsgFile.Checked && (txtMsgFile.Text.Length == 0)){
				errorProvider.SetError(txtMsgFile, "What am I supposed to hide?");
			}
			else if(rdoMsgText.Checked && (txtMessage.Text.Length == 0)){
				errorProvider.SetError(txtMessage, "What am I supposed to hide?");
			}
			else{

				Stream sourceStream = null;
				FileStream destinationStream = null;
				WaveStream audioStream = null;
			
				//create a stream that contains the message, preceeded by its length
				Stream messageStream = GetMessageStream();
				//open the key file
				Stream keyStream = new FileStream(txtKeyFile.Text, FileMode.Open);
			
				try {
				
					//how man samples do we need?
					long countSamplesRequired = WaveUtility.CheckKeyForMessage(keyStream, messageStream.Length);
				
					if(countSamplesRequired > Int32.MaxValue){
						throw new Exception("Message too long, or bad key! This message/key combination requires "+countSamplesRequired+" samples, only "+Int32.MaxValue+" samples are allowed.");
					}

					if(rdoSrcFile.Checked){ //use a .wav file as the carrier
						sourceStream = new FileStream(txtSrcFile.Text, FileMode.Open);
					}else{ //record a carrier wave
						frmRecorder recorder = new frmRecorder(countSamplesRequired);
						recorder.ShowDialog(this);
						sourceStream = recorder.RecordedStream;
					}

					this.Cursor = Cursors.WaitCursor;
				
					//create an empty file for the carrier wave
					destinationStream = new FileStream(txtDstFile.Text, FileMode.Create);
				
					//copy the carrier file's header
					audioStream = new WaveStream(sourceStream, destinationStream);
					if (audioStream.Length <= 0){
						throw new Exception("Invalid WAV file");
					}
				
					//are there enough samples in the carrier wave?
					if(countSamplesRequired > audioStream.CountSamples){
						String errorReport = "The carrier file is too small for this message and key!\r\n"
							+ "Samples available: " + audioStream.CountSamples + "\r\n"			
							+ "Samples needed: " + countSamplesRequired;
						throw new Exception(errorReport);
					}

					//hide the message
					WaveUtility utility = new WaveUtility(audioStream, destinationStream);
					utility.Hide(messageStream, keyStream);
				}
				catch(Exception ex) {
					this.Cursor = Cursors.Default;
					MessageBox.Show(ex.Message);
				}
				finally{
					if(keyStream != null){ keyStream.Close(); }
					if(messageStream != null){ messageStream.Close(); }
					if(audioStream != null){ audioStream.Close(); }
					if(sourceStream != null){ sourceStream.Close(); }
					if(destinationStream != null){ destinationStream.Close(); }
					this.Cursor = Cursors.Default;
				}
			}
		}

		private void btnExtract_Click(object sender, System.EventArgs e) {
			//extract the message from the carrier wave
			
			if(txtSrcFile.Text.Length == 0){
				errorProvider.SetError(txtSrcFile, "You forgot to choose a carrier file.");
			}
			else if(txtKeyFile.Text.Length == 0){
				errorProvider.SetError(txtKeyFile, "You forgot to choose a key file.");
			}
			else{

				this.Cursor = Cursors.WaitCursor;
			
				FileStream sourceStream = null;
				WaveStream audioStream = null;
				//create an empty stream to receive the extracted message
				MemoryStream messageStream = new MemoryStream();
				//open the key file
				Stream keyStream = new FileStream(txtKeyFile.Text, FileMode.Open);
			
				try {
					//open the carrier file
					sourceStream = new FileStream(txtSrcFile.Text, FileMode.Open);
					audioStream = new WaveStream(sourceStream);
					WaveUtility utility = new WaveUtility(audioStream);
			
					//exctract the message from the carrier wave
					utility.Extract(messageStream, keyStream);
				
					messageStream.Seek(0, SeekOrigin.Begin);
					if(rdoMessageDstFile.Checked){ //save result to a file
						FileStream fs = new FileStream(txtMessageDstFile.Text, FileMode.Create);
						
						byte[] buffer = new byte[messageStream.Length];
						messageStream.Read(buffer, 0, buffer.Length);
						fs.Write(buffer, 0, buffer.Length);
						fs.Close();
					}else{ //display result
						txtExtractedMessage.Text = new StreamReader(messageStream).ReadToEnd();					
					}
				}
				catch(Exception ex) {
					this.Cursor = Cursors.Default;
					MessageBox.Show(ex.Message);
				}
				finally{
					if(keyStream != null){ keyStream.Close(); }
					if(messageStream != null){ messageStream.Close(); }
					if(audioStream != null){ audioStream.Close(); }
					if(sourceStream != null){ sourceStream.Close(); }
					this.Cursor = Cursors.Default;
				}
			}
		}

		private void tabCtl_SelectedIndexChanged(object sender, System.EventArgs e) {
			if(tabCtl.SelectedIndex==0){	//"hide" tab selected
				rdoRecord.Enabled = true;
			}else{							//"extract" tab selected
				rdoSrcFile.Checked = true;
				rdoRecord.Enabled = false;				
			}
		}

		/// <summary>Open a FileDialog and write the selected filename into a TextBox</summary>
		/// <param name="dialog">The OPen/Save-FileDialog</param>
		/// <param name="control">The corresponding TextBox</param>
		/// <param name="useFilter">Allow only .wav files</param>
		private void GetFileName(FileDialog dialog, TextBox control, bool useFilter){
			if(useFilter){ dialog.Filter = "Wave Audio (*.wav)|*.wav"; }
			if( dialog.ShowDialog(this) == DialogResult.OK){
				control.Text = dialog.FileName;
			}
		}

		/// <summary>Write length an content of the message file/text into a stream</summary>
		/// <returns></returns>
		private Stream GetMessageStream(){
			BinaryWriter messageWriter = new BinaryWriter(new MemoryStream());
			if(rdoMsgFile.Checked){
				FileStream fs = new FileStream(txtMsgFile.Text, FileMode.Open);
				int fileLength = (int)fs.Length;
				messageWriter.Write(fileLength);
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, fileLength);
				messageWriter.Write(buffer);
				fs.Close();
			}else{
				messageWriter.Write(txtMessage.Text.Length);
				messageWriter.Write(Encoding.ASCII.GetBytes(txtMessage.Text));
			}
			messageWriter.Seek(0, SeekOrigin.Begin);
			return messageWriter.BaseStream;			
		}

		private void rdoCarrierWave_CheckedChanged(object sender, System.EventArgs e) {
			txtSrcFile.Enabled = btnSrcFile.Enabled = rdoSrcFile.Checked;
		}

		private void rdoContent_CheckedChanged(object sender, System.EventArgs e) {
			txtMsgFile.Enabled = btnMsgFile1.Enabled = rdoMsgFile.Checked;
			txtMessage.Enabled = rdoMsgText.Checked;
		}

		private void textField_TextChanged(object sender, System.EventArgs e) {
			errorProvider.SetError((Control)sender, String.Empty);
		}

		private void txtMsgFile_Enter(object sender, System.EventArgs e) {
			rdoMsgFile.Checked = true;
		}

		private void txtMessage_Enter(object sender, System.EventArgs e) {
			rdoMsgText.Checked = true;
		}

		private void txtMessageDstFile_Enter(object sender, System.EventArgs e) {
			rdoMessageDstFile.Checked = true;
		}

		private void txtExtractedMessage_Enter(object sender, System.EventArgs e) {
			rdoMessageDisplay.Checked = true;
		}

	}
}
