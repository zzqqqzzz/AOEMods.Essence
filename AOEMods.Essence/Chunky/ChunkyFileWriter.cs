﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOEMods.Essence.Chunky;

public class ChunkyFileWriter : BinaryWriter
{
    public ChunkyFileWriter(Stream output) : base(output)
    {
    }

    public ChunkyFileWriter(Stream output, Encoding encoding) : base(output, encoding)
    {
    }

    public ChunkyFileWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
    {
    }

    protected ChunkyFileWriter()
    {
    }

    public void WriteCString(string s)
    {
        Write(Encoding.UTF8.GetBytes(s));
        Write((byte)0);
    }

    public void Write(ChunkHeader header)
    {
        Write(Encoding.UTF8.GetBytes(header.Type));
        Write(Encoding.UTF8.GetBytes(header.Name));
        Write(header.Version);
        Write(header.Length);
        Write(header.Path.Length);
        Write(Encoding.UTF8.GetBytes(header.Path));
    }

    public void Write(ChunkyFileHeader header)
    {
        Write(header.Magic);
        Write(header.Version);
        Write(header.Platform);
    }
}