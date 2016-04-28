using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace SteganoWave
{
	/// <summary>Records an audio wave</summary>
	public class frmRecorder : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblSamplesRecorded;
		private System.Windows.Forms.Label lblSamplesRequired;
		
		/// <summary>How many samples do we need to hide the message using the specified key?</summary>
		private long countSamplesRequired = 0;
		/// <summary>How man samples do we have recorded yet?</summary>
		private long countSamplesRecorded = 0;
		/// <summary>Empty stream to receive the recorded samples</summary>
		private MemoryStream recordedData = new MemoryStream();
		/// <summary>The recorder will do the WaveIn work</summary>
		private WaveInRecorder waveRecorder;
		/// <summary>Format of the new wave: 16 bit, stereo</summary>
		private WaveFormat format = new WaveFormat(11025, BytesPerSample*8, 2);

		/// <summary>Again: 16 bit</summary>
		private const int BytesPerSample = 2;

		/// <summary>Header + recorded samples</summary>
		private Stream recordedStream;
		public Stream RecordedStream{
			get{ return recordedStream; }
		}

		/// <summary>Initialize a new recorder Form to record [countSamplesRequired] or more samples</summary>
		/// <param name="countSamplesRequired">Minimum count of samples to record</param>
		public frmRecorder(long countSamplesRequired){
			InitializeComponent();
			this.countSamplesRequired = countSamplesRequired;

			lblSamplesRequired.Text = countSamplesRequired.ToString();
			lblSamplesRecorded.Text = countSamplesRecorded.ToString();
		}

		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStartStop = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lblSamplesRecorded = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblSamplesRequired = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnStartStop
			// 
			this.btnStartStop.Location = new System.Drawing.Point(16, 16);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(75, 48);
			this.btnStartStop.TabIndex = 0;
			this.btnStartStop.Text = "Start";
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(104, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Samples Recorded:";
			// 
			// lblSamplesRecorded
			// 
			this.lblSamplesRecorded.Location = new System.Drawing.Point(232, 24);
			this.lblSamplesRecorded.Name = "lblSamplesRecorded";
			this.lblSamplesRecorded.Size = new System.Drawing.Size(88, 23);
			this.lblSamplesRecorded.TabIndex = 2;
			this.lblSamplesRecorded.Text = "00000000";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(104, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Samples Required:";
			// 
			// lblSamplesRequired
			// 
			this.lblSamplesRequired.Location = new System.Drawing.Point(232, 48);
			this.lblSamplesRequired.Name = "lblSamplesRequired";
			this.lblSamplesRequired.Size = new System.Drawing.Size(88, 23);
			this.lblSamplesRequired.TabIndex = 2;
			this.lblSamplesRequired.Text = "00000000";
			// 
			// frmRecorder
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(336, 87);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lblSamplesRecorded,
																		  this.label1,
																		  this.btnStartStop,
																		  this.label2,
																		  this.lblSamplesRequired});
			this.Name = "frmRecorder";
			this.Text = "Record a Sound";
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>Start recording</summary>
		private void Start(){
			waveRecorder = new WaveInRecorder(-1, format, 16384, 3, new BufferDoneEventHandler(WaveDataArrived));
			btnStartStop.Enabled = false;
			btnStartStop.Text = "Stop";
		}

		/// <summary>Stop recording, add a header to the sound data</summary>
		private void Stop(){
			waveRecorder.Dispose();
			recordedStream = WaveStream.CreateStream(recordedData, format);
			Close();
		}
		
		/// <summary>Callback method - copy a buffer of recorded audio data</summary>
		/// <param name="data">Pointer to the raw data</param>
		/// <param name="size">Size of the data</param>
		private void WaveDataArrived(IntPtr data, int size) {
			byte[] recBuffer = new byte[size];
			System.Runtime.InteropServices.Marshal.Copy(data, recBuffer, 0, size);
			recordedData.Write(recBuffer, 0, recBuffer.Length);

			countSamplesRecorded += size/BytesPerSample;
			MethodInvoker invoker = new MethodInvoker(SetSamplesRecordedLabelText);
			lblSamplesRecorded.Invoke(invoker);

			if(countSamplesRecorded >= countSamplesRequired){
				//enough samples arrived - allow the user to stop recording
				invoker = new MethodInvoker(SetStopButtonEnabled);
				btnStartStop.Invoke(invoker);
			}
		}

		private void SetSamplesRecordedLabelText()
		{
			lblSamplesRecorded.Text = countSamplesRecorded.ToString();
		}

		private void SetStopButtonEnabled()
		{
			btnStartStop.Enabled = true;
		}

		private void btnStartStop_Click(object sender, System.EventArgs e) {
			if(btnStartStop.Text == "Start"){
				Start();
			}else{
				Stop();
			}
		}
	}
}
