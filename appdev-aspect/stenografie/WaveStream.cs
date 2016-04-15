//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  This material may not be duplicated in whole or in part, except for 
//  personal use, without the express written consent of the author. 
//
//  Email:  ianier@hotmail.com
//
//  Copyright (C) 1999-2003 Ianier Munoz. All Rights Reserved.

//	The methods commented with <summary> tags have been inserted by
//	Corinna John, EMail: picturekey@binary-universe.net
//	Anybody who knows a little about the wave format could have
//	written these enhancements, so I don't claim any rights,
//	do whatever you want with them.

using System;
using System.IO;

namespace SteganoWave
{
	public class WaveStream : Stream, IDisposable
	{
		private Stream m_Stream;
		private long m_DataPos;
		private int m_Length;

		private WaveFormat m_Format;

		public WaveFormat Format
		{
			get { return m_Format; }
		}

		private string ReadChunk(BinaryReader reader) {
			byte[] ch = new byte[4];
			reader.Read(ch, 0, ch.Length);
			return System.Text.Encoding.ASCII.GetString(ch);
		}

		private void ReadHeader() {
			BinaryReader Reader = new BinaryReader(m_Stream);
			if (ReadChunk(Reader) != "RIFF")
				throw new Exception("Invalid file format");

			Reader.ReadInt32(); // File length minus first 8 bytes of RIFF description, we don't use it

			if (ReadChunk(Reader) != "WAVE")
				throw new Exception("Invalid file format");

			if (ReadChunk(Reader) != "fmt ")
				throw new Exception("Invalid file format");

			int len = Reader.ReadInt32();
			if (len < 16) // bad format chunk length
				throw new Exception("Invalid file format");

			m_Format = new WaveFormat(22050, 16, 2); // initialize to any format
			m_Format.wFormatTag = Reader.ReadInt16();
			m_Format.nChannels = Reader.ReadInt16();
			m_Format.nSamplesPerSec = Reader.ReadInt32();
			m_Format.nAvgBytesPerSec = Reader.ReadInt32();
			m_Format.nBlockAlign = Reader.ReadInt16();
			m_Format.wBitsPerSample = Reader.ReadInt16(); 

			// advance in the stream to skip the wave format block 
			len -= 16; // minimum format size
			while (len > 0) {
				Reader.ReadByte();
				len--;
			}

			// assume the data chunk is aligned
			while(m_Stream.Position < m_Stream.Length && ReadChunk(Reader) != "data")
				;

			if (m_Stream.Position >= m_Stream.Length)
				throw new Exception("Invalid file format");

			m_Length = Reader.ReadInt32();
			m_DataPos = m_Stream.Position;

			Position = 0;
		}

		/// <summary>ReadChunk(reader) - Changed to CopyChunk(reader, writer)</summary>
		/// <param name="reader">source stream</param>
		/// <returns>four characters</returns>
		private string CopyChunk(BinaryReader reader, BinaryWriter writer)
		{
			byte[] ch = new byte[4];
			reader.Read(ch, 0, ch.Length);
			
			//copy the chunk
			writer.Write(ch);
			
			return System.Text.Encoding.ASCII.GetString(ch);
		}

		/// <summary>ReadHeader() - Changed to CopyHeader(destination)</summary>
		private void CopyHeader(Stream destinationStream)
		{
			BinaryReader reader = new BinaryReader(m_Stream);
			BinaryWriter writer = new BinaryWriter(destinationStream);
			
			if (CopyChunk(reader, writer) != "RIFF")
				throw new Exception("Invalid file format");

			writer.Write( reader.ReadInt32() ); // File length minus first 8 bytes of RIFF description
			
			if (CopyChunk(reader, writer) != "WAVE")
				throw new Exception("Invalid file format");

			if (CopyChunk(reader, writer) != "fmt ")
				throw new Exception("Invalid file format");

			int len = reader.ReadInt32();
			if (len < 16){ // bad format chunk length
				throw new Exception("Invalid file format");
			}else{
				writer.Write(len);
			}

			m_Format = new WaveFormat(22050, 16, 2); // initialize to any format
			m_Format.wFormatTag = reader.ReadInt16();
			m_Format.nChannels = reader.ReadInt16();
			m_Format.nSamplesPerSec = reader.ReadInt32();
			m_Format.nAvgBytesPerSec = reader.ReadInt32();
			m_Format.nBlockAlign = reader.ReadInt16();
			m_Format.wBitsPerSample = reader.ReadInt16(); 

			//copy format information
			writer.Write( m_Format.wFormatTag );
			writer.Write( m_Format.nChannels );
			writer.Write( m_Format.nSamplesPerSec );
			writer.Write( m_Format.nAvgBytesPerSec );
			writer.Write( m_Format.nBlockAlign );
			writer.Write( m_Format.wBitsPerSample ); 


			// advance in the stream to skip the wave format block 
			len -= 16; // minimum format size
			writer.Write( reader.ReadBytes(len) );
			len = 0;
			/*while (len > 0)
			{
				reader.ReadByte();
				len--;
			}*/

			// assume the data chunk is aligned
			while(m_Stream.Position < m_Stream.Length && CopyChunk(reader, writer) != "data")
				;

			if (m_Stream.Position >= m_Stream.Length)
				throw new Exception("Invalid file format");

			m_Length = reader.ReadInt32();
			writer.Write( m_Length );
			
			m_DataPos = m_Stream.Position;
			Position = 0;
		}

