using System;

public interface IMessage {
    string Name { get; }
    string Body { get; set; }
    string Type { get; set; }
    string ToString();
}
