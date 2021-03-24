using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine;

public class Message
{

    public const byte FALSE_BYTE = 0;
    public const byte TRUE_BYTE = 1;

    public const byte DELIMITER = 28;
    public const byte DEFAULT_CONTENT_DELIMITER = 29;
    public const byte END_OF_MESSAGE = 31;
 
    public const int Request = 0;
    public const int Response = 1;
 
	public const int NewPlayerReq = 0;
	public const int StartGameReq = 1;
	public const int StartTurnReq = 2;
	public const int PlacePieceReq = 3;
	public const int UpdateStateReq = 4;
    public const int ChallengePlayerReq = 5;
    public const int ProposalAnswerReq = 6; // bool
    public const int CancelProposalReq = 7;
 
	public const int NewPlayerResp = 0;
	public const int StartGameResp = 1;
	public const int StartTurnResp = 2;
	public const int PlacePieceResp = 3;
    public const int UpdateLobbyResp = 4;
    public const int ChallengeProposalResp = 5;
    public const int WaitForChallengeResp = 6;
    public const int ChallengeRejectedResp = 7;
    public const int CancelProposalResp = 8;
    public const int GameOverResp = 9;

    public byte Type;
    public byte ID;
    private byte delimiter;
    public List<byte[]> Content;

    public Message(byte type, byte id, byte delimiter) : 
        this(type,id,delimiter,new List<byte[]>()) {
    }

    public Message(byte type, byte id, byte delimiter, List<byte[]> content) {
        Type = type;
        ID = id;
        this.delimiter = delimiter;
        Content = content;
    }

    public void AddContentByte(byte b) {
        Content.Add(new byte[]{b});
    }

    public void AddContentString(string newContent) {
        Content.Add(Encoding.ASCII.GetBytes(newContent));
    }

    public void AddContentStrings(string[] newContent) {
        foreach(string s in newContent) {
            AddContentString(s);
        }
    }

    public List<string> GetContentStringList() {
        List<string> strings = new List<string>();
        foreach(byte[] cs in Content) {
            strings.Add(Encoding.ASCII.GetString(cs, 0, cs.Length));
        }
        return strings;
    }

    public byte[] Serialize() {
        //First serialize the message type
        MemoryStream builder = new MemoryStream();
        builder.Append(Type);
        builder.Append(DELIMITER);

        //Second serialize the messageID
        builder.Append(ID);
        builder.Append(DELIMITER);

        //Third serialize the delimiter (if any) to be used when parsing the content
        builder.Append(delimiter);
        builder.Append(DELIMITER);

        //Last append the content
        foreach(byte[] s in Content) {
            builder.Append(s);
            builder.Append(delimiter);
        }
        builder.Append(DELIMITER);
        builder.Append((byte)'\n');
        return builder.ToArray();
    }

    public static Message Deserialize(byte[] receivedData) {
        ByteBuffer buffer = new ByteBuffer(receivedData);
        return Deserialize(buffer);
    }

    public static Message Deserialize(ByteBuffer buffer) {
        byte messageType;
        byte messageID;
        byte contentDelimiter;
        List<byte[]> content = new List<byte[]>();
        try {
            messageType = buffer.ReadByte();
            buffer.ConsumeDelimiter(DELIMITER);
    
            messageID = buffer.ReadByte();
            buffer.ConsumeDelimiter(DELIMITER);

            contentDelimiter = buffer.ReadByte();
            buffer.ConsumeDelimiter(DELIMITER);

            byte[] c = buffer.ReadBytes(contentDelimiter);
            for( ; c.Length > 0; c = buffer.ReadBytes(contentDelimiter)) {
                if(c == null) {
                    break;
                }
                content.Add(c);
            }
        } catch (InvalidOperationException e) {
            Debug.Log("Invalid operation " + e.ToString());
            return null;
        }
        return new Message(messageType, messageID, contentDelimiter, content);
    
    }

}