		/// <summary>Write a new header</summary>
		public static Stream CreateStream(Stream waveData, WaveFormat format) {
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);

			writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF".ToCharArray()));
			
			writer.Write((Int32)(waveData.Length + 36)); //File length minus first 8 bytes of RIFF description

			writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt ".ToCharArray()));
		
			writer.Write((Int32)16); //length of following chunk: 16

			writer.Write((Int16)format.wFormatTag);
			writer.Write((Int16)format.nChannels);
			writer.Write((Int32)format.nSamplesPerSec);
			writer.Write((Int32)format.nAvgBytesPerSec);
			writer.Write((Int16)format.nBlockAlign);
			writer.Write((Int16)format.wBitsPerSample);

			writer.Write(System.Text.Encoding.ASCII.GetBytes("data".ToCharArray()));
			
			writer.Write((Int32)waveData.Length);

			waveData.Seek(0, SeekOrigin.Begin);
			byte[] b = new byte[waveData.Length];
			waveData.Read(b, 0, (int)waveData.Length);
			writer.Write(b);

			writer.Seek(0, SeekOrigin.Begin);
			return stream;
		}


		public WaveStream(Stream sourceStream, Stream destinationStream)
		{
			m_Stream = sourceStream;
			CopyHeader(destinationStream);
		}
		
		public WaveStream(Stream sourceStream) {
			m_Stream = sourceStream;
			ReadHeader();
		}

		~WaveStream()
		{
			Dispose();
		}
		
		public void Dispose()
		{
			if (m_Stream != null)
				m_Stream.Close();
			GC.SuppressFinalize(this);
		}

		public override bool CanRead
		{
			get { return true; }
		}
		public override bool CanSeek
		{
			get { return true; }
		}
		public override bool CanWrite
		{
			get { return false; }
		}
		public override long Length
		{
			get { return m_Length; }
		}

		/// <summary>Length of the data (in samples)</summary>
		public long CountSamples {
			get { return (long)((m_Length - m_DataPos) / (m_Format.wBitsPerSample/8)); }
		}

		public override long Position
		{
			get { return m_Stream.Position - m_DataPos; }
			set { Seek(value, SeekOrigin.Begin); }
		}
		public override void Close()
		{
			Dispose();
		}
		public override void Flush()
		{
		}
		public override void SetLength(long len)
		{
			throw new InvalidOperationException();
		}
		public override long Seek(long pos, SeekOrigin o)
		{
			switch(o)
			{
				case SeekOrigin.Begin:
					m_Stream.Position = pos + m_DataPos;
					break;
				case SeekOrigin.Current:
					m_Stream.Seek(pos, SeekOrigin.Current);
					break;
				case SeekOrigin.End:
					m_Stream.Position = m_DataPos + m_Length - pos;
					break;
			}
			return this.Position;
		}

		public override int Read(byte[] buf, int ofs, int count)
		{
			int toread = (int)Math.Min(count, m_Length - Position);
			return m_Stream.Read(buf, ofs, toread);
		}

		/// <summary>Read - Changed to Copy</summary>
		/// <param name="buf">Buffer to receive the data</param>
		/// <param name="ofs">Offset</param>
		/// <param name="count">Count of bytes to read</param>
		/// <param name="destination">Where to copy the buffer</param>
		/// <returns>Count of bytes actually read</returns>
		public int Copy(byte[] buf, int ofs, int count, Stream destination) {
			int toread = (int)Math.Min(count, m_Length - Position);
			int read = m_Stream.Read(buf, ofs, toread);
			destination.Write(buf, ofs, read);

			if(m_Stream.Position != destination.Position){
				Console.WriteLine();
			}

			return read;
		}

		public override void Write(byte[] buf, int ofs, int count)
		{
			throw new InvalidOperationException();
		}
	}
}
