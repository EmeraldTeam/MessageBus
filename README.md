# MessageBus
Message bus to pass messages between objects without coupling them.

[![NuGet Status](http://img.shields.io/nuget/v/gitter-api-pcl.svg?style=flat)](https://www.nuget.org/packages/EmeraldTeam.MessageBus)

Passing messages is implemented via next two methods:

<code>public static void Send&#60;T&#62;(object key, T arguments)</code>

<code>public static void Send(object key)</code>

And handling them is implemented via next two methods:

<code>public static void Subscribe&#60;T&#62;(this object subscriber, object key, Action&#60;T&#62; listenerAction)</code>

<code>public static void Subscribe(this object subscriber, object key, Action listenerAction)</code>

So you could write something like

<code>object1.Subscribe&#60;string&#62;("ChatChannel", chatMessage => Console.WriteLine(chatMessage));</code>

<code>object2.Send("ChatChannel", "Hello!");</code>

<b>Pay attention!</b>

If your subscriber is expecting argument of type and you are sending a derived class or implementation then message will not be received.
To avoid this you need to manually type expecting argument type in Send call.
