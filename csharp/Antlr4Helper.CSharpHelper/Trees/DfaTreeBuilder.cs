using Antlr4Helper.CSharpHelper.DFAs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr4Helper.CSharpHelper.Trees
{
	internal class DfaTreeBuilder
	{
		public static DfaTree Build(DFA dfa, int skipMinIndex)
		{
			return Build(dfa.States, dfa._dict, skipMinIndex);
		}

		public static DfaTree Build(List<DFAState> states, char[] dict, int skipMinIndex)
		{
			List<TrieNodeEx> nodes = new List<TrieNodeEx>(states.Count);
			for(int i = 0; i < states.Count; i++) {
				nodes.Add(new TrieNodeEx() { Index = i });
			}
			//var count = 0;
			for(int i = 0; i < states.Count; i++) {
				var state = states[i];
				var node = nodes[i];
				foreach(var (key, value) in state.Transitions) {
					nodes[value.Id].Char = key;
					node.Add(key, nodes[value.Id]);
					//count++;
				}
				if(state.AcceptId > skipMinIndex) {
					node.SetResults(1);
				} else {
					node.SetResults(state.AcceptId);
				}
			}

			DfaTree dfaTree = new DfaTree();
			dfaTree.setDict(dict);
			dfaTree.build(nodes);
			return dfaTree;
		}


	}
}
