using System;
using System.Collections.Generic;
using System.Linq;

namespace EmeraldTeam.MessageBus
{
	/// <summary>
	/// Class to deliver messages from one object to another without directly linking them.
	/// It pushes messages to channels on which objects can subscribe.
	/// </summary>
	public static class MessageBus
	{
		/// <summary>
		/// Dictionary of message listeners
		/// </summary>
		private static Dictionary<ChannelKey, object> Listeners { get; } = new Dictionary<ChannelKey, object>();

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

		/// <summary>
		/// Unsubscribe object from channel by key and argument type
		/// </summary>
		/// <param name="subscriber">Object to unsubscribe</param>
		/// <param name="key">Channel key</param>
		public static void UnSubscribe<T>(this object subscriber, object key)
		{
			var fullKey = new ChannelKey(key, typeof(T));
			if (!Listeners.ContainsKey(fullKey))
				return;

			((List<MessageReaction<T>>)Listeners[fullKey]).RemoveAll(listener => listener.Subscriber.Equals(subscriber));
		}

		/// <summary>
		/// Unsubscribe object from channel by key
		/// </summary>
		/// <param name="subscriber">Object to unsubscribe</param>
		/// <param name="key">Channel key</param>
		public static void UnSubscribe(this object subscriber, object key)
		{
			var fullKey = new ChannelKey(key);
			if (!Listeners.ContainsKey(fullKey))
				return;

			((List<MessageReaction>)Listeners[fullKey]).RemoveAll(listener => listener.Subscriber.Equals(subscriber));
		}

		/// <summary>
		/// Class to represent a complex channel key
		/// </summary>
		private class ChannelKey
		{
			/// <summary>
			/// Channel key
			/// </summary>
			private object Key { get; }

			/// <summary>
			/// Channel argument type
			/// </summary>
			private Type ArgumentType { get; }

			public ChannelKey(object key, Type argumentType = null)
			{
				Key = key;
				ArgumentType = argumentType;
			}

			public override bool Equals(object @object)
			{
				var anotherKey = @object as ChannelKey;
				return anotherKey != null && anotherKey.Key.Equals(Key) && anotherKey.ArgumentType == ArgumentType;
			}

			public override int GetHashCode()
			{
				return (Key?.GetHashCode() ?? 0) + (ArgumentType?.GetHashCode() ?? 0);
			}
		}

		/// <summary>
		/// Class to represent reaction on message
		/// </summary>
		/// <typeparam name="T">Type of argument</typeparam>
		private class MessageReaction<T>
		{
			/// <summary>
			/// Object which reacts
			/// </summary>
			public object Subscriber { get; }

			/// <summary>
			/// Method to react on incoming message
			/// </summary>
			public Action<T> MessageHandler { get; }

			public MessageReaction(object subscriber, Action<T> messageHandler)
			{
				Subscriber = subscriber;
				MessageHandler = messageHandler;
			}
		}

		/// <summary>
		/// Class to represent reaction on message
		/// </summary>
		private class MessageReaction
		{
			/// <summary>
			/// Object which reacts
			/// </summary>
			public object Subscriber { get; }

			/// <summary>
			/// Method to react on incoming message
			/// </summary>
			public Action MessageHandler { get; }

			public MessageReaction(object subscriber, Action messageHandler)
			{
				Subscriber = subscriber;
				MessageHandler = messageHandler;
			}
		}
	}
}