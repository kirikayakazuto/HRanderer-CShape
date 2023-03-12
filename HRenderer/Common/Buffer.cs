using System;
using System.IO;

namespace HRenderer.Common {
    public class Buffer {
        
        private BinaryReader _binaryReader;
        private BinaryWriter _binaryWriter;
        private MemoryStream _memoryStream;
        private bool mBEMode; //big endian mode
        
        public Buffer(bool isBigEndian) {
            this._memoryStream = new MemoryStream();
            this._binaryReader = new BinaryReader(this._memoryStream);
            this._binaryWriter = new BinaryWriter(this._memoryStream);
            this.mBEMode = isBigEndian;
        }

        public void Close() {
            this._memoryStream.Close();
            this._binaryReader.Close();
            this._binaryWriter.Close();;
        }

        public long Position {
            get => this._memoryStream.Position;
            set => this._memoryStream.Position = value;
        }

        public long Length => this._memoryStream.Length;
        
        /**
         * byte
         */
        public void WeiteByte(byte value) {
            this._binaryWriter.Write(value);
        }
        public byte ReadByte() {
            return this._binaryReader.ReadByte();
        }
        
        /**
         * int16
         */
        public void WriteInt16(Int16 value) {
            this.Write(BitConverter.GetBytes(value));
        }
        public Int16 ReadInt16() {
            return this._binaryReader.ReadInt16();
        }

        /**
         * int 32
         */
        public void WriteInt32(Int32 value) {
            this.Write(BitConverter.GetBytes(value));
        }
        public Int32 ReadInt32() {
            return this._binaryReader.ReadInt32();
        }

        /**
         * float
         */
        public void WriteFloat(float value) {
            this.Write(BitConverter.GetBytes(value));
        }
        public float ReadFloat() {
            return this._binaryReader.ReadSingle();
        }

        /**
         * uint 32
         */
        public void WriteUInt32(UInt32 value) {
            this.Write(BitConverter.GetBytes(value));
        }
        public UInt32 ReadUInt32() {
            return this._binaryReader.ReadUInt32();
        }
        
        private void Write(byte[] bytes) {
            this._binaryWriter.Write(bytes);
        }
    }
}