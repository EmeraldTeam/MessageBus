# MessageBus
Message bus to pass messages between objects without coupling them.

https://www.nuget.org/packages/EmeraldTeam.MessageBus/1.0.0

Passing messages is implemented via next two methods:

/// <summary>
/// Send message to the bus
/// </summary>
/// <param name="key">Channel key</param>
/// <param name="arguments">Arguments of message</param>
public static void Send<T>(object key, T arguments)
{
	var fullKey = new ChannelKey(key, typeof(T));
	if (!Listeners.ContainsKey(fullKey))
		return;
	var actionList = (List<MessageReaction<T>>)Listeners[fullKey];
	foreach (var listener in actionList)
	{
		listener.MessageHandler(arguments);
	}
}

/// <summary>
/// Send message to the bus
/// </summary>
/// <param name="key">Channel key</param>
public static void Send(object key)
{
	var fullKey = new ChannelKey(key);
	if (!Listeners.ContainsKey(fullKey))
		return;
	var actionList = (List<MessageReaction>)Listeners[fullKey];
	foreach (var listener in actionList)
	{
		listener.MessageHandler();
	}
}

And handling them is implemented via next two methods:

/// <summary>
/// Subscribe on messages from channel with given key and given argument type
/// </summary>
/// <param name="subscriber">Subscriber object</param>
/// <param name="key">Channel key</param>
/// <param name="listenerAction">Action to react on message</param>
public static void Subscribe<T>(this object subscriber, object key, Action<T> listenerAction)
{
	var fullKey = new ChannelKey(key, typeof(T));
	if (!Listeners.ContainsKey(fullKey))
		Listeners[fullKey] = new List<MessageReaction<T>>();

	var list = (List<MessageReaction<T>>)Listeners[fullKey];
	if (list.Any(reaction => reaction.Subscriber.Equals(subscriber)))
		return;

	list.Add(new MessageReaction<T>(subscriber, listenerAction));
}

/// <summary>
/// Subscribe on messages from channel with given key
/// </summary>
/// <param name="subscriber">Subscriber object</param>
/// <param name="key">Channel key</param>
/// <param name="listenerAction">Action to react on message</param>
public static void Subscribe(this object subscriber, object key, Action listenerAction)
{
	var fullKey = new ChannelKey(key);
	if (!Listeners.ContainsKey(fullKey))
		Listeners[fullKey] = new List<MessageReaction>();

	var list = (List<MessageReaction>)Listeners[fullKey];
	if (list.Any(reaction => reaction.Subscriber.Equals(subscriber)))
		return;

	list.Add(new MessageReaction(subscriber, listenerAction));
}

So you could write something like

object1.Subscribe<string>("ChatChannel", chatMessage => Console.WriteLine(chatMessage));
object2.Send("ChatChannel", "Hello!");
