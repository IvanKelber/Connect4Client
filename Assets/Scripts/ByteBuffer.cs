using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteBuffer
{

    List<byte> data;
    public byte[] Receiver;

    int lastRead = -1;

    public ByteBuffer() {
        data = new List<byte>();
    }

    public ByteBuffer(byte[] bytes) {
        data = new List<byte>(bytes);
    }

    public void WriteByte(byte b) {
        data.Add(b);
    }

    public void Write(byte[] b) {
        data.AddRange(b);
    }

    public byte ReadByte() {
        if(lastRead < data.Count - 1) {
            return data[++lastRead];
        }
        throw new InvalidOperationException("Reached end of buffer: " + lastRead);
    }

    //Reads a buffer until it finds a delimiter or EOF
    public byte[] ReadBytes(byte delimiter) {
        int start = lastRead + 1;
        while(lastRead < data.Count - 1) {
            if(data[++lastRead] == delimiter) {
                return data.GetRange(start, lastRead - start).ToArray();
            }
        }
        lastRead = start;
        return new byte[]{};
         
    }

    public string ToString() {
        string s = "[";
        foreach(byte b in data) {
            s += b + " ";
        }
        s += "]";
        return s;
    }

    public void ConsumeDelimiter(byte delimiter) {
        if(lastRead < data.Count - 1 && data[lastRead + 1] == delimiter) {
            lastRead++;
            return;
        } 
    }

    public void Clear() {
        data.Clear();
    }

    public void Catchup() {
        Write(Receiver);
        Array.Clear(Receiver, 0, Receiver.Length);
    }

}
