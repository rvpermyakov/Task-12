using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree {
	class Node {
		public int value;
		public Node left = null;
		public Node right = null;

		public Node(int value) {
			this.value = value;
		}
	}

	class BinTree {
		public Node head;

		public BinTree(int value) {
			head = new Node(value);
		}

		public void Add(int value, Node node, ref int comparers) {
			comparers++;
			if (value > node.value) {
				if (node.right == null)
					node.right = new Node(value);
				else
					Add(value, node.right, ref comparers);
			} else {
				if (node.left == null)
					node.left = new Node(value);
				else
					Add(value, node.left, ref comparers);
			}
		}

		public void ToList(Node node, List<int> array) {
			if (node == null)
				return;

			ToList(node.left, array);
			array.Add(node.value);
			ToList(node.right, array);
		}
	}
}

namespace Sort
{
	class Program
	{
		static int comparers = 0;
		static int forwardings = 0;
		static Random rnd = new Random();

		static int[] TreeSort(int[] sourceArray) {
			//Для сортировки используется бинарное дерево поиска, с последующим инфиксным обходом для получения линейного списка вершин
			Tree.BinTree sourceTree = new Tree.BinTree(sourceArray[0]);

			for (int i = 1; i < sourceArray.Length; i++)
				sourceTree.Add(sourceArray[i], sourceTree.head, ref comparers);

			List<int> result = new List<int>();
			sourceTree.ToList(sourceTree.head, result);
			return result.ToArray();
		}

		static List<int> InsertionSort(List<int>[] buckets) {
			List<int> result = new List<int>();
			for (int i = 0; i < buckets.Length; i++)
				buckets[i].Sort();
			
			while (true) {
				bool haveItems = false;
				int min = int.MaxValue;
				List<int> minList = null;
				for (int i = 0; i < buckets.Length; i++) {
					comparers++;
					if (buckets[i].Count > 0 && min > buckets[i][0]) {
						haveItems = true;
						min = buckets[i][0];
						minList = buckets[i];
					}
				}

				if (haveItems) {
					forwardings++;
					result.Add(min);
					minList.Remove(min);
				} else
					break;
			}

			return result;
		}

		static int[] BucketSort(int[] sourceArray) {
			int min = sourceArray.Min();
			int max = sourceArray.Max();
			//Если минимальное отрицательное число по модулю будет больше, чем максимальное положительное, то максимальным числом ставим модуль минимального отрицательного
			//Это позволит избежать выход за границы массива, при вычислении номера "корзины", в которую мы положим значение для сортировки
			max = (max > Math.Abs(min)) ? max : Math.Abs(min);
			int blocksCount = 100; //Какой-либо подбор формулы невозможен. Для сортировки массивов с кол-вом элементов >= 10^8 рекомендуется использовать 1500 "корзин"

			List<int>[] buckets = new List<int>[blocksCount];
			for (int i = 0; i < buckets.Length; i++)
				buckets[i] = new List<int>();

			for (int i = 0; i < sourceArray.Length; i++) {
				int index = (int)Math.Floor(Convert.ToDouble(Math.Abs(sourceArray[i]) * (blocksCount - 1)) / max);
				buckets[index].Add(sourceArray[i]);
				forwardings++;
			}

			for (int i = 0; i < blocksCount; i++)
				buckets[i].Sort();

			//Для слияния всех корзин используется простая сортировка вставками
			List<int> result = InsertionSort(buckets);
			return result.ToArray();
		}

		static int[] Generate(int n)
		{
			int[] temp = new int[n];
			for (int i = 0; i < n; i++)
				temp[i] = rnd.Next(-99, 99);
			return temp;
		}

		static void PrintStats(int[] sourceArray, int[] sortedArray, string typeSort)
		{
			Console.Write("Source array:\n\t");
			for (int i = 0; i < sourceArray.Length; i++)
				Console.Write("{0} ", sourceArray[i]);
			Console.WriteLine();
			Console.Write("Sorted by {0} array:\n\t", typeSort);
			for (int i = 0; i < sortedArray.Length; i++)
				Console.Write("{0} ", sortedArray[i]);
			Console.WriteLine();
			Console.WriteLine("Number of forwardings: {0}", forwardings);
			Console.WriteLine("Number of comparers: {0}", comparers);
			Console.Write("\n\n\n");
			forwardings = 0;
			comparers = 0;
		}

		static void Main(string[] args)
		{
			Console.Write("N1 = ");
			int n1 = int.Parse(Console.ReadLine());
			int[] array1 = Generate(n1);

			Console.Write("N2 = ");
			int n2 = int.Parse(Console.ReadLine());
			int[] array2 = Generate(n2);

			Console.Write("N3 = ");
			int n3 = int.Parse(Console.ReadLine());
			int[] array3 = Generate(n3);


			int[] treeArray1 = TreeSort(array1);
			PrintStats(array1, treeArray1, "tree sort");
			int[] bucketArray1 = BucketSort(array1);
			PrintStats(array1, bucketArray1, "bucket sort");

			int[] treeArray2 = TreeSort(array2);
			PrintStats(array2, treeArray2, "tree sort");
			int[] bucketArray2 = BucketSort(array2);
			PrintStats(array2, bucketArray2, "bucket sort");

			int[] treeArray3 = TreeSort(array3);
			PrintStats(array3, treeArray3, "tree sort");
			int[] bucketArray3 = BucketSort(array3);
			PrintStats(array3, bucketArray3, "bucket sort");

			Console.ReadKey();
		}
	}
}