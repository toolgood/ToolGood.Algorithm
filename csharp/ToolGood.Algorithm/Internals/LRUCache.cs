using System;
using System.Collections.Generic;
using System.Threading;

namespace ToolGood.Algorithm.Internals
{
	internal class LRUCache<TKey, TValue> : IDisposable
	{
		private readonly DoubleLinkedListNode<TKey, TValue> _head;
		private readonly DoubleLinkedListNode<TKey, TValue> _tail;
		private readonly Dictionary<TKey, DoubleLinkedListNode<TKey, TValue>> _dictionary;
		private readonly ReaderWriterLockSlim _slimLock = new ReaderWriterLockSlim();

		private readonly int _capacity;
		private bool _disposed;

		public LRUCache(int capacity)
		{
			if(capacity <= 0) {
				throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
			}
			_capacity = capacity;
			_head = new DoubleLinkedListNode<TKey, TValue>();
			_tail = new DoubleLinkedListNode<TKey, TValue>();
			_head.Next = _tail;
			_tail.Previous = _head;
			_dictionary = new Dictionary<TKey, DoubleLinkedListNode<TKey, TValue>>();
		}

		public bool TryGet(TKey key, out TValue value)
		{
			_slimLock.EnterReadLock();
			try {
				if(_dictionary.TryGetValue(key, out var node)) {
					value = node.Value;
				} else {
					value = default;
					return false;
				}
			} finally {
				_slimLock.ExitReadLock();
			}

			_slimLock.EnterWriteLock();
			try {
				if(_dictionary.TryGetValue(key, out var node)) {
					MoveToLast(node);
				}
			} finally {
				_slimLock.ExitWriteLock();
			}
			return true;
		}

		public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
		{
			_slimLock.EnterReadLock();
			try {
				if(_dictionary.TryGetValue(key, out var node)) {
					var value = node.Value;
					_slimLock.ExitReadLock();
					_slimLock.EnterWriteLock();
					try {
						if(_dictionary.TryGetValue(key, out var node2)) {
							MoveToLast(node2);
						}
					} finally {
						_slimLock.ExitWriteLock();
					}
					return value;
				}
			} finally {
				if(_slimLock.IsReadLockHeld) {
					_slimLock.ExitReadLock();
				}
			}

			var newValue = factory(key);

			_slimLock.EnterWriteLock();
			try {
				if(_dictionary.TryGetValue(key, out var existingNode)) {
					MoveToLast(existingNode);
					return existingNode.Value;
				}
				if(_dictionary.Count >= _capacity) {
					EvictOldest();
				}
				var newNode = new DoubleLinkedListNode<TKey, TValue>(key, newValue);
				AddLast(newNode);
				_dictionary.Add(key, newNode);
				return newValue;
			} finally {
				_slimLock.ExitWriteLock();
			}
		}

		public void Put(TKey key, TValue value)
		{
			_slimLock.EnterWriteLock();
			try {
				if(_dictionary.TryGetValue(key, out var node)) {
					node.Value = value;
					MoveToLast(node);
				} else {
					if(_dictionary.Count >= _capacity) {
						EvictOldest();
					}
					var newNode = new DoubleLinkedListNode<TKey, TValue>(key, value);
					AddLast(newNode);
					_dictionary.Add(key, newNode);
				}
			} finally {
				_slimLock.ExitWriteLock();
			}
		}

		public void Remove(TKey key)
		{
			_slimLock.EnterWriteLock();
			try {
				if(_dictionary.TryGetValue(key, out var node)) {
					_dictionary.Remove(key);
					RemoveNode(node);
				}
			} finally {
				_slimLock.ExitWriteLock();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!_disposed) {
				if(disposing) {
					_slimLock.Dispose();
				}
				_disposed = true;
			}
		}

		private void EvictOldest()
		{
			var first = _head.Next;
			if(first != _tail) {
				RemoveNode(first);
				_dictionary.Remove(first.Key);
			}
		}

		private void MoveToLast(DoubleLinkedListNode<TKey, TValue> node)
		{
			if(node.Next != _tail) {
				RemoveNode(node);
				AddLast(node);
			}
		}

		private void AddLast(DoubleLinkedListNode<TKey, TValue> node)
		{
			node.Previous = _tail.Previous;
			node.Next = _tail;
			_tail.Previous.Next = node;
			_tail.Previous = node;
		}

		private static void RemoveNode(DoubleLinkedListNode<TKey, TValue> node)
		{
			node.Previous.Next = node.Next;
			node.Next.Previous = node.Previous;
		}

		internal class DoubleLinkedListNode<TKey2, TValue2>
		{
			public DoubleLinkedListNode()
			{ }

			public DoubleLinkedListNode(TKey2 key, TValue2 value)
			{
				Key = key;
				Value = value;
			}

			public TKey2 Key { get; private set; }
			public TValue2 Value { get; set; }
			public DoubleLinkedListNode<TKey2, TValue2> Previous { get; set; }
			public DoubleLinkedListNode<TKey2, TValue2> Next { get; set; }
		}
	}
}
