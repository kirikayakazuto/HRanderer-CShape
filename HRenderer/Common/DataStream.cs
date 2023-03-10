using System;
using System.IO;

namespace HRenderer.Common {
	public class DataStream {
		private BinaryReader _mBinReader;
		private BinaryWriter _mBinWriter;
		private MemoryStream _mMemStream;
		private bool mBEMode; //big endian mode

		public DataStream(bool isBigEndian) {
			_mMemStream = new MemoryStream();
			InitWithMemoryStream(_mMemStream, isBigEndian);
		}

		public DataStream(byte[] buffer, bool isBigEndian) {
			_mMemStream = new MemoryStream(buffer);
			InitWithMemoryStream(_mMemStream, isBigEndian);
		}

		public DataStream(byte[] buffer, int index, int count, bool isBigEndian) {
			_mMemStream = new MemoryStream(buffer, index, count);
			InitWithMemoryStream(_mMemStream, isBigEndian);
		}

		private void InitWithMemoryStream(MemoryStream ms, bool isBigEndian) {
			_mBinReader = new BinaryReader(_mMemStream);
			_mBinWriter = new BinaryWriter(_mMemStream);
			mBEMode = isBigEndian;
		}

		public void Close() {
			_mMemStream.Close();
			_mBinReader.Close();
			_mBinWriter.Close();
		}

		public void SetBigEndian(bool isBigEndian) {
			mBEMode = isBigEndian;
		}

		public bool IsBigEndian() {
			return mBEMode;
		}

		public long Position {
			get { return _mMemStream.Position; }
			set { _mMemStream.Position = value; }
		}

		public long Length {
			get { return _mMemStream.Length; }
		}

		public byte[] ToByteArray() {
			return _mMemStream.ToArray();
		}
		
		public long Seek(long offset, SeekOrigin loc) {
			return _mMemStream.Seek(offset, loc);
		}

		public void WriteRaw(byte[] bytes) {
			_mBinWriter.Write(bytes);
		}

		public void WriteRaw(byte[] bytes, int offset, int count) {
			_mBinWriter.Write(bytes, offset, count);
		}

		public void WriteByte(byte value) {
			_mBinWriter.Write(value);
		}

		public byte ReadByte() {
			return _mBinReader.ReadByte();
		}

		public void WriteInt16(UInt16 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public UInt16 ReadInt16() {
			UInt16 val = _mBinReader.ReadUInt16();
			if (mBEMode)
				return BitConverter.ToUInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}

		public void WriteInt32(UInt32 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public UInt32 ReadInt32() {
			UInt32 val = _mBinReader.ReadUInt32();
			if (mBEMode)
				return BitConverter.ToUInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}
		
		public void WriteFloat(float value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public float ReadFlot() {
			var val = _mBinReader.ReadSingle();
			if (mBEMode) val = BitConverter.ToSingle(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}
		
		public void WriteInt64(UInt64 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public UInt64 ReadInt64() {
			UInt64 val = _mBinReader.ReadUInt64();
			if (mBEMode)
				return BitConverter.ToUInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}

		public void WriteString8(string value) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			byte[] bytes = encoding.GetBytes(value);
			_mBinWriter.Write((byte) bytes.Length);
			_mBinWriter.Write(bytes);
		}

		public string ReadString8() {
			int len = ReadByte();
			byte[] bytes = _mBinReader.ReadBytes(len);
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			return encoding.GetString(bytes);
		}

		public void WriteString16(string value) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			byte[] data = encoding.GetBytes(value);
			WriteInteger(BitConverter.GetBytes((Int16) data.Length));
			_mBinWriter.Write(data);
		}

		public string ReadString16() {
			ushort len = ReadInt16();
			byte[] bytes = _mBinReader.ReadBytes(len);
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			return encoding.GetString(bytes);
		}

		private void WriteInteger(byte[] bytes) {
			if (mBEMode) FlipBytes(bytes);
			_mBinWriter.Write(bytes);
		}

		private byte[] FlipBytes(byte[] bytes) {
			for (int i = 0, j = bytes.Length - 1; i < j; ++i, --j) {
				(bytes[i], bytes[j]) = (bytes[j], bytes[i]);
			}
			return bytes;
		}

		public void WriteSByte(sbyte value) {
			_mBinWriter.Write(value);
		}

		public sbyte ReadSByte() {
			return _mBinReader.ReadSByte();
		}
		

		public void WriteSInt16(Int16 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public Int16 ReadSInt16() {
			Int16 val = _mBinReader.ReadInt16();
			if (mBEMode) return BitConverter.ToInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}

		public void WriteSInt32(Int32 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public Int32 ReadSInt32() {
			Int32 val = _mBinReader.ReadInt32();
			if (mBEMode) return BitConverter.ToInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}

		public void WriteSInt64(Int64 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public Int64 ReadSInt64() {
			Int64 val = _mBinReader.ReadInt64();
			if (mBEMode) return BitConverter.ToInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}

		public void WriteUByte(byte value) {
			_mBinWriter.Write(value);
		}

		public byte ReadUByte() {
			return _mBinReader.ReadByte();
		}

		public void WriteUInt16(UInt16 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public UInt16 ReadUInt16() {
			UInt16 val = _mBinReader.ReadUInt16();
			if (mBEMode)
				return BitConverter.ToUInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}

		public void WriteUInt32(UInt32 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public UInt32 ReadUInt32() {
			UInt32 val = _mBinReader.ReadUInt32();
			if (mBEMode) return BitConverter.ToUInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}

		public void WriteUInt64(UInt64 value) {
			WriteInteger(BitConverter.GetBytes(value));
		}

		public UInt64 ReadUInt64() {
			UInt64 val = _mBinReader.ReadUInt64();
			if (mBEMode)
				return BitConverter.ToUInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
			return val;
		}
	}
}