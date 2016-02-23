# MessageBus
Message bus to pass messages between objects without coupling them.

https://www.nuget.org/packages/EmeraldTeam.MessageBus/1.0.0

Passing messages is implemented via next two methods:

#public static void Send<T>(object key, T arguments)

#public static void Send(object key)

And handling them is implemented via next two methods:

#public static void Subscribe<T>(this object subscriber, object key, Action<T> listenerAction)

#public static void Subscribe(this object subscriber, object key, Action listenerAction)

So you could write something like

object1.Subscribe<string>("ChatChannel", chatMessage => Console.WriteLine(chatMessage));

object2.Send("ChatChannel", "Hello!");
