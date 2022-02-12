﻿using System;
using Bedrock.Framework.Experimental.Protocols.Framing.VariableSized.LengthFielded;

namespace ServerApplication.Framing.VariableSized.LengthFielded
{
    internal class Header : IHeader, IEquatable<Header>
    {
        public int PayloadLength { get; }
        public int SomeCustomData { get; }

        public Header(int payloadLength, int someCustomData)
        {
            PayloadLength = payloadLength;
            SomeCustomData = someCustomData;
        }

        public Header(ReadOnlySpan<byte> headerAsSpan)
        {
            PayloadLength = BitConverter.ToInt32(headerAsSpan.Slice(0, 4));
            SomeCustomData = BitConverter.ToInt32(headerAsSpan.Slice(4));
        }

        public ReadOnlySpan<byte> AsSpan()
        {
            var payloadLengthAsArray = BitConverter.GetBytes(PayloadLength);
            var someCustomDataAsArray = BitConverter.GetBytes(SomeCustomData);

            byte[] header = new byte[Helper.HeaderLength];
            header[0] = payloadLengthAsArray[0];
            header[1] = payloadLengthAsArray[1];
            header[2] = payloadLengthAsArray[2];
            header[3] = payloadLengthAsArray[3];
            header[4] = someCustomDataAsArray[0];
            header[5] = someCustomDataAsArray[1];
            header[6] = someCustomDataAsArray[2];
            header[7] = someCustomDataAsArray[3];
            return header.AsSpan();
        }

        public override string ToString() => $"Payload length: {PayloadLength} - Some custom data: {SomeCustomData}";

        #region IEquatable
        public override bool Equals(object obj) => Equals((Header)obj);

        public override int GetHashCode() => HashCode.Combine(PayloadLength, SomeCustomData);

        public bool Equals(Header other) => PayloadLength == other.PayloadLength && SomeCustomData.Equals(other.SomeCustomData);

        public static bool operator ==(Header left, Header right) => left.Equals(right);

        public static bool operator !=(Header left, Header right) => !(left == right);
        #endregion
    }
}
