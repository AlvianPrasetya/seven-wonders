using System.Collections;

public interface IActionable {

	IEnumerator Perform(Player player);
	void Effect(Player player);

}
