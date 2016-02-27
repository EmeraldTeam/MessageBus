# MessageBus
Message bus to pass messages between objects without coupling them.

[![NuGet Status](http://img.shields.io/nuget/v/gitter-api-pcl.svg?style=flat)](https://www.nuget.org/packages/EmeraldTeam.MessageBus)

Passing messages is implemented via next two methods:

<b>public static void Send&#60;T&#62;(object key, T arguments)</b>

<b>public static void Send(object key)</b>

And handling them is implemented via next two methods:

<b>public static void Subscribe&#60;T&#62;(this object subscriber, object key, Action&#60;T&#62; listenerAction)</b>

<b>public static void Subscribe(this object subscriber, object key, Action listenerAction)</b>

So you could write something like

<b>object1.Subscribe&#60;string&#62;("ChatChannel", chatMessage => Console.WriteLine(chatMessage));</b>

<b>object2.Send("ChatChannel", "Hello!");</b>
