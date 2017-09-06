using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for Class1
/// </summary>
public class EventDispatcher
{
    public delegate void Handler();
    private static Dictionary<int, Handler> eventMap = new Dictionary<int, Handler>();

    public delegate void InnerEventHandler(object data);
    private static Dictionary<int, List<InnerEventHandler>> innerEventMap = new Dictionary<int, List<InnerEventHandler>>();


    //消息派发
    public static int talkMessageEvent = 10008;


    public static void addInnerEventHandler(int eventId, InnerEventHandler callback)
    {
        if (innerEventMap.ContainsKey(eventId))
        {
            innerEventMap[eventId].Add(callback);
        }
        else
        {
            innerEventMap[eventId] = new List<InnerEventHandler>();
            innerEventMap[eventId].Add(callback);
        }
    }
    public static void addEventHandler(int msgId, Handler callback)
	{
        eventMap[msgId] = null;
        eventMap.Remove(msgId);
	}


    public static void DispatchInnerEvent(int eventId, object data)
    {
        if (innerEventMap.ContainsKey(eventId))
        {
            List<InnerEventHandler> callbacks = innerEventMap[eventId];
            if (callbacks != null)
            {
                foreach (InnerEventHandler callback in callbacks)
                {
                    callback(data);
                }
            }
        }

    }

    public static void removeEventHandler(int msgId)
    {
        eventMap[msgId] = null;
        eventMap.Remove(msgId);
    }


    private static ByteBuffer raw = new ByteBuffer();
    //for network event
    public static void Execute(KeyValuePair<int, ByteBuffer> data)
    {
        KeyValuePair<int, ByteBuffer> buffer = (KeyValuePair<int, ByteBuffer>)data;
        int msgId = buffer.Key;

        if (eventMap.ContainsKey(msgId))
        {
            Handler callback = eventMap[msgId];

            if (callback != null)
            {
                raw.SetPosition(0);
                //raw.WriteBytes(buff.ToBytes());
                raw.SetPosition(0);
                //callback(raw);
            }


        }
    }
}
