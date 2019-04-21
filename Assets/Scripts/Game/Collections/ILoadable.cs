using System.Collections;

public interface ILoadable {

	IEnumerator Load();
	IEnumerator RandomLoad(int randomSeed);

}
