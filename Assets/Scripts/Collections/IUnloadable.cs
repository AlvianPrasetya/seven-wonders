using System.Collections;

public interface IUnloadable<T> {

	IEnumerator Unload(IPushable<T> targetContainer);

}
