using UnityEngine;

// ResolverQueue is a priority queue implementation for IResolver.
public class ResolverQueue {

	private const int MaxElements = 1000;

	private class ResolverNode {

		public IResolvable resolver;
		public int priority;
		public int enqueueIndex;

		public ResolverNode(IResolvable resolver, int priority, int enqueueIndex) {
			this.resolver = resolver;
			this.priority = priority;
			this.enqueueIndex = enqueueIndex;
		}

		public static bool operator <(ResolverNode x, ResolverNode y) {
			if (x.priority < y.priority) {
				return true;
			}

			if (x.priority == y.priority && x.enqueueIndex > y.enqueueIndex) {
				return true;
			}

			return false;
		}

		public static bool operator >(ResolverNode x, ResolverNode y) {
			if (x.priority > y.priority) {
				return true;
			}

			if (x.priority == y.priority && x.enqueueIndex < y.enqueueIndex) {
				return true;
			}

			return false;
		}

	}

	private int enqueueIndex;
	private int queueSize;
	private ResolverNode[] resolverArray;

	public ResolverQueue() {
		enqueueIndex = 0;
		queueSize = 0;
		resolverArray = new ResolverNode[MaxElements];
	}

	public int Size() {
		return queueSize;
	}

	public void Enqueue(IResolvable resolver, int priority) {
		if (queueSize == MaxElements) {
			return;
		}

		ResolverNode resolverNode = new ResolverNode(resolver, priority, enqueueIndex++);
		int index = queueSize++;
		resolverArray[index] = resolverNode;
		BubbleUp(index);

		Debug.Log(string.Format(
			"Enqueued {0} with priority {1} and enqueue index {2}",
			resolverNode.resolver,
			resolverNode.priority,
			resolverNode.enqueueIndex
		));
	}

	public IResolvable Peek() {
		if (queueSize == 0) {
			return null;
		}

		return resolverArray[0].resolver;
	}

	public IResolvable Dequeue() {
		if (queueSize == 0) {
			return null;
		}

		Debug.Log(string.Format(
			"Dequeued {0} with priority {1} and enqueue index {2}",
			resolverArray[0].resolver.GetType().FullName,
			resolverArray[0].priority,
			resolverArray[0].enqueueIndex
		));

		IResolvable resolver = resolverArray[0].resolver;
		resolverArray[0] = null;
		queueSize--;

		if (queueSize == 0) {
			return resolver;
		}

		Swap(0, queueSize);
		BubbleDown(0);
		return resolver;
	}

	private void BubbleUp(int index) {
		int currentIndex = index;
		int parentIndex = GetParentIndex(currentIndex);

		while (parentIndex >= 0 && resolverArray[currentIndex] > resolverArray[parentIndex]) {
			Swap(currentIndex, parentIndex);
			currentIndex = parentIndex;
			parentIndex = GetParentIndex(currentIndex);
		}
	}

	private void BubbleDown(int index) {
		int currentIndex = index;
		int leftChildIndex = GetLeftChildIndex(currentIndex);
		int rightChildIndex = GetRightChildIndex(currentIndex);

		while ((leftChildIndex < queueSize && resolverArray[currentIndex] < resolverArray[leftChildIndex]) ||
			(rightChildIndex < queueSize && resolverArray[currentIndex] < resolverArray[rightChildIndex])) {
			if (rightChildIndex >= queueSize || resolverArray[leftChildIndex] > resolverArray[rightChildIndex]) {
				Swap(currentIndex, leftChildIndex);
				currentIndex = leftChildIndex;
			} else {
				Swap(currentIndex, rightChildIndex);
				currentIndex = rightChildIndex;
			}

			leftChildIndex = GetLeftChildIndex(currentIndex);
			rightChildIndex = GetRightChildIndex(currentIndex);
		}
	}

	private int GetParentIndex(int index) {
		return (index + 1) / 2 - 1;
	}

	private int GetLeftChildIndex(int index) {
		return 2 * index + 1;
	}

	private int GetRightChildIndex(int index) {
		return 2 * index + 2;
	}

	private void Swap(int indexA, int indexB) {
		ResolverNode tmp = resolverArray[indexA];
		resolverArray[indexA] = resolverArray[indexB];
		resolverArray[indexB] = tmp;
	}

}
