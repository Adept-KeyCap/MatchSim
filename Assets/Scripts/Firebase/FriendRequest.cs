using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FriendRequest
{
    public string senderId { get; set; }
    public string senderUsername { get; set; }
    public string receiverId { get; set; }
    public string requestKey { get; set; }
    public string status { get; set; } // "pending", "accepted", "rejected"
    

    public FriendRequest()
    {
    }
    public FriendRequest(string senderId,string senderUsername, string receiverId,string requestKey, string status)
    {
        this.senderId = senderId;
        this.senderUsername = senderUsername;
        this.receiverId = receiverId;
        this.requestKey = requestKey;
        this.status = status;
    }
}
