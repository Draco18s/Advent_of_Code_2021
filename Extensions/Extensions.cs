using System.Collections.Generic;

public static class Extentions {
	public static IEnumerable<T> Enumerate<T>(this T[,] arr) {
		int l1 = arr.GetLength(0);
		int l2 = arr.GetLength(1);
		for(int y=0;y<l1;y++) {
			for(int x=0;x<l2;x++) {
				yield return arr[x, y];
			}
		}
	}

	private static System.Random rng = new System.Random();  

	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}