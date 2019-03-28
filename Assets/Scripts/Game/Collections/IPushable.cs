using System.Collections;

public interface IPushable<T> {

	IEnumerator Push(T element);
	IEnumerator PushMany(T[] elements);

}
