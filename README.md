# MessageBus
Message bus to pass messages between objects without coupling them.

[![NuGet Status](http://img.shields.io/nuget/v/gitter-api-pcl.svg?style=flat)](https://www.nuget.org/packages/EmeraldTeam.MessageBus/1.0.0)

Passing messages is implemented via next two methods:

<b>public static void Send<T>(object key, T arguments)</b>

<b>public static void Send(object key)</b>

And handling them is implemented via next two methods:

<b>public static void Subscribe<T>(this object subscriber, object key, Action<T> listenerAction)</b>

<b>public static void Subscribe(this object subscriber, object key, Action listenerAction)</b>

So you could write something like

<b>object1.Subscribe<string>("ChatChannel", chatMessage => Console.WriteLine(chatMessage));</b>

<b>object2.Send("ChatChannel", "Hello!");</b>
