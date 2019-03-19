using System.Collections;

public interface IShuffleable {

	IEnumerator Shuffle(int numIterations, int randomSeed);

}
