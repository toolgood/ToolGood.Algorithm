using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_MEDIAN : Function_N
	{
		public Function_MEDIAN(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length < 1) {
				throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
			}
		}

		public override string Name => "Median";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>(funcs.Length);
			var error = TryEvaluateAll(engine, tempParameter, args);
			if(error != null) { return error; }

			var list = new List<decimal>();
			if(FunctionUtil.FlattenToList(args, list) == false) { return FunctionError(); }
			if(list.Count == 0) { return FunctionError(); }

			int n = list.Count;
			if(n == 1) return Operand.Create(list[0]);

			int mid = n / 2;
			if(n % 2 == 0) {
				return Operand.Create((QuickSelect(list, mid - 1) + QuickSelect(list, mid)) / 2);
			}
			return Operand.Create(QuickSelect(list, mid));
		}

		private static decimal QuickSelect(List<decimal> arr, int k)
		{
			int left = 0, right = arr.Count - 1;
			while(left < right) {
				int pivot = SelectPivot(arr, left, right);
				int partition = Partition(arr, left, right, pivot);
				if(partition == k) {
					return arr[k];
				}
				if(k < partition) {
					right = partition - 1;
				} else {
					left = partition + 1;
				}
			}
			return arr[left];
		}

		private static int SelectPivot(List<decimal> arr, int left, int right)
		{
			int mid = left + (right - left) / 2;
			decimal a = arr[left], b = arr[mid], c = arr[right];
			if(a < b) {
				if(b < c) return mid;
				if(a < c) return right;
				return left;
			}
			if(a < c) return left;
			if(b < c) return right;
			return mid;
		}

		private static int Partition(List<decimal> arr, int left, int right, int pivotIndex)
		{
			decimal pivot = arr[pivotIndex];
			(arr[pivotIndex], arr[right]) = (arr[right], arr[pivotIndex]);
			int storeIndex = left;
			for(int i = left; i < right; i++) {
				if(arr[i] < pivot) {
					(arr[storeIndex], arr[i]) = (arr[i], arr[storeIndex]);
					storeIndex++;
				}
			}
			(arr[storeIndex], arr[right]) = (arr[right], arr[storeIndex]);
			return storeIndex;
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			if(funcs.Length == 1) {
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}
}
