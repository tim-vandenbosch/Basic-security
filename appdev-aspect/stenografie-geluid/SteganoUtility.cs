using System;
using System.IO;

namespace SteganoWave1
{
	public class SteganoUtility
	{
		private WaveLib.WaveStream sourceStream;
		private Stream destinationStream;
		private int bytesPerSample;

		public SteganoUtility(WaveLib.WaveStream sourceStream, Stream destinationStream)
		:this(sourceStream){
			this.destinationStream = destinationStream;
		}

		public SteganoUtility(WaveLib.WaveStream sourceStream) {
			this.sourceStream = sourceStream;
			this.bytesPerSample = sourceStream.Format.wBitsPerSample / 8;
		}

		public void Hide(Stream messageStream, Stream keyStream){
			
			byte[] waveBuffer = new byte[bytesPerSample];
			byte message, bit, waveByte;
			int messageBuffer, keyByte;
			
			while( (messageBuffer=messageStream.ReadByte()) >= 0 ){
				//read one byte of the message
				message = (byte)messageBuffer;
				
				//for each bit in message
				for(int bitIndex=0; bitIndex<8; bitIndex++){
					
					//read a byte from the key
					keyByte = GetKeyValue(keyStream);
					
					//skip samples
					for(int n=0; n<keyByte-1; n++){
						sourceStream.Copy(waveBuffer, 0, waveBuffer.Length, destinationStream);
					}

					//read one sample from the wave stream
					sourceStream.Read(waveBuffer, 0, waveBuffer.Length);
					waveByte = waveBuffer[bytesPerSample-1];
					
					bit = (byte)(((message & (byte)(1 << bitIndex)) > 0) ? 1 : 0);
						
					//change the last bit of the sample
					if((bit == 1) && ((waveByte % 2) == 0)){
						waveByte += 1;
					}else if((bit == 0) && ((waveByte % 2) == 1)){
						waveByte -= 1;
					}

					waveBuffer[bytesPerSample-1] = waveByte;

					//write the result to destinationStream
					destinationStream.Write(waveBuffer, 0, bytesPerSample);
					
				}
			}

			waveBuffer = new byte[sourceStream.Length - sourceStream.Position];
			sourceStream.Read(waveBuffer, 0, waveBuffer.Length);
			destinationStream.Write(waveBuffer, 0, waveBuffer.Length);
		}

		public void Extract(Stream messageStream, Stream keyStream){
		
			byte[] waveBuffer = new byte[bytesPerSample];
			byte message, bit, waveByte;
			int messageLength = 0;
			int keyByte;
			
			while( (messageLength==0 || messageStream.Length<messageLength) ){
				//clear the message-byte
				message = 0;
				
				//for each bit in message
				for(int bitIndex=0; bitIndex<8; bitIndex++){

					//read a byte from the key
					keyByte = GetKeyValue(keyStream);
					
					//skip [keyByte] samples
					for(int n=0; n<keyByte; n++){
						//read one sample from the wave stream
						sourceStream.Read(waveBuffer, 0, waveBuffer.Length);
					}
					waveByte = waveBuffer[bytesPerSample-1];
					
					//get the last bit of the sample
					bit = (byte)(((waveByte % 2) == 0) ? 0 : 1);

					//write the bit into the message-byte
					message += (byte)(bit << bitIndex);
					
				}

				//add the re-constructed byte to the message
				messageStream.WriteByte(message);
				
				if(messageLength==0 && messageStream.Length==4){
					//first 4 byte contain the message's length
					messageStream.Seek(0, SeekOrigin.Begin);
					messageLength = new BinaryReader(messageStream).ReadInt32();
					messageStream.SetLength(0);
				}
			}

		}
		
		/// <summary>Counts the samples that will be skipped using the specified key stream</summary>
		/// <param name="keyStream">Key stream</param>
		/// <param name="messageLength">Length of the message</param>
		/// <returns>Minimum length (in samples) of an audio file</returns>
		public long CheckKeyForMessage(Stream keyStream, long messageLength){
			long requiredLength = 0, messageLengthBits = messageLength*8;
			
			for(int n=0; n<messageLengthBits; n++){
				requiredLength += GetKeyValue(keyStream);
			}
			
			keyStream.Seek(0, SeekOrigin.Begin);
			return requiredLength;
		}

		
		private byte GetKeyValue(Stream keyStream){
			int keyValue;
			if( (keyValue=keyStream.ReadByte()) < 0){
				keyStream.Seek(0, SeekOrigin.Begin);
				keyValue=keyStream.ReadByte();
			}
			return (byte)keyValue;
		}
	}
}
