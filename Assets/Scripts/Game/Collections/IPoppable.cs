public interface IPoppable<T> {

	T Pop();
	T[] PopMany(int count);

}
